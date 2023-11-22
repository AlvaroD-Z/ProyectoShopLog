using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ProyectoShopLog.AplicacionWeb.Utilidades.ViewComponents
{
    public class MenuUsuarioViewComponent: ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            string correoUsuario = "";

            if (claimUser.Identity.IsAuthenticated)
            {
                correoUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["nombreUsuario"] = correoUsuario;

            return View();
        }

    }
}
