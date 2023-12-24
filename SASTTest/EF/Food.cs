using System;
using System.Collections.Generic;

namespace SASTTest.EF;

public partial class Food
{
    public int FoodID { get; set; }

    public int FoodGroupID { get; set; }

    public string FoodName { get; set; } = null!;

    public int Calories { get; set; }

    public double Protein { get; set; }

    public double Fat { get; set; }

    public double Carbohydrates { get; set; }

    public virtual FoodGroup FoodGroup { get; set; } = null!;
}
