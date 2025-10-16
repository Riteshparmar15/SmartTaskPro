using Microsoft.AspNetCore.Mvc;

namespace SmartTaskPro.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
