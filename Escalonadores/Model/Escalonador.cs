using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Escalonadores.Model
{
    [Table("escalonador")]
    public class Escalonador
    {
        [Key]
        [Column("id_escalonador")]
        public long idEscalonador { get; set; }

        [Column("nome_escalonador")]
        public string? nomeEscalonador { get; set; }
    }
}
