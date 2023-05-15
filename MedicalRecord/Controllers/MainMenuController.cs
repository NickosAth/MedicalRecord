using Microsoft.AspNetCore.Mvc;

namespace MedicalRecord.Controllers
{
	public class MainMenuController : Controller
	{
		public IActionResult Index()
		{
            return View();
		}
	}
}
