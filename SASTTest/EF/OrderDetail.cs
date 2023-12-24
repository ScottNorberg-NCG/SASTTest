using System;
using System.Collections.Generic;

namespace SASTTest.EF;

public partial class OrderDetail
{
    public int OrderDetailID { get; set; }

    public int OrderID { get; set; }

    public int FoodGroupID { get; set; }

    public int Quantity { get; set; }

    public virtual FoodGroup FoodGroup { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
