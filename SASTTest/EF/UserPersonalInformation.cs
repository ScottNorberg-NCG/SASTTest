using System;
using System.Collections.Generic;

namespace SASTTest.EF;

public partial class UserPersonalInformation
{
    public int UserID { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? EmailAddress { get; set; }

    public virtual SiteUser User { get; set; } = null!;
}
