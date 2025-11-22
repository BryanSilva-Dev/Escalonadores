using Escalonadores.Model;
using Escalonadores.Request;
using Escalonadores.Response;
using Escalonadores.Struct;
using Escalonadores.Struct.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

namespace Escalonadores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EscalonadorHospitalController : ControllerBaseAuthorized
    {
        public EscalonadorHospitalController(Context context) : base(context)
        {

        }

        [HttpPost]
        public IActionResult Post([FromBody] EscalonadorHospitalRequest request)
        {

            EscalonadorHospitalResponse response = new EscalonadorHospitalResponse();
            List<EscalonadorExecucao> eventos = new List<EscalonadorExecucao>();
            List<Paciente> listPacientes = new List<Paciente>();
            List<Paciente> listPacientesAtualizados = new List<Paciente>();
            List<PacienteExecucaoStruct> pacSimulacao = new List<PacienteExecucaoStruct>();
            List<PacienteExecucaoStruct> pacAtendidos = new List<PacienteExecucaoStruct>();
            List<PacienteExecucaoStruct> pacRemocao = new List<PacienteExecucaoStruct>();
            long nMedicosDisponiveis = 0;
            long nMedicosTotais = 0;
            long trocasContexto = 0;
            long totalPacientes = 0;
            long momento = 0;
            long pacientesAtendidos = 0;
            bool ocioso = false;

            try
            {
                Execucao execucao = new Execucao();
                execucao.idAlgoritmo = request.idAlgoritmo;
                execucao.nMedicos = request.nMedicos;
                execucao.dataExecucao = DateTime.Now;

                execucao = _execucaoRepository.Create(execucao);

                foreach (PacienteStruct paciente in request.listPacientes)
                {
                    Paciente novoP = new Paciente()
                    {
                        idExecucao = execucao.idExecucao,
                        tempoChegada = paciente.tempoChegada,
                        duracao = paciente.duracao,
                        idPrioridadeManchester = paciente.idPrioridadeManchester,
                        quantum = paciente.quantum,
                        tempoSaida = 0,
                        tempoEspera = 0,
                        duracaoTotal = 0,
                    };

                    listPacientes.Add(novoP);
                }

                listPacientes = _pacienteRepository.CreateRange(listPacientes);

                totalPacientes = listPacientes.Count();

                nMedicosDisponiveis = request.nMedicos;
                nMedicosTotais = request.nMedicos;

                pacSimulacao = listPacientes.Select(p => new PacienteExecucaoStruct()
                {
                    idPaciente = p.idPaciente,
                    idExecucao = p.idExecucao,
                    tempoChegada = p.tempoChegada,
                    tempoSaida = p.tempoSaida,
                    duracao = p.duracao,
                    idPrioridadeManchester = p.idPrioridadeManchester,
                    quantum = p.quantum,
                    emAtendimento = false,
                    tempoEspera = 0,
                    inicio = 0,
                    fim = 0,
                    duracaoTotal = 0,
                    nAtendimentos = 0,
                }).ToList();

                if (request.idAlgoritmo == (long)EscalonadorEnum.RoundRobin)
                {
                    var pacChegada = pacSimulacao.Where(x => x.tempoChegada == momento).ToList();

                    while (pacientesAtendidos != totalPacientes)
                    {
                        pacRemocao = new List<PacienteExecucaoStruct>();

                        if (momento != 0)
                        {
                            var novaChegada = pacSimulacao.Where(x => x.tempoChegada == momento).ToList();

                            if(novaChegada != null && novaChegada.Count() > 0)
                                pacChegada.AddRange(novaChegada);

                            pacChegada = pacChegada.OrderByDescending(x => x.emAtendimento).ThenBy(x => x.nAtendimentos).ToList();
                        }

                        foreach (var pac in pacChegada)
                        {
                            if (!pac.emAtendimento && nMedicosDisponiveis > 0)
                            {
                                pac.emAtendimento = true;
                                trocasContexto = trocasContexto + 1;
                                pac.inicio = momento;
                                pac.duracao = pac.duracao - 1;
                                pac.quantum = pac.quantum - 1;
                                pac.contadorMedico = nMedicosDisponiveis;
                                pac.nAtendimentos = pac.nAtendimentos + 1;
                                nMedicosDisponiveis = nMedicosDisponiveis - 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = pac.contadorMedico;
                                escalonadorExecucao.inicio = true;
                                escalonadorExecucao.fim = false;
                                escalonadorExecucao.espera = false;
                                escalonadorExecucao.momento = momento;

                                eventos.Add(escalonadorExecucao);
                            }

                            else if (!pac.emAtendimento && nMedicosDisponiveis == 0)
                            {
                                pac.tempoEspera = pac.tempoEspera + 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = null;
                                escalonadorExecucao.inicio = false;
                                escalonadorExecucao.fim = false;
                                escalonadorExecucao.espera = true;
                                escalonadorExecucao.momento = momento;

                                eventos.Add(escalonadorExecucao);

                            }

                            else if (pac.emAtendimento && pac.duracao > 0 && pac.quantum > 0)
                            {
                                pac.duracao = pac.duracao - 1;
                                pac.quantum = pac.quantum - 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = pac.idPaciente;
                                escalonadorExecucao.inicio = false;
                                escalonadorExecucao.fim = false;
                                escalonadorExecucao.espera = false;
                                escalonadorExecucao.momento = momento;
                            }

                            else if (pac.emAtendimento && pac.duracao > 0 && pac.quantum == 0)
                            {
                                pac.emAtendimento = false;
                                pac.quantum = pacSimulacao.Where(x => x.idPaciente == pac.idPaciente).Select(x => x.quantum).FirstOrDefault();
                                pac.tempoEspera = pac.tempoEspera + 1;
                                pac.contadorMedico = null;

                                nMedicosDisponiveis = nMedicosDisponiveis + 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = null;
                                escalonadorExecucao.inicio = false;
                                escalonadorExecucao.fim = false;
                                escalonadorExecucao.espera = true;
                                escalonadorExecucao.momento = momento;
                            }

                            else if (pac.emAtendimento && pac.duracao == 0)
                            {
                                pac.emAtendimento = false;
                                pac.fim = momento;
                                pac.tempoSaida = momento;
                                pac.duracao = pacSimulacao.Where(x => x.idPaciente == pac.idPaciente).Select(x => x.duracao).FirstOrDefault();
                                pac.duracaoTotal = pac.fim - pac.inicio;
                                pac.contadorMedico = null;

                                pacAtendidos.Add(pac);

                                nMedicosDisponiveis = nMedicosDisponiveis + 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = pac.contadorMedico;
                                escalonadorExecucao.inicio = false;
                                escalonadorExecucao.fim = true;
                                escalonadorExecucao.espera = false;
                                escalonadorExecucao.momento = momento;

                                eventos.Add(escalonadorExecucao);

                                pacRemocao.Add(pac);
                            }

                            //Processador (médico) ocioso
                            else
                            {
                                ocioso = true;
                            }
                        }

                        pacChegada.RemoveAll(p => pacRemocao.Contains(p));

                        if (ocioso)
                        {
                            ocioso = false;

                            EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                            escalonadorExecucao.idExecucao = execucao.idExecucao;
                            escalonadorExecucao.idPaciente = null;
                            escalonadorExecucao.contador_medico = null;
                            escalonadorExecucao.inicio = false;
                            escalonadorExecucao.fim = false;
                            escalonadorExecucao.espera = false;
                            escalonadorExecucao.momento = momento;

                            eventos.Add(escalonadorExecucao);
                        }

                        momento++;
                    }

                    foreach (var pac in pacAtendidos)
                    {
                        var paciente = listPacientes.Where(x => x.idPaciente == pac.idPaciente).FirstOrDefault();

                        paciente.tempoSaida = pac.tempoSaida;
                        paciente.tempoEspera = pac.tempoEspera;
                        paciente.duracaoTotal = pac.duracaoTotal;

                        listPacientesAtualizados.Add(paciente);
                    }

                    _pacienteRepository.UpdateRange(listPacientesAtualizados);

                }


                else if (request.idAlgoritmo == (long)EscalonadorEnum.PrioridadeNaoPreemptivo)
                {
                    var pacChegada = pacSimulacao.Where(x => x.tempoChegada == momento).ToList();

                    while (pacientesAtendidos != totalPacientes)
                    {
                        pacRemocao = new List<PacienteExecucaoStruct>();

                        if (momento != 0)
                        {
                            var novaChegada = pacSimulacao.Where(x => x.tempoChegada == momento).ToList();

                            if (novaChegada != null && novaChegada.Count() > 0)
                                pacChegada.AddRange(novaChegada);

                            pacChegada = pacChegada.OrderByDescending(x => x.emAtendimento).ThenByDescending(x => x.idPrioridadeManchester).ToList();
                        }

                        else
                        {
                            pacChegada = pacChegada.OrderByDescending(x => x.duracao).ToList();
                        }

                        foreach (var pac in pacChegada)
                        {
                            if (!pac.emAtendimento && nMedicosDisponiveis > 0)
                            {
                                pac.emAtendimento = true;
                                trocasContexto = trocasContexto + 1;
                                pac.inicio = momento;
                                pac.duracao = pac.duracao - 1;
                                pac.contadorMedico = nMedicosDisponiveis;
                                nMedicosDisponiveis = nMedicosDisponiveis - 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = pac.contadorMedico;
                                escalonadorExecucao.inicio = true;
                                escalonadorExecucao.fim = false;
                                escalonadorExecucao.espera = false;
                                escalonadorExecucao.momento = momento;

                                eventos.Add(escalonadorExecucao);
                            }

                            else if (!pac.emAtendimento && nMedicosDisponiveis == 0)
                            {
                                pac.tempoEspera = pac.tempoEspera + 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = null;
                                escalonadorExecucao.inicio = false;
                                escalonadorExecucao.fim = false;
                                escalonadorExecucao.espera = true;
                                escalonadorExecucao.momento = momento;

                                eventos.Add(escalonadorExecucao);

                            }

                            else if (pac.emAtendimento && pac.duracao > 0)
                            {
                                pac.duracao = pac.duracao - 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = pac.contadorMedico;
                                escalonadorExecucao.inicio = false;
                                escalonadorExecucao.fim = false;
                                escalonadorExecucao.espera = false;
                                escalonadorExecucao.momento = momento;
                            }

                            else if (pac.emAtendimento && pac.duracao == 0)
                            {
                                pac.emAtendimento = false;
                                pac.fim = momento;
                                pac.tempoSaida = momento;
                                pac.duracao = pacSimulacao.Where(x => x.idPaciente == pac.idPaciente).Select(x => x.duracao).FirstOrDefault();
                                pac.duracaoTotal = pac.fim - pac.inicio;
                                pac.contadorMedico = null;

                                pacAtendidos.Add(pac);

                                nMedicosDisponiveis = nMedicosDisponiveis + 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = pac.contadorMedico;
                                escalonadorExecucao.inicio = false;
                                escalonadorExecucao.fim = true;
                                escalonadorExecucao.espera = false;
                                escalonadorExecucao.momento = momento;

                                eventos.Add(escalonadorExecucao);

                                pacRemocao.Add(pac);
                            }

                            //Processador (médico) ocioso
                            else
                            {
                                ocioso = true;
                            }
                        }

                        pacChegada.RemoveAll(p => pacRemocao.Contains(p));

                        if (ocioso)
                        {
                            ocioso = false;

                            EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                            escalonadorExecucao.idExecucao = execucao.idExecucao;
                            escalonadorExecucao.idPaciente = null;
                            escalonadorExecucao.contador_medico = null;
                            escalonadorExecucao.inicio = false;
                            escalonadorExecucao.fim = false;
                            escalonadorExecucao.espera = false;
                            escalonadorExecucao.momento = momento;

                            eventos.Add(escalonadorExecucao);
                        }

                        momento++;
                    }

                    foreach (var pac in pacAtendidos)
                    {
                        var paciente = listPacientes.Where(x => x.idPaciente == pac.idPaciente).FirstOrDefault();

                        paciente.tempoSaida = pac.tempoSaida;
                        paciente.tempoEspera = pac.tempoEspera;
                        paciente.duracaoTotal = pac.duracaoTotal;

                        listPacientesAtualizados.Add(paciente);
                    }

                    _pacienteRepository.UpdateRange(listPacientesAtualizados);

                }


                else if (request.idAlgoritmo == (long)EscalonadorEnum.ShortestRemainingTimeFirst)
                {
                    var pacChegada = pacSimulacao.Where(x => x.tempoChegada == momento).ToList();

                    while (pacientesAtendidos != totalPacientes)
                    {
                        pacRemocao = new List<PacienteExecucaoStruct>();

                        if (momento != 0)
                        {
                            var novaChegada = pacSimulacao.Where(x => x.tempoChegada == momento).ToList();

                            if (novaChegada != null && novaChegada.Count() > 0)
                                pacChegada.AddRange(novaChegada);

                            pacChegada = pacChegada.OrderBy(x => x.duracao).ThenBy(x => x.tempoChegada).ToList();
                        }

                        else
                        {
                            pacChegada = pacChegada.OrderBy(x => x.duracao).ThenBy(x => x.tempoChegada).ToList();
                        }

                        var listAtendimento = pacChegada.Where(x => x.emAtendimento).ToList();

                        if(listAtendimento != null && listAtendimento.Count() > 0)
                        {
                            foreach(var atend in listAtendimento)
                            {
                                if(!pacChegada.Take((int)nMedicosTotais - (int)nMedicosDisponiveis).Contains(atend))
                                {
                                    atend.emAtendimento = false;
                                    atend.tempoEspera = atend.tempoEspera + 1;
                                    atend.contadorMedico = null;
                                    atend.preemptado = true;
                                    nMedicosDisponiveis = nMedicosDisponiveis + 1;

                                    EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                    escalonadorExecucao.idExecucao = atend.idExecucao;
                                    escalonadorExecucao.idPaciente = atend.idPaciente;
                                    escalonadorExecucao.contador_medico = null;
                                    escalonadorExecucao.inicio = false;
                                    escalonadorExecucao.fim = false;
                                    escalonadorExecucao.espera = true;
                                    escalonadorExecucao.momento = momento;
                                }
                            }
                        }

                        foreach (var pac in pacChegada)
                        {
                            if(!pac.preemptado)
                            {
                                if (!pac.emAtendimento && nMedicosDisponiveis > 0)
                                {
                                    pac.emAtendimento = true;
                                    trocasContexto = trocasContexto + 1;
                                    pac.inicio = momento;
                                    pac.duracao = pac.duracao - 1;
                                    pac.contadorMedico = nMedicosDisponiveis;
                                    pac.nAtendimentos = pac.nAtendimentos + 1;
                                    nMedicosDisponiveis = nMedicosDisponiveis - 1;

                                    EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                    escalonadorExecucao.idExecucao = pac.idExecucao;
                                    escalonadorExecucao.idPaciente = pac.idPaciente;
                                    escalonadorExecucao.contador_medico = pac.contadorMedico;
                                    escalonadorExecucao.inicio = true;
                                    escalonadorExecucao.fim = false;
                                    escalonadorExecucao.espera = false;
                                    escalonadorExecucao.momento = momento;

                                    eventos.Add(escalonadorExecucao);
                                }

                                else if (!pac.emAtendimento && nMedicosDisponiveis == 0)
                                {
                                    pac.tempoEspera = pac.tempoEspera + 1;

                                    EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                    escalonadorExecucao.idExecucao = pac.idExecucao;
                                    escalonadorExecucao.idPaciente = pac.idPaciente;
                                    escalonadorExecucao.contador_medico = null;
                                    escalonadorExecucao.inicio = false;
                                    escalonadorExecucao.fim = false;
                                    escalonadorExecucao.espera = true;
                                    escalonadorExecucao.momento = momento;

                                    eventos.Add(escalonadorExecucao);

                                }

                                else if (pac.emAtendimento && pac.duracao > 0)
                                {
                                    pac.duracao = pac.duracao - 1;

                                    EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                    escalonadorExecucao.idExecucao = pac.idExecucao;
                                    escalonadorExecucao.idPaciente = pac.idPaciente;
                                    escalonadorExecucao.contador_medico = pac.contadorMedico;
                                    escalonadorExecucao.inicio = false;
                                    escalonadorExecucao.fim = false;
                                    escalonadorExecucao.espera = false;
                                    escalonadorExecucao.momento = momento;
                                }

                                else if (pac.emAtendimento && pac.duracao == 0)
                                {
                                    pac.emAtendimento = false;
                                    pac.fim = momento;
                                    pac.tempoSaida = momento;
                                    pac.duracao = pacSimulacao.Where(x => x.idPaciente == pac.idPaciente).Select(x => x.duracao).FirstOrDefault();
                                    pac.duracaoTotal = pac.fim - pac.inicio;
                                    pac.contadorMedico = null;

                                    pacAtendidos.Add(pac);

                                    nMedicosDisponiveis = nMedicosDisponiveis + 1;

                                    EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                    escalonadorExecucao.idExecucao = pac.idExecucao;
                                    escalonadorExecucao.idPaciente = pac.idPaciente;
                                    escalonadorExecucao.contador_medico = pac.contadorMedico;
                                    escalonadorExecucao.inicio = false;
                                    escalonadorExecucao.fim = true;
                                    escalonadorExecucao.espera = false;
                                    escalonadorExecucao.momento = momento;

                                    eventos.Add(escalonadorExecucao);

                                    pacRemocao.Add(pac);
                                }

                                //Processador (médico) ocioso
                                else
                                {
                                    ocioso = true;
                                }
                            }
                            pac.preemptado = false;
                        }

                        if(pacRemocao != null && pacRemocao.Count() > 0)
                            pacChegada.RemoveAll(p => pacRemocao.Contains(p));

                        if (ocioso)
                        {
                            ocioso = false;

                            EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                            escalonadorExecucao.idExecucao = execucao.idExecucao;
                            escalonadorExecucao.idPaciente = null;
                            escalonadorExecucao.contador_medico = null;
                            escalonadorExecucao.inicio = false;
                            escalonadorExecucao.fim = false;
                            escalonadorExecucao.espera = false;
                            escalonadorExecucao.momento = momento;

                            eventos.Add(escalonadorExecucao);
                        }

                        momento++;
                    }

                    foreach (var pac in pacAtendidos)
                    {
                        var paciente = listPacientes.Where(x => x.idPaciente == pac.idPaciente).FirstOrDefault();

                        paciente.tempoSaida = pac.tempoSaida;
                        paciente.tempoEspera = pac.tempoEspera;
                        paciente.duracaoTotal = pac.duracaoTotal;

                        listPacientesAtualizados.Add(paciente);
                    }

                    _pacienteRepository.UpdateRange(listPacientesAtualizados);

                }


                else if (request.idAlgoritmo == (long)EscalonadorEnum.ShortestJobFirst)
                {
                    var pacChegada = pacSimulacao.Where(x => x.tempoChegada == momento).ToList();

                    while(pacientesAtendidos != totalPacientes)
                    {
                        pacRemocao = new List<PacienteExecucaoStruct>();

                        if(momento != 0)
                        {
                            var novaChegada = pacSimulacao.Where(x => x.tempoChegada == momento).ToList();

                            if (novaChegada != null && novaChegada.Count() > 0)
                                pacChegada.AddRange(novaChegada);

                            pacChegada = pacChegada.OrderByDescending(x => x.emAtendimento).ThenBy(x => x.duracao).ToList();
                        }

                        else
                        {
                            pacChegada = pacChegada.OrderByDescending(x => x.duracao).ToList();
                        }

                        foreach(var pac in pacChegada)
                        {
                            if(!pac.emAtendimento && nMedicosDisponiveis > 0)
                            {
                                pac.emAtendimento = true;
                                trocasContexto = trocasContexto + 1;
                                pac.inicio = momento;
                                pac.duracao = pac.duracao - 1;
                                pac.contadorMedico = nMedicosDisponiveis;
                                nMedicosDisponiveis = nMedicosDisponiveis - 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = pac.contadorMedico;
                                escalonadorExecucao.inicio = true;
                                escalonadorExecucao.fim = false;
                                escalonadorExecucao.espera = false;
                                escalonadorExecucao.momento = momento;

                                eventos.Add(escalonadorExecucao);
                            }

                            else if(!pac.emAtendimento && nMedicosDisponiveis == 0)
                            {
                                pac.tempoEspera = pac.tempoEspera + 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = null;
                                escalonadorExecucao.inicio = false;
                                escalonadorExecucao.fim = false;
                                escalonadorExecucao.espera = true;
                                escalonadorExecucao.momento = momento;

                                eventos.Add(escalonadorExecucao);

                            }

                            else if(pac.emAtendimento && pac.duracao > 0)
                            {
                                pac.duracao = pac.duracao - 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = pac.contadorMedico;
                                escalonadorExecucao.inicio = false;
                                escalonadorExecucao.fim = false;
                                escalonadorExecucao.espera = false;
                                escalonadorExecucao.momento = momento;
                            }

                            else if(pac.emAtendimento && pac.duracao == 0)
                            {
                                pac.emAtendimento = false;
                                pac.fim = momento;
                                pac.tempoSaida = momento;
                                pac.duracao = pacSimulacao.Where(x => x.idPaciente == pac.idPaciente).Select(x => x.duracao).FirstOrDefault();
                                pac.duracaoTotal = pac.fim - pac.inicio;
                                pac.contadorMedico = null;

                                pacAtendidos.Add(pac);

                                nMedicosDisponiveis = nMedicosDisponiveis + 1;

                                EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                                escalonadorExecucao.idExecucao = pac.idExecucao;
                                escalonadorExecucao.idPaciente = pac.idPaciente;
                                escalonadorExecucao.contador_medico = pac.contadorMedico;
                                escalonadorExecucao.inicio = false;
                                escalonadorExecucao.fim = true;
                                escalonadorExecucao.espera = false;
                                escalonadorExecucao.momento = momento;

                                eventos.Add(escalonadorExecucao);

                                pacRemocao.Add(pac);
                            }

                            //Processador (médico) ocioso
                            else
                            {
                                ocioso = true;
                            }
                        }

                        if (pacRemocao != null && pacRemocao.Count() > 0)
                            pacChegada.RemoveAll(p => pacRemocao.Contains(p));

                        if (ocioso)
                        {
                            ocioso = false;

                            EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                            escalonadorExecucao.idExecucao = execucao.idExecucao;
                            escalonadorExecucao.idPaciente = null;
                            escalonadorExecucao.contador_medico = null;
                            escalonadorExecucao.inicio = false;
                            escalonadorExecucao.fim = false;
                            escalonadorExecucao.espera = false;
                            escalonadorExecucao.momento = momento;

                            eventos.Add(escalonadorExecucao);
                        }

                        momento++;
                    }

                    foreach(var pac in pacAtendidos)
                    {
                        var paciente = listPacientes.Where(x => x.idPaciente == pac.idPaciente).FirstOrDefault();

                        paciente.tempoSaida = pac.tempoSaida;
                        paciente.tempoEspera = pac.tempoEspera;
                        paciente.duracaoTotal = pac.duracaoTotal;

                        listPacientesAtualizados.Add(paciente);
                    }

                    _pacienteRepository.UpdateRange(listPacientesAtualizados);

                }

                eventos = _escalonadorExecucaoRepository.CreateRange(eventos);

                execucao.nTrocasContexto = trocasContexto;

                long totalExecucoes = eventos.Count();

                long totalEspera = eventos.Where(x => x.espera).Count();

                long cpuTotal = eventos.Where(x => x.inicio || x.fim || x.contador_medico != null).Count();

                long totalAtendimento = pacAtendidos.Select(x => x.duracaoTotal).ToList().Sum();

                execucao.mediaEspera = totalExecucoes / totalEspera;

                execucao.mediaExecucao = totalAtendimento / totalPacientes;

                execucao.mediaCPU = cpuTotal / totalExecucoes;

                _execucaoRepository.Update(execucao);

                response.execucao = execucao;
                response.eventos = eventos;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.isError = true;
                response.errorDescription = ex.Message;
                return Ok(response);
            }
        }
    }
}