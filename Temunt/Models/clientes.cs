using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Temunt.Models
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Column("nombre")]
        [Required] 
        [StringLength(100)]
        public string Nombre { get; set; }

        [Column("empresa")]
        [StringLength(100)]
        public string Empresa { get; set; }

        [Column("contacto")]
        [StringLength(100)]
        public string Contacto { get; set; }

        [Column("telefono")]
        [StringLength(20)]
        public string Telefono { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; }
    }
}
