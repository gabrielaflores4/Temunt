using Microsoft.AspNetCore.Mvc;

namespace Temunt.Controllers
{
    public class Usuarios : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
