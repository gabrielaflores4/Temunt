using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Temunt.Models

{
    public class categorias
    {

        [Key]
        public int id_cat { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

    }
}
