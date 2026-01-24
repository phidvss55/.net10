using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;

namespace webapi.Services;

public class PizzaService
{
    static List<Pizza> Pizzas { get; }

    static PizzaService()
    {
        Pizzas = new List<Pizza>
        {
            new Pizza() { Id = 1, Name = "Margherita", IsGlutenFree = true },
            new Pizza() { Id = 2, Name = "Pepperoni", IsGlutenFree = false }
        };
    }
    
    public static List<Pizza> GetAll() => Pizzas;
    public static Pizza? Get(int id) => Pizzas.FirstOrDefault(p => p.Id == id);
    
    public static void Add(Pizza pizza) 
    {
        pizza.Id = Pizzas.Count + 1;
        Pizzas.Add(pizza);
    }

    public static void Delete(int id)
    {
        Pizza pizza = Get(id);
        if (pizza is null)
            return;
        
        Pizzas.Remove(pizza);
    }

    public static void Update(Pizza pizza)
    {
        var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
        if (index >= 0)
        {
            Pizzas[index] = pizza;
        }
    }
}