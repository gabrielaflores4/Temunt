using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Temunt.Models
{
    public class producto


    {
        [Key]
        public int id_prod { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public int id_cat { get; set; }
        public int id_prov { get; set; }

        [ForeignKey("id_cat")]
        [ValidateNever]
        public categorias? categorias { get; set; }

        [ForeignKey("id_prov")]
        [ValidateNever]
        public proveedores? proveedores { get; set; }
    }
}
