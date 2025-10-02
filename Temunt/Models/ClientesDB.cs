using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Temunt.Models
{
    public class clientes
    {
        [Key]
        [Column("id_cliente")]
        public int id_cliente { get; set; }

        [Column("nombre")]
        public string nombre { get; set; }

        [Column("empresa")]
        public string empresa { get; set; }

        [Column("contacto")]
        public string contacto { get; set; }

        [Column("telefono")]
        public string telefono { get; set; }

        [Column("email")]
        public string email { get; set; }
    }
}