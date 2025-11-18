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

        public long inicio { get; set; }

        public long fim { get; set; }
        public bool executado { get; set; }
    }
}
