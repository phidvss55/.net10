using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapi.Models.Views;

namespace webapi.Controllers.Views;

[Route("product")]
public class ProductController: Controller
{
    private readonly Uri baseUri = new Uri("http://localhost:8080/api/");
    private readonly HttpClient _client;

    public ProductController()
    {
        _client = new HttpClient();
        _client.BaseAddress = baseUri;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        List<ProductViewModel> products = new List<ProductViewModel>();
        HttpResponseMessage response = _client.GetAsync("product").Result;
        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            products = JsonConvert.DeserializeObject<List<ProductViewModel>>(data);
        }
        return View("~/Components/Pages/Products/Index.cshtml", products);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public IActionResult Create(ProductViewModel product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        try
        {
            string data = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync("product", content).Result;
            if (response.IsSuccessStatusCode)        
            {
                TempData["Message"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return View();
        }
        return View();
    }

    [HttpGet("edit")]
    public IActionResult Edit(int id)
    {
        ProductViewModel product = new ProductViewModel();
        HttpResponseMessage response = _client.GetAsync($"product/{id}").Result;
        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            product = JsonConvert.DeserializeObject<ProductViewModel>(data);
        }
        return View(product);
    }

    [HttpPost("edit")]
    public IActionResult Edit(ProductViewModel product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        try {
            string data = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync($"product/{product.Id}", content).Result;
            
            if (response.IsSuccessStatusCode)        
            {
                TempData["Message"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(product);
        } catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return View();
        }
    }

    [HttpPost("delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        try
        {
            HttpResponseMessage response = _client.DeleteAsync($"product/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Product deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to delete product";
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToAction("Index");
    }
}