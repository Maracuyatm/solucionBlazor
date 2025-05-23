using Microsoft.AspNetCore.Mvc;

namespace BlazorCrud.Server.Controllers
{
    public class EstadoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
