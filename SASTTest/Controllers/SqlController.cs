using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SASTTest.EF;
using SASTTest.Extensions;
using SASTTest.Models;
using System.Text;

namespace SASTTest.Controllers;

public class SqlController : Controller
{
    VulnerabilityBuffetContext _dbContext;
    IConfiguration _config;

    private readonly string _awsAccessKey = "AKIAIOSFODNN7EXAMPLE";
    private readonly string _awsSecretKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";
    private readonly string _connectionString = "Server=localhost\\SQL2019;Initial Catalog=VulnerabilityBuffet;Persist Security Info=False;User ID=ApplicationLogUser;Password=P@ssw0rd*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

    public SqlController(VulnerabilityBuffetContext context, IConfiguration config)
    {
        _dbContext = context;
        _config = config;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AllStringLineBreak()
    {
        var model = new AccountUserViewModel();
        model.SearchText = "(None)";
        return View(model);
    }

    //SQL Injection vulnerability #1 (via ExecSQL)
    [HttpPost]
    public IActionResult AllStringLineBreak(string foodName, string foodGroup)
    {
        var model = new AccountUserViewModel();
        model.SearchText = $"Food Name: {foodName}, Food Group: {foodGroup}";
        var searchText = @"SELECT * 
                                FROM FoodDisplayView 
                                WHERE FoodName LIKE '%" + foodName + "%' OR " +
                                "FoodGroup LIKE '%" + foodGroup + "%'";

        model.Foods = _dbContext.ExecSQL<FoodDisplayView>(searchText);
        return View(model);
    }

    [HttpGet]
    public IActionResult AllStringLineBreakSafeSecond()
    {
        var model = new AccountUserViewModel();
        model.SearchText = "(None)";
        return View(model);
    }

    //Not new vulnerability - uses ExecSQL
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AllStringLineBreakSafeSecond(string foodName, string foodGroup)
    {
        var model = new AccountUserViewModel();
        model.SearchText = $"Food Name: {foodName}, Food Group: {foodGroup}";
        var searchText = @"SELECT * 
                                FROM FoodDisplayView 
                                WHERE FoodName LIKE '%" + foodName + "%' OR " +
        "FoodGroup LIKE '%' + @FoodGroup + '%'";

        model.Foods = _dbContext.ExecSQL<FoodDisplayView>(searchText, new SqlParameter("@FoodGroup", foodGroup));
        return View(model);
    }

    //SQL Injection vulnerability #2
    [HttpGet]
    public IActionResult UnsafeModel_Concat(string foodName)
    {
        var options = new CookieOptions();
        options.Expires = DateTime.Now.AddYears(1);
        options.HttpOnly = false;
        options.SameSite = SameSiteMode.None;
        options.IsEssential = true;

        HttpContext.Response.Cookies.Append("FoodNameSearch", foodName, options);

        var model = new AccountUserViewModel();
        model.SearchText = foodName;

        using (var connection = new SqlConnection())
        {
            connection.ConnectionString = "Server=localhost\\SQL2019;Initial Catalog=VulnerabilityBuffet;Persist Security Info=False;User ID=ApplicationLogUser;Password=P@ssw0rd*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM FoodDisplayView WHERE FoodName LIKE '%" + foodName + "%'";

            connection.Open();

            var foods = new List<FoodDisplayView>();

            using (var reader = command.ExecuteReader())
            {
                foods.AddRange(reader.LoadFoodsFromReader());
            }

            model.Foods = foods;

            connection.Close();
        }

        return View(model);
    }

    //SQL Injection vulnerability #3
    [HttpGet]
    public IActionResult UnsafeModel_Interpolation(string foodName)
    {
        var model = new AccountUserViewModel();
        model.SearchText = foodName;

        using (var connection = new SqlConnection("Server=localhost\\SQL2019;Initial Catalog=VulnerabilityBuffet;Persist Security Info=False;User ID=ApplicationLogUser;Password=P@ssw0rd*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"))
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM FoodDisplayView WHERE FoodName LIKE '%{foodName}%'";

            connection.Open();

            var foods = new List<FoodDisplayView>();

            using (var reader = command.ExecuteReader())
            {
                foods.AddRange(reader.LoadFoodsFromReader());
            }

            model.Foods = foods;

            connection.Close();
        }

        return View(model);
    }

    [HttpGet]
    [HttpPost]
    //SQL Injection vulnerability #4
    public IActionResult UnsafeModel_Format(string foodName)
    {
        var cn = new SqlConnection(_connectionString);
        var command = cn.CreateCommand();
        command.CommandText = string.Format("SELECT * FROM FoodDisplayView WHERE FoodName LIKE '%{0}%'", foodName);

        cn.Open();

        var foods = new List<FoodDisplayView>();

        using (var reader = command.ExecuteReader())
        {
            foods.AddRange(reader.LoadFoodsFromReader());
        }

        cn.Close();

        return View(foods);
    }

    //SQL Injection vulnerability #5
    public IActionResult UnsafeModel_Concat_Query(string foodName)
    {
        var query = "SELECT * FROM FoodDisplayView WHERE FoodName LIKE '%" + foodName + "%'";

        var model = new AccountUserViewModel();
        model.SearchText = foodName;

        using (var connection = new SqlConnection())
        {
            connection.ConnectionString = _connectionString;

            var command = connection.CreateCommand();
            command.CommandText = query;

            connection.Open();

            var foods = new List<FoodDisplayView>();

            using (var reader = command.ExecuteReader())
            {
                foods.AddRange(reader.LoadFoodsFromReader());
            }

            model.Foods = foods;

            connection.Close();
        }

        return Redirect("/Home/Details/" + foodName);
    }

    //SQL Injection vulnerability #6
    public IActionResult UnsafeModel_Interpolation_Query(string foodName)
    {
        var query = $"SELECT * FROM FoodDisplayView WHERE FoodName LIKE '%{foodName}%'";

        var model = new AccountUserViewModel();
        model.SearchText = foodName;

        using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            connection.Open();

            var foods = new List<FoodDisplayView>();

            using (var reader = command.ExecuteReader())
            {
                foods.AddRange(reader.LoadFoodsFromReader());
            }

            model.Foods = foods;

            connection.Close();
        }

        return View(model);
    }

    //SQL Injection vulnerability #7
    public IActionResult UnsafeModel_Format_Query(string foodName)
    {
        var query = string.Format("SELECT * FROM FoodDisplayView WHERE FoodName LIKE '%{0}%'", foodName);

        var model = new AccountUserViewModel();
        model.SearchText = foodName;

        using (var cn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var command = cn.CreateCommand();
            command.CommandText = query;

            cn.Open();

            var foods = new List<FoodDisplayView>();

            using (var reader = command.ExecuteReader())
            {
                foods.AddRange(reader.LoadFoodsFromReader());
            }

            model.Foods = foods;

            cn.Close();
        }

        return View(model);
    }

    //SQL Injection vulnerability #8
    public IActionResult UnsafeModel_StringBuilder(string foodName)
    {
        var query = new StringBuilder();

        query.AppendLine("SELECT * ");
        query.AppendLine("FROM FoodDisplayView ");
        query.Append("WHERE FoodName LIKE '%");
        query.Append(foodName);
        query.Append("%'");

        var model = new AccountUserViewModel();
        model.SearchText = foodName;

        using (var cn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var command = cn.CreateCommand();
            command.CommandText = query.ToString();

            cn.Open();

            var foods = new List<FoodDisplayView>();

            using (var reader = command.ExecuteReader())
            {
                foods.AddRange(reader.LoadFoodsFromReader());
            }

            model.Foods = foods;

            cn.Close();
        }

        return View(model);
    }

    //SQL Injection vulnerability #9
    public IActionResult UnsafeModel_StringBuilder_Format(string foodName)
    {
        var query = new StringBuilder();

        query.AppendLine("SELECT * ");
        query.AppendLine("FROM FoodDisplayView ");
        query.AppendFormat("WHERE FoodName LIKE '%{0}%'", foodName);

        var model = new AccountUserViewModel();
        model.SearchText = foodName;

        using (var cn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var command = cn.CreateCommand();
            command.CommandText = query.ToString();

            cn.Open();

            var foods = new List<FoodDisplayView>();

            using (var reader = command.ExecuteReader())
            {
                foods.AddRange(reader.LoadFoodsFromReader());
            }

            model.Foods = foods;

            cn.Close();
        }

        return View(model);
    }

    //SQL Injection vulnerability #10
    private List<FoodDisplayView> UnsafeModel_StringBuilder_Interpolation(string foodName)
    {
        var query = new StringBuilder();

        query.AppendLine("SELECT * ");
        query.AppendLine("FROM FoodDisplayView ");
        query.AppendLine($"WHERE FoodName LIKE '%{foodName}%'");

        var foods = new List<FoodDisplayView>();

        using (var cn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var command = cn.CreateCommand();
            command.CommandText = query.ToString();

            cn.Open();

            using (var reader = command.ExecuteReader())
            {
                foods.AddRange(reader.LoadFoodsFromReader());
            }

            cn.Close();
        }

        return foods;
    }

    //SQL Injection via EF vulnerability #1
    public IActionResult UnsafeModel_DbContext_FromSqlRaw(string foodName)
    {
        var model = new AccountUserViewModel();
        model.SearchText = foodName;

        var query = $"SELECT * FROM FoodDisplayView WHERE FoodName LIKE '%{foodName}%'";

        model.Foods = _dbContext.FoodDisplayViews.FromSqlRaw(query).ToList();

        return View(model);
    }

    //SQL Injection via EF vulnerability #2
    public IActionResult UnsafeModel_DbContext_ExecuteSqlRaw(string foodName)
    {
        var model = new AccountUserViewModel();
        model.SearchText = foodName;

        var query = $"SELECT * FROM FoodDisplayView WHERE FoodName LIKE '%{foodName}%'";

        int i = _dbContext.Database.ExecuteSqlRaw(query);

        return View(model);
    }

    //SQL Injection via EF vulnerability #2
    public IActionResult UnsafeModel_DbContext_ExecuteSqlRawAsync(string foodName)
    {
        var model = new AccountUserViewModel();
        model.SearchText = foodName;

        var query = $"SELECT * FROM FoodDisplayView WHERE FoodName LIKE '%{foodName}%'";

        int i = _dbContext.Database.ExecuteSqlRawAsync(query).Result;

        return View(model);
    }

    [HttpGet]
    public IActionResult FalsePositive([FromQuery]string foodName)
    {
        var query = "SELECT * FROM FoodDisplayView WHERE FoodName LIKE '%' + @FoodName + '%'";

        var model = new AccountUserViewModel();
        model.SearchText = foodName;

        var cn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        var command = cn.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@FoodName", foodName);

        cn.Open();

        var foods = new List<FoodDisplayView>();

        using (var reader = command.ExecuteReader())
        {
            foods.AddRange(reader.LoadFoodsFromReader());
        }

        model.Foods = foods;

        return View(model);
    }

    //TODO: Add DbContext checks (FromSql, FromInterpolated, etc.)
}
