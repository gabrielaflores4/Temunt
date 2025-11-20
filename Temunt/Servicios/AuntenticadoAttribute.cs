using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Temunt.Servicios
{
    public class AutenticadoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var idUsuario = session.GetInt32("id_usuario");

            string controller = context.RouteData.Values["controller"]?.ToString();
            string action = context.RouteData.Values["action"]?.ToString();

            bool esLogin = controller.Equals("Home", StringComparison.OrdinalIgnoreCase)
                           && action.Equals("Index", StringComparison.OrdinalIgnoreCase);

            // Si NO es la página de login y NO hay usuario → redirige
            if (!esLogin && !idUsuario.HasValue)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
