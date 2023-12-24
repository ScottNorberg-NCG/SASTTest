using Microsoft.Data.SqlClient;
using SASTTest.EF;

namespace SASTTest.Extensions;

public static class SqlDataReaderExtensions
{
    public static List<FoodDisplayView> LoadFoodsFromReader(this SqlDataReader reader)
    { 
        var foods = new List<FoodDisplayView>();

        while (reader.Read())
        {
            var newFood = new FoodDisplayView();

            newFood.FoodID = reader.GetInt32(0);
            newFood.FoodGroupID = reader.GetInt32(1);
            newFood.FoodGroup = reader.GetString(2);
            newFood.FoodName = reader.GetString(3);
            newFood.Calories = reader.GetInt32(4);
            newFood.Protein = reader.GetDouble(5);
            newFood.Fat = reader.GetDouble(6);
            newFood.Carbohydrates = reader.GetDouble(7);

            foods.Add(newFood);
        }

        return foods;
    }
}
