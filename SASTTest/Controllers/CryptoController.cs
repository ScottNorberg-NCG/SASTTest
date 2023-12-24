using Microsoft.AspNetCore.Mvc;
using SASTTest.EF;
using System.Security.Cryptography;
using System.Text;

namespace SASTTest.Controllers;

public class CryptoController : Controller
{
    private readonly int _keySize = 1024;
    private readonly byte[] ENCRYPTION_KEY = new byte[] { 3, 13, 23, 33, 43, 53, 63, 73 };
    private readonly byte[] ENCRYPTION_IV = new byte[] { 5, 17, 29, 41, 53, 65, 77, 89 };

    private readonly VulnerabilityBuffetContext _dbContext;

    public CryptoController(VulnerabilityBuffetContext dbContext)
    { 
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Rsa()
    {
        using (var rsa1 = new RSACryptoServiceProvider(1024))
        { }

        using (var rsa2 = new RSACryptoServiceProvider(_keySize))
        { }

        using (var rsa3 = new RSACryptoServiceProvider())
        {
            rsa3.KeySize = 512;
        }

        var rsa4 = new RSACryptoServiceProvider();
        rsa4.KeySize = _keySize;

        var rsa5 = new RSACryptoServiceProvider() { KeySize = 128 };

        return View();
    }

    public IActionResult Hash()
    {
        var toHash = "ThisIsMySecretPassword";
        var toHashBytes = Encoding.UTF8.GetBytes(toHash);

        using (var sha1 = new SHA1CryptoServiceProvider())
        {
            byte[] sha1Bytes = sha1.ComputeHash(toHashBytes);
        }

        using (var sha1 = SHA1.Create())
        {
            byte[] sha1Bytes = sha1.ComputeHash(toHashBytes);
        }

        using (var md5 = new MD5CryptoServiceProvider())
        {
            byte[] sha1Bytes = md5.ComputeHash(toHashBytes);
        }

        using (var md5 = MD5.Create())
        {
            byte[] sha1Bytes = md5.ComputeHash(toHashBytes);
        }

        return View();
    }

    public IActionResult Encrypt(string foodName, string algorithm)
    {
        if (algorithm == "DES")
            return Json(new { encrypted = EncryptDES(foodName) });
        else if (algorithm == "RC2")
            return Json(new { encrypted = EncryptRC2(foodName) });
        else
            return Json(new { encrypted = "Algorithm Not Found" });
    }

    public IActionResult Decrypt(string encrypted, string algorithm)
    {
        if (algorithm == "DES")
            return Json(new { decrypted = DecryptDES(encrypted) });
        else if (algorithm == "RC2")
            return Json(new { decrypted = DecryptRC2(encrypted) });
        else
            return Json(new { decrypted = "Algorithm Not Found" });
    }

    private string EncryptDES(string toEncrypt)
    {
        byte[] plainTextBytes = UTF8Encoding.UTF8.GetBytes(toEncrypt);

        //DES is not safe - use AES instead
        var desService = new DESCryptoServiceProvider();

        desService.Key = ENCRYPTION_KEY;
        desService.IV = ENCRYPTION_IV;
        desService.Mode = CipherMode.ECB;
        desService.Padding = PaddingMode.PKCS7;

        using (var transform = desService.CreateEncryptor())
        {
            byte[] encryptedBytes = transform.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
            return Convert.ToBase64String(encryptedBytes, 0, encryptedBytes.Length);
        }
    }

    private string DecryptDES(string toDecrypt)
    {
        byte[] encryptedBytes = Convert.FromBase64String(toDecrypt);

        //DES is not safe - use AES instead
        var desService = new DESCryptoServiceProvider();

        //Same as above, seeing if we can make this easier for SAST scanners to find
        desService.Key = new byte[] { 3, 13, 23, 33, 43, 53, 63, 73 };
        desService.IV = new byte[] { 5, 17, 29, 41, 53, 65, 77, 89 };

        desService.Mode = CipherMode.ECB;
        desService.Padding = PaddingMode.PKCS7;

        using (var transform = desService.CreateDecryptor())
        {
            byte[] decryptedBytes = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            return UTF8Encoding.UTF8.GetString(decryptedBytes);
        }
    }

    private string EncryptRC2(string toEncrypt)
    {
        byte[] plainTextBytes = UTF8Encoding.UTF8.GetBytes(toEncrypt);

        //RC2 needs a long key to be secure, so let's use a short one here
        var rc2Service = new RC2CryptoServiceProvider();

        rc2Service.Key = new byte[] { 10, 30, 50, 70, 90 };
        rc2Service.IV = new byte[] { 8, 20, 16, 40, 24, 60, 32, 80 };
        rc2Service.Mode = CipherMode.ECB;
        rc2Service.Padding = PaddingMode.PKCS7;

        using (var transform = rc2Service.CreateEncryptor())
        {
            byte[] encryptedBytes = transform.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
            return Convert.ToBase64String(encryptedBytes, 0, encryptedBytes.Length);
        }
    }

    private string DecryptRC2(string toDecrypt)
    {
        byte[] encryptedBytes = Convert.FromBase64String(toDecrypt);

        //DES is not safe - use AES instead
        var rc2service = new RC2CryptoServiceProvider();

        rc2service.Key = new byte[] { 10, 30, 50, 70, 90 };
        rc2service.IV = new byte[] { 8, 20, 16, 40, 24, 60, 32, 80 };
        rc2service.Mode = CipherMode.ECB;
        rc2service.Padding = PaddingMode.PKCS7;

        using (var transform = rc2service.CreateDecryptor())
        {
            byte[] decryptedBytes = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            return UTF8Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
