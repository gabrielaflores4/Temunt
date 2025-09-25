using Microsoft.AspNetCore.Mvc;

namespace Temunt.Controllers
{
    public class DashboardAdmin : Controller
    {
        public IActionResult IndexA()
        {
            return View();
        }
    }
}
