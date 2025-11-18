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
            List<PacienteExecucaoStruct> pacSimulacao = new List<PacienteExecucaoStruct>();
            long nMedicos = 0;
            long trocasContexto = 0;
            long totalPacientes = 0;
            double mediaEspera = 0;
            double mediaExecucao = 0;
            double mediaCPU = 0;
            long momento = 0;
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
                        tempoSaida = 0
                    };

                    listPacientes.Add(novoP);
                }

                listPacientes = _pacienteRepository.CreateRange(listPacientes);

                totalPacientes = listPacientes.Count();

                pacSimulacao = listPacientes.Select(p => new PacienteExecucaoStruct()
                {
                    idPaciente = p.idPaciente,
                    idExecucao = p.idExecucao,
                    tempoChegada = p.tempoChegada,
                    tempoSaida = p.tempoSaida,
                    duracao = p.duracao,
                    idPrioridadeManchester = p.idPrioridadeManchester,
                    quantum = p.quantum,
                    inicio = -1,
                    fim = -1,
                    executado = false
                }).ToList();

                if (request.idAlgoritmo == (long)EscalonadorEnum.RoundRobin)
                {

                }


                else if (request.idAlgoritmo == (long)EscalonadorEnum.PrioridadeNaoPreemptivo)
                {

                }


                else if (request.idAlgoritmo == (long)EscalonadorEnum.ShortestRemainingTimeFirst)
                {

                }


                else if (request.idAlgoritmo == (long)EscalonadorEnum.ShortestJobFirst)
                {


                    pacSimulacao = pacSimulacao.OrderBy(p => p.duracao).ToList();

                    var pacAtendimento = pacSimulacao.Take((int)nMedicos).ToList();

                    foreach(var p in pacSimulacao)
                    {
                        if (pacAtendimento.Select(x => x.idPaciente).ToList().Contains(p.idPaciente))
                        {

                        }
                        long contadorMedico = 1;
                        EscalonadorExecucao escalonadorExecucao = new EscalonadorExecucao();
                        escalonadorExecucao.idExecucao = p.idExecucao;
                        escalonadorExecucao.idPaciente = p.idPaciente;
                        escalonadorExecucao.contador_medico = pacAtendimento.Select(x => x.idPaciente).ToList().Contains(p.idPaciente) ? contadorMedico : null;
                        escalonadorExecucao.inicio = true;
                        escalonadorExecucao.fim = false;
                        escalonadorExecucao.espera = false;
                    }
                }

                eventos = _escalonadorExecucaoRepository.CreateRange(eventos);

                double somaEspera = 0;
                double somaExecucao = 0;

                foreach (var pac in pacSimulacao)
                {
                    long espera = pac.fim - pac.tempoChegada - (pac.fim - pac.inicio);
                    if (espera < 0) espera = 0;

                    somaEspera += espera;
                    somaExecucao += (pac.fim - pac.tempoChegada);
                }

                if (totalPacientes > 0)
                {
                    mediaEspera = somaEspera / totalPacientes;
                    mediaExecucao = somaExecucao / totalPacientes;
                }


                execucao.mediaEspera = mediaEspera;
                execucao.mediaExecucao = mediaExecucao;
                execucao.mediaCPU = mediaCPU;
                execucao.nTrocasContexto = trocasContexto;

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