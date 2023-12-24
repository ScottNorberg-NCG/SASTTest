using System;
using System.Collections.Generic;

namespace SASTTest.EF;

public partial class NutritionFacts_Raw
{
    public int Database_Number { get; set; }

    public string Food_Group { get; set; } = null!;

    public string Food_Name { get; set; } = null!;

    public double? Protein_g { get; set; }

    public double? Fat_g { get; set; }

    public double? Carbohydrates_g { get; set; }

    public double? Ash_g { get; set; }

    public int? Calories { get; set; }

    public string? Starch_g { get; set; }

    public string? Sucrose_g { get; set; }

    public string? Glucose_g { get; set; }

    public string? Fructose_g { get; set; }

    public string? Lactose_g { get; set; }

    public string? Maltose_g { get; set; }

    public double? Alcohol_g { get; set; }

    public double? Water_g { get; set; }

    public short? Caffeine_mg { get; set; }

    public short? Theobromine_mg { get; set; }

    public double? Sugar_g { get; set; }

    public string? Galactose_g { get; set; }

    public double? Fiber_g { get; set; }

    public int? Calcium_mg { get; set; }

    public double? Iron_mg { get; set; }

    public int? Magnesium_mg { get; set; }

    public int? Phosphorus_mg { get; set; }

    public int? Potasssium_mg { get; set; }

    public int? Sodium_mg { get; set; }

    public double? Zinc_mg { get; set; }

    public double? Copper_mg { get; set; }

    public string? Fluoride_mcg { get; set; }

    public double? Manganese_mg { get; set; }

    public double? Selenium_mcg { get; set; }

    public int? Vitamin_A_IU { get; set; }

    public int? Retinol_mcg { get; set; }

    public int? Retinol_Equivalents_mcg { get; set; }

    public int? Beta_Carotene_mcg { get; set; }

    public int? Alpha_Carotene_mcg { get; set; }

    public double? Vitamin_E_mg { get; set; }

    public double? Vitamin_D_mcg { get; set; }

    public string? Vitamin_D2_Ergocalciferol_mcg { get; set; }

    public string? Vitamin_D3_Cholecalciferol_mcg { get; set; }

    public int? Beta_Cryptoxanthin_mcg { get; set; }

    public int? Lycopene_mcg { get; set; }

    public int? Lutein_and_Zeaxanthin_mcg { get; set; }

    public double? Vitamin_C_mg { get; set; }

    public double? Thiamin_B1_mg { get; set; }

    public double? Riboflavin_B2_mg { get; set; }

    public double? Niacin_B3_mg { get; set; }

    public double? Vitamin_B5_mg { get; set; }

    public double? Vitamin_B6_mg { get; set; }

    public int? Folate_B9_mg { get; set; }

    public double? Vitamin_B12 { get; set; }

    public double? Choline_mg { get; set; }

    public int? Cholesterol_mg { get; set; }

    public double? Saturated_Fat_g { get; set; }

    public double? Net_Carbs { get; set; }
}
