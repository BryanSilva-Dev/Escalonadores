using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Escalonadores.Model
{
    [Table("execucao")]
    public class Execucao
    {
        [Key]
        [Column("id_execucao")]
        public long idExecucao { get; set; }

        [Column("id_algoritmo")]
        public long idAlgoritmo { get; set; }

        [Column("n_medicos")]
        public long nMedicos { get; set; }

        [Column("n_trocas_contexto")]
        public long nTrocasContexto { get; set; }

        [Column("media_espera")]
        public double mediaEspera { get; set; }

        [Column("media_execucao")]
        public double mediaExecucao { get; set; }

        [Column("media_cpu")]
        public double mediaCPU { get; set; }

        [Column("data_execucao")]
        public DateTime dataExecucao { get; set; }
    }
}
