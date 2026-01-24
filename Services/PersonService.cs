using webapi.Contracts;
using webapi.Models;

namespace webapi.Services;

public class PersonService: IPersonService
{
    DateTime _serviceCreated;
    Guid _serviceId;
    
    public string GetPersonName()   
    {
        return "John Doe";
    }
    
    public PersonService()
    {
        _serviceCreated = DateTime.Now;
        _serviceId = Guid.NewGuid();                
    }

    public string GetWelcomeMessage()
    {
        return $"Welcome to Contoso! The current time is {_serviceCreated}. This service instance has an ID of {_serviceId}";
    }

    // public static List<Person> GetAllPeople()
    // {
    //     // var people = await DBNull.Per
    //     var pizzas = await db.Pizzas.ToListAsync();
    // }
    //
    // public static InsertPeople(Person person)
    // {
    //     await db.pizzas.AddAsync(
    //         new Pizza { ID = 1, Name = "Pepperoni", Description = "The classic pepperoni pizza" }
    //     );
    //     
    //     db.People.Add(person);
    //     await db.SaveChangesAsync();
    // }
}