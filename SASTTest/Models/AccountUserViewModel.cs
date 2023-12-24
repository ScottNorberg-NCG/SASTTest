using SASTTest.EF;

namespace SASTTest.Models;

public class AccountUserViewModel
{
    public string SearchText { get; set; }
    public string Category { get; set; }
    public List<FoodDisplayView> Foods { get; set; } = new List<FoodDisplayView>();
}
