using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SASTTest.Areas.Admin.Models;
using SASTTest.EF;

namespace SASTTest.Areas.Admin.Controllers;

[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Administrator")]
public class UsersController : Controller
{
    private readonly VulnerabilityBuffetContext _dbContext;

    public UsersController(VulnerabilityBuffetContext dbContext)
    {  
        _dbContext = dbContext; 
    }

    [HttpGet]
    public IActionResult List()
    {
        var users = _dbContext.Users.ToList();
        return View(users);
    }

    [HttpGet]
    public IActionResult Create() 
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create([FromForm]SiteUser user)
    {
        _dbContext.SiteUsers.Add(user);
        _dbContext.SaveChanges();
        return View(user);
    }

    [HttpGet]
    public IActionResult Edit()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Edit([FromForm] EditUserModel model)
    {
        var user = _dbContext.SiteUsers.Single(u => u.UserName == model.UserName);
        user.FavoriteFood = model.FavoriteFood;
        user.FavoriteFoodGroup = model.FavoriteFoodGroup;
        user.IsAdmin = model.IsAdmin;

        _dbContext.SaveChanges();
        return View(user);
    }
}
