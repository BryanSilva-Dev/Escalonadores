using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Escalonadores.Model
{
    [Table("prioridade_manchester")]
    public class PrioridadeManchester
    {
        [Key]
        [Column("id_prioridade_manchester")]
        public long idPrioridadeManchester { get; set; }

        [Column("nome_prioridade")]
        public string? nomePrioridade { get; set; }

        [Column("cor")]
        public string? cor { get; set; }
    }
}
