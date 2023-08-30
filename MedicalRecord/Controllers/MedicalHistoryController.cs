using Microsoft.AspNetCore.Mvc;

namespace MedicalRecord.Controllers
{
    public class MedicalHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
