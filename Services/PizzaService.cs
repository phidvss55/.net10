using webapi.Contracts;
using webapi.Models;

namespace webapi.Services;

public class PizzaService : IPizzaService
{
    private static readonly List<Pizza> Pizzas = new List<Pizza>
    {
        new Pizza() { Id = 1, Name = "Margherita", IsGlutenFree = true },
        new Pizza() { Id = 2, Name = "Pepperoni", IsGlutenFree = false }
    };
    
    public List<Pizza> GetAll() => Pizzas;
    public Pizza? Get(int id) => Pizzas.FirstOrDefault(p => p.Id == id);
    
    public void Add(Pizza pizza) 
    {
        pizza.Id = Pizzas.Count + 1;
        Pizzas.Add(pizza);
    }

    public void Delete(int id)
    {
        Pizza? pizza = Get(id);
        if (pizza is null)
            return;
        
        Pizzas.Remove(pizza);
    }

    public void Update(Pizza pizza)
    {
        var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
        if (index >= 0)
        {
            Pizzas[index] = pizza;
        }
    }
}