using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Contracts;
using webapi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("/pizzas")]
[Authorize]
public class PizzaController : BaseApiController
{
    private readonly IPizzaService _pizzaService;

    public PizzaController(IPizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }

    [HttpGet(Name = "GetAllPizzas")]
    [AllowAnonymous]
    public IActionResult GetAll()
    {
        return Ok(_pizzaService.GetAll());
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var pizza = _pizzaService.Get(id);
        if (pizza == null) return NotFound();
        return Ok(pizza);
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Post([FromBody] Pizza pizza)
    {
        _pizzaService.Add(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, Pizza pizza)
    {
        if (id != pizza.Id)
            return BadRequest();
           
        var existingPizza = _pizzaService.Get(id);
        if(existingPizza is null)
            return NotFound();
   
        _pizzaService.Update(pizza);           
   
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = _pizzaService.Get(id);
   
        if (pizza is null)
            return NotFound();
       
        _pizzaService.Delete(id);
   
        return NoContent();
    }
}