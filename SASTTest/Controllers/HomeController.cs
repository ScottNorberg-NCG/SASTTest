using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SASTTest.EF;
using SASTTest.Models;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace SASTTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly VulnerabilityBuffetContext _dbContext;

    private readonly string _awsAccessKey = "AKIAIOSFODNN7EXAMPLE";
    private readonly string _awsSecretKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";

    public HomeController(ILogger<HomeController> logger, VulnerabilityBuffetContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public IActionResult Index()
    { 
        return View();
    }

    public IActionResult Privacy()
    {
        Response.Cookies.Append("PrivacyViewedNoOptions", "Yes");
        Response.Cookies.Append("PrivacyViewedDefaultOptions", "Yes", new Microsoft.AspNetCore.Http.CookieOptions());

        return View();
    }

    [HttpGet]
    public IActionResult NewFood()
    {
        return View();
    }

    [HttpPost]
    public IActionResult NewFood(Food newFood) 
    {
        _dbContext.Foods.Add(newFood);
        _dbContext.SaveChanges();
        return RedirectToAction("Details", newFood.FoodName);
    }

    [HttpGet]
    public IActionResult Details(string id)
    {
        var food = _dbContext.Foods.SingleOrDefault(f => f.FoodName == id);
        return View(food);
    }

    public IActionResult Cookies()
    {
        var options = new CookieOptions();
        options.Expires = DateTime.Now.AddYears(1);
        options.HttpOnly = false;
        options.SameSite = SameSiteMode.Lax;
        options.IsEssential = true;
        options.Secure = false;

        Response.Cookies.Append("CookieName", "Alex", options);

        return View("Index");
    }

    [HttpGet]
    public IActionResult ReturnDataViaModelAndView_Contains()
    {
        var model = new AccountUserViewModel();
        model.SearchText = "(None)";
        return View(model);
    }

    //Not new vulnerability - uses ExecSQL
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ReturnDataViaModelAndView_Contains([FromForm]string foodName, [FromForm]string foodGroup)
    {
        if (!ModelState.IsValid)
            return View();

        var model = new AccountUserViewModel();
        model.SearchText = $"Food Name: {foodName}, Food Group: {foodGroup}";
        model.Foods = _dbContext.FoodDisplayViews.Where(f => f.FoodName.Contains(foodName) && f.FoodGroup.Contains(foodGroup)).ToList();
        return View(model);
    }

    [HttpGet]
    public IActionResult ReturnDataViaModelAndView_Equals()
    {
        var model = new AccountUserViewModel();
        model.SearchText = "(None)";
        return View(model);
    }

    //Not new vulnerability - uses ExecSQL
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ReturnDataViaModelAndView_Equals([FromForm] string foodName, [FromForm] string foodGroup)
    {
        if (!ModelState.IsValid)
            return View();

        var model = new AccountUserViewModel();
        model.SearchText = $"Food Name: {foodName}, Food Group: {foodGroup}";
        model.Foods = _dbContext.FoodDisplayViews.Where(f => f.FoodName == foodName && f.FoodGroup == foodGroup).ToList();
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
