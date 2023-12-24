using Microsoft.AspNetCore.Mvc;
using SASTTest.Models;

namespace SASTTest.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class XssController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Bold(string foodName)
        {
            var model = new AccountUserViewModel();
            model.SearchText = foodName;
            return View(foodName);
        }

        public IActionResult Bold_Safe(string foodName)
        {
            var model = new AccountUserViewModel();
            model.SearchText = foodName;
            return View(foodName);
        }

        public IActionResult Italic(string foodName)
        {
            var model = new AccountUserViewModel();
            model.SearchText = foodName;
            return View(foodName);
        }

        public IActionResult Italic_Safe(string foodName)
        {
            var model = new AccountUserViewModel();
            model.SearchText = foodName;
            return View(foodName);
        }

        public IActionResult Raw(string foodName)
        {
            var model = new AccountUserViewModel();
            model.SearchText = foodName;
            return View(foodName);
        }

        public IActionResult Raw_Safe(string foodName)
        {
            var model = new AccountUserViewModel();
            model.SearchText = foodName;

            model.Category = "(Unknown)";
            var lowered = foodName.ToLower();

            if (lowered == "pork" || lowered == "chicken" || lowered == "beef")
                model.Category = "Meats";

            ViewBag.Category = $"<i>{model.Category}</i>";
            return View(foodName);
        }
    }
}
