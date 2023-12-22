using Microsoft.AspNetCore.Mvc;
using SASTTest.Models;
using System.Diagnostics;
using System.Security.Cryptography;

namespace SASTTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly int _keySize = 1024;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        { 
            return View();
        }

        public IActionResult Crypto()
        {
            using (var rsa1 = new RSACryptoServiceProvider(1024))
            { }

            using (var rsa2 = new RSACryptoServiceProvider(_keySize))
            { }

            //TODO in SAST
            using (var rsa3 = new RSACryptoServiceProvider())
            {
                rsa3.KeySize = 512;
            }

            //TODO in SAST
            var rsa4 = new RSACryptoServiceProvider();
            rsa4.KeySize = _keySize;

            var rsa5 = new RSACryptoServiceProvider() { KeySize = 128 };

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
