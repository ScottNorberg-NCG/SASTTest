using System;
using System.Collections.Generic;

namespace SASTTest.EF;

public partial class SiteUser
{
    public int UserID { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string? FavoriteFood { get; set; }

    public string? FavoriteFoodGroup { get; set; }

    public bool IsAdmin { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<UserFile> UserFiles { get; set; } = new List<UserFile>();

    public virtual UserPersonalInformation? UserPersonalInformation { get; set; }
}
