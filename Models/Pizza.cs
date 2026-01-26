using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

[Table("pizzas")]
public class Pizza
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsGlutenFree { get; set; }
}