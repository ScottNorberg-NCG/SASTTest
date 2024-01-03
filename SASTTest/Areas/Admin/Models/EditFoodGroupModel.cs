using Microsoft.AspNetCore.Mvc;

namespace SASTTest.Areas.Admin.Models
{
    public class EditFoodGroupModel
    {
        public int FoodGroupID { get; set; }
        public string Text { get; set; }
        public float Price { get; set; }
    }
}
