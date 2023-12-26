using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SASTTest.EF;
using SASTTest.Models;
using System.Security.Claims;

namespace SASTTest.Controllers;

public class FileController : Controller
{
    private readonly VulnerabilityBuffetContext _dbContext;
    private readonly IWebHostEnvironment _hostEnvironment;

    public FileController(VulnerabilityBuffetContext dbContext, IWebHostEnvironment hostEnvironment)
    {
        _dbContext = dbContext;
        _hostEnvironment = hostEnvironment;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult SafeFileUpload()
    {
        var user = _dbContext.SiteUsers.Single(u => u.UserName == HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
        ViewBag.Files = _dbContext.UserFiles.Where(f => f.UserID == user.UserID).ToList();
        ViewBag.Message = "";

        return View();
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult SafeFileUpload(SafeFileUploadViewModel model)
    {
        var user = _dbContext.SiteUsers.Single(u => u.UserName == HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
        ViewBag.Files = _dbContext.UserFiles.Where(f => f.UserID == user.UserID).ToList();

        var extension = Path.GetExtension(model.FileBytes.FileName);

        byte[] fileBytes;

        using (var stream = model.FileBytes.OpenReadStream())
        {
            fileBytes = new byte[stream.Length];

            for (int i = 0; i < stream.Length; i++)
            {
                fileBytes[i] = (byte)stream.ReadByte();
            }
        }

        if (fileBytes.Length > 2097152)
        {
            ViewBag.Message = $"File too large.";
            return View();
        }

        switch (extension)
        {
            case ".jpg":
            case ".jpeg":
                if (fileBytes[0] != 255 || fileBytes[1] != 216 || fileBytes[2] != 255)
                {
                    ViewBag.Message = $"Image appears not to be in jpg format. Please try another.";
                    return View();
                }
                else if (fileBytes[3] != 219 && fileBytes[3] != 224 && fileBytes[3] != 238 && fileBytes[3] != 225)
                {
                    ViewBag.Message = $"Image appears not to be in jpg format. Please try another.";
                    return View();
                }
                break;
            case ".gif":
                if (fileBytes[0] != 71 || fileBytes[1] != 73 || fileBytes[2] != 70 || fileBytes[3] != 56 || fileBytes[5] != 97)
                {
                    ViewBag.Message = $"Image appears not to be in gif format. Please try another.";
                    return View();
                }
                else if (fileBytes[4] != 55 && fileBytes[4] != 57)
                {
                    ViewBag.Message = $"Image appears not to be in gif format. Please try another.";
                    return View();
                }
                break;
            case ".png":
                if (fileBytes[0] != 137 || fileBytes[1] != 80 || fileBytes[2] != 78 || fileBytes[3] != 71 ||
                    fileBytes[4] != 13 || fileBytes[5] != 10 || fileBytes[6] != 26 || fileBytes[7] != 10)
                {
                    ViewBag.Message = $"Image appears not to be in png format. Please try another.";
                    return View();
                }
                break;
            default:
                ViewBag.Message = $"Extension {extension} is not supported";
                return View();
        }

        var userFile = new UserFile();
        userFile.UserID = user.UserID;
        userFile.FileName = model.FileName;
        userFile.FileExtension = extension;
        userFile.FileBytes = fileBytes;
        userFile.CreatedOn = DateTime.Now;
        _dbContext.UserFiles.Add(userFile);
        _dbContext.SaveChanges();

        ViewBag.Files = _dbContext.UserFiles.Where(f => f.UserID == user.UserID).ToList();
        ViewBag.Message = "File saved successfully";

        return View();
    }

    [HttpGet]
    public IActionResult UnsafeFileUpload()
    {
        ViewBag.Files = GetUploadedFiles();
        ViewBag.Message = "";

        return View();
    }

    [HttpPost]
    public IActionResult UnsafeFileUpload(IFormFile file)
    {
        ViewBag.Files = GetUploadedFiles();

        byte[] fileBytes;

        using (var stream = file.OpenReadStream())
        {
            fileBytes = new byte[stream.Length];

            for (int i = 0; i < stream.Length; i++)
            {
                fileBytes[i] = (byte)stream.ReadByte();
            }
        }

        SaveFileName(file.FileName);

        var rootFolder = _hostEnvironment.ContentRootPath;
        System.IO.File.WriteAllBytes(rootFolder + "\\wwwroot\\UploadedFiles\\" + file.FileName, fileBytes);

        ViewBag.Files = GetUploadedFiles();
        ViewBag.Message = "File saved successfully";

        return View();
    }

    [HttpGet]
    public IActionResult FileInclusion()
    {
        ViewBag.FileContents = "";
        return View();
    }

    [HttpPost]
    public IActionResult FileInclusion(AccountUserViewModel model)
    {
        var fullFilePath = _hostEnvironment.ContentRootPath + "\\wwwroot\\text\\" + model.SearchText;
        var fileContents = System.IO.File.ReadAllText(fullFilePath);
        ViewBag.FileContents = fileContents;

        return View(model);
    }

    private void SaveFileName(string fileName)
    {
        _dbContext.Database.ExecuteSqlRaw($"INSERT UnsafeFile (FileName) VALUES (@FileName)", new SqlParameter("@FileName", fileName));
    }

    private List<string> GetUploadedFiles()
    {
        var list = new List<string>();

        var rootFolder = _hostEnvironment.ContentRootPath;

        var files = Directory.GetFiles(rootFolder + "\\wwwroot\\UploadedFiles\\");

        foreach (var file in files)
        {
            list.Add(Path.GetFileName(file));
        }

        return list;
    }
}
