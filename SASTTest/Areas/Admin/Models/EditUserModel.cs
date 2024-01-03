using System.ComponentModel.DataAnnotations;

namespace SASTTest.Areas.Admin.Models
{
    public class EditUserModel
    {
        [Required]
        public string UserName { get; set; }
        public string FavoriteFood { get; set; }
        public string FavoriteFoodGroup { get; set; }
        public bool IsAdmin { get; set; }
    }
}
