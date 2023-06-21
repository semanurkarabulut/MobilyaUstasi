using Microsoft.AspNetCore.Mvc;

namespace MobilyaUstasi.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
