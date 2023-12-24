using System;
using System.Collections.Generic;

namespace SASTTest.EF;

public partial class FoodGroup
{
    public int FoodGroupID { get; set; }

    public string FoodGroupText { get; set; } = null!;

    public double? Price { get; set; }

    public virtual ICollection<Food> Foods { get; set; } = new List<Food>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
