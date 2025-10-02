using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Temunt.Models
{
    public class detalleP
    {

        [Key]
        public int id_detalle { get; set; }
        public int cantidad { get; set; }
        public string garantia { get; set; }
        public DateTime? garantiaExp { get; set; }
        public DateTime fechaD { get; set; }
        public int id_pedidos { get; set; }
        public int id_prod { get; set; }

        [ForeignKey("id_prod")]
        public virtual producto producto { get; set; }

        [ForeignKey("id_pedidos")]
        public virtual pedidos pedidos { get; set; }

    }
}
