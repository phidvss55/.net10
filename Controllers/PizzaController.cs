using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController: ControllerBase
{
    [HttpGet(Name = "GetAllPizzas")]
    public IEnumerable<Models.Pizza> GetAll()
    {
        return Services.PizzaService.GetAll();
    }

    [HttpGet("{id}")]
    public Models.Pizza? Get(int id)
    {
        return Services.PizzaService.Get(id);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Models.Pizza pizza)
    {
        Services.PizzaService.Add(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Pizza pizza)
    {
        if (id != pizza.Id)
            return BadRequest();
           
        var existingPizza = PizzaService.Get(id);
        if(existingPizza is null)
            return NotFound();
   
        PizzaService.Update(pizza);           
   
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = PizzaService.Get(id);
   
        if (pizza is null)
            return NotFound();
       
        PizzaService.Delete(id);
   
        return NoContent();
    }
}