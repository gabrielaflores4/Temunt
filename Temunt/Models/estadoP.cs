using System.ComponentModel.DataAnnotations;

namespace Temunt.Models
{
    public class estadoP
    {

        [Key]
        public int id_estado { get; set; }
        public string estado { get; set; }
    }
}
