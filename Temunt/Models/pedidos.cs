using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Temunt.Models
{
    public class pedidos
    {


        [Key]
        public int id_pedidos { get; set; }
        public DateTime? fechaP { get; set; }
        public int id_usuario { get; set; }
        public int id_estado { get; set; }
        public int id_cliente { get; set; }

        [ForeignKey("id_usuario")]
        public virtual usuarios usuarios { get; set; }

        [ForeignKey("id_estado")]
        public virtual estadoP estadoP { get; set; }

        [ForeignKey("id_cliente")]
        public virtual clientes clientes { get; set; }

        public virtual ICollection<detalleP> detalleP { get; set; }

        public decimal Monto => detalleP?.Sum(d => d.cantidad) ?? 0;

        [NotMapped]
        public decimal Total => detalleP?.Sum(d => d.cantidad * d.producto.precio) ?? 0;

    }
}
