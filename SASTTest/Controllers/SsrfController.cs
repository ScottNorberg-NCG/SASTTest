using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace SASTTest.Controllers;

public class SsrfController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SsrfController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult HttpClient_Get([FromQuery] string url)
    {
        var content = "";

        using (var client = new HttpClient())
        {
            content = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        }

        return Content(content);
    }

    [HttpGet]
    public IActionResult HttpClient_Post([FromQuery] string url)
    {
        var content = "";
        var dummyContentAsObject = new { message = "Hello", name = "Taylor" };
        var dummyContentAsString = System.Text.Json.JsonSerializer.Serialize(dummyContentAsObject);

        using (var client = _httpClientFactory.CreateClient())
        {
            var httpContent = new StringContent(dummyContentAsString, Encoding.UTF8, "application/json");

            content = client.PostAsync(url, httpContent).Result.Content.ReadAsStringAsync().Result;
        }

        return Content(content);
    }

    [HttpGet]
    public IActionResult HttpClient_Put([FromQuery] string url)
    {
        var content = "";
        var dummyContentAsObject = new { message = "Hello", name = "Taylor" };
        var dummyContentAsString = System.Text.Json.JsonSerializer.Serialize(dummyContentAsObject);

        using (var client = _httpClientFactory.CreateClient())
        {
            var httpContent = new StringContent(dummyContentAsString, Encoding.UTF8, "application/json");

            content = client.PostAsync(new Uri(url), httpContent).Result.Content.ReadAsStringAsync().Result;
        }

        return Content(content);
    }

    [HttpGet]
    public IActionResult HttpClient_Delete([FromQuery] string url)
    {
        var content = "";
        var dummyContentAsObject = new { message = "Hello", name = "Taylor" };
        var dummyContentAsString = System.Text.Json.JsonSerializer.Serialize(dummyContentAsObject);

        using (var client = new HttpClient())
        {
            var httpContent = new StringContent(dummyContentAsString, Encoding.UTF8, "application/json");

            var uri = new Uri(url);

            content = client.DeleteAsync(uri).Result.Content.ReadAsStringAsync().Result;
        }

        return Content(content);
    }

    [HttpGet]
    public IActionResult WebRequest_Get([FromQuery] string url)
    {
        var content = "";

        WebRequest request = WebRequest.Create(url);
        request.Method = "GET";

        using (WebResponse response = request.GetResponseAsync().Result)
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            content = reader.ReadToEnd();
            // Process the response content
        }

        return Content(content);
    }

    [HttpGet]
    public IActionResult WebRequest_Post([FromQuery] string url)
    {
        var content = "";

        var uri = new Uri(url);
        WebRequest request = WebRequest.Create(uri);
        request.Method = "POST";

        using (WebResponse response = request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            content = reader.ReadToEnd();
            // Process the response content
        }

        return Content(content);
    }
}
