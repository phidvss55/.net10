using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers;

[Route("[controller]")] 
public class ProductController : BaseApiController
{
    private readonly ApplicationDBContext _dbContext;
    public ProductController(ApplicationDBContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = _dbContext.Products.ToList();
        if (products.Count == 0)
        {
            return NotFound("No products found.");
        }

        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        try {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(product);
        } catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        try
        {
            _dbContext.Add(product);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        catch (Exception e)
        {
            return BadRequest("Error creating product: " + e.Message);
        }
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(Product product)
    {
        try
        {
            var existingProduct = _dbContext.Products.Find(product.Id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {product.Id} not found.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Qty = product.Qty;

            _dbContext.SaveChanges();
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest("Error updating product: " + e.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        try
        {
           var product = _dbContext.Products.Find(id);
           if (product == null)
           {
               throw new Exception($"Product with ID {id} not found.");
           }
           _dbContext.Products.Remove(product);
          _dbContext.SaveChanges();
          return Ok("Product deleted with ID: " + id);
        }
        catch (Exception e)
        {
            return BadRequest("Error deleting product: " + e.Message);
        }
    }
}