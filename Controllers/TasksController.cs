using Microsoft.AspNetCore.Mvc;

namespace SmartTaskPro.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
