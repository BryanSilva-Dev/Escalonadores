using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Escalonadores.Model
{
    [Table("paciente")]
    public class Paciente
    {
        [Key]
        [Column("id_paciente")]
        public long idPaciente { get; set; }

        [Column("id_execucao")]
        public long idExecucao { get; set; }

        [Column("tempo_chegada")]
        public long tempoChegada { get; set; }

        [Column("tempo_saida")]
        public long tempoSaida { get; set; }

        [Column("duracao")]
        public long duracao { get; set; }

        [Column("id_prioridade_manchester")]
        public long idPrioridadeManchester { get; set; }

        [Column("quantum")]
        public long? quantum { get; set; }
    }
}
