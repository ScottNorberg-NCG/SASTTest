using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SASTTest.EF;

namespace SASTTest.Pages;

public class AddFoodGroupModel : PageModel
{
    private readonly VulnerabilityBuffetContext _dbContext;

    [BindProperty]
    public FoodGroup Input { get; set; }

    public AddFoodGroupModel(VulnerabilityBuffetContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void OnGet()
    {
    }

    public void OnPost()
    {
        _dbContext.FoodGroups.Add(Input);
        _dbContext.SaveChanges();
    }
}
