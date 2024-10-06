using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    public class FishStatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
