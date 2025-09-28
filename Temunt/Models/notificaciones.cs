using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Temunt.Models
{
    public class notificaciones
    {

        [Key]
        public int id_notif { get; set; }
        public string mensaje { get; set; }
        public DateTime fecha { get; set; }
        public int id_usuario { get; set; }

        [ForeignKey(nameof(id_usuario))]
        public usuarios usuarios { get; set; }
    }
}
