using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SASTTest.Areas.Admin.Models;
using SASTTest.EF;

namespace SASTTest.Areas.Admin.Controllers;

public class FoodGroupController : Controller
{
    private readonly VulnerabilityBuffetContext _dbContext;

    public FoodGroupController(VulnerabilityBuffetContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        return View(_dbContext.FoodGroups.Select(g => new { g.FoodGroupID, g.FoodGroupText }).ToList());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromForm] AddFoodGroupModel model)
    {
        var newFoodGroup = new FoodGroup() { FoodGroupText = model.Text, Price = model.Price };
        _dbContext.FoodGroups.Add(newFoodGroup);
        _dbContext.SaveChanges();
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit()
    {
        return View();
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public IActionResult Edit([FromForm] EditFoodGroupModel model)
    {
        var foodGroup = _dbContext.FoodGroups.Single(fg => fg.FoodGroupID == model.FoodGroupID);
        foodGroup.FoodGroupText = model.Text;
        foodGroup.Price = model.Price;

        _dbContext.SaveChanges();
        return View(model);
    }
}
