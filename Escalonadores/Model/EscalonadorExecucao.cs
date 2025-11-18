using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Escalonadores.Model
{
    [Table("escalonador_execucao")]
    public class EscalonadorExecucao
    {
        [Key]
        [Column("id_escalonador_execucao")]
        public long idEscalonadorExecucao { get; set; }

        [Column("id_execucao")]
        public long idExecucao { get; set; }

        [Column("id_paciente")]
        public long idPaciente { get; set; }

        [Column("contador_medico")]
        public long? contador_medico { get; set; }

        [Column("inicio")]
        public bool inicio { get; set; }

        [Column("fim")]
        public bool fim { get; set; }

        [Column("espera")]
        public bool espera { get; set; }

        [Column("momento")]
        public long momento { get; set; }
    }
}
