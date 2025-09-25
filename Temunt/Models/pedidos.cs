using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Temunt.Models
{
    public class pedidos
    {


        [Key]
        public int id_pedidos { get; set; }
        public DateTime fechaP { get; set; }
        public int id_usuario { get; set; }
        public int id_estado { get; set; }

        [ForeignKey("id_usuario")]
        public virtual usuarios usuarios { get; set; }

        [ForeignKey("id_estado")]
        public virtual estadoP estadoP { get; set; }

    }
}
