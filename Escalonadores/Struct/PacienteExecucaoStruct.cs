using Escalonadores.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Escalonadores.Struct
{
    public class PacienteExecucaoStruct
    {
        public PacienteExecucaoStruct()
        {

        }

        public long idPaciente { get; set; }

        public long idExecucao { get; set; }

        public long tempoChegada { get; set; }

        public long tempoSaida { get; set; }

        public long duracao { get; set; }

        public long idPrioridadeManchester { get; set; }

        public long? quantum { get; set; }

        public bool emAtendimento { get; set; }

        public long tempoEspera { get; set; }

        public long? contadorMedico { get; set; }

        public long inicio { get; set; }

        public long fim { get; set; }

        public long duracaoTotal { get; set; }

        public long nAtendimentos { get; set; }

        public bool preemptado { get; set; }

        public long? quantumOriginal { get; set; }
    }
}
