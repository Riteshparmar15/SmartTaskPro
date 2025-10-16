using Microsoft.AspNetCore.Mvc;

namespace SmartTaskPro.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
