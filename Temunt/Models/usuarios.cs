using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Temunt.Models
{
    public class usuarios
    {
        [Key]
        [Column("id_usuario")]  // Aseguramos que 'id_usuarios' corresponde con la columna de la base de datos
        public int id_usuario { get; set; }

        [Column("nombreU")]
        public string nombreU { get; set; }

        [Column("nombreP")]
        public string nombreP { get; set; }

        [Column("direccion")]
        public string direccion { get; set; }

        [Column("email")]
        public string email { get; set; }

        [Column("contra")]
        public string contra { get; set; }

        [Column("telefono")]
        public string telefono { get; set; }

        [Column("roles")]
        public string roles { get; set; }
    }
}
