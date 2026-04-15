using System.Collections.Generic; 
namespace ProductRecordSystem.Models; 
public class Customer 
{ 
public int Id { get; set; } 
public string FirstName { get; set; } = string.Empty; 
public string LastName { get; set; } = string.Empty; 
public string Email { get; set; } = string.Empty; 
public string Phone { get; set; } = string.Empty; 
// One Customer has many Orders (1-to-M) 
public ICollection<Order> Orders { get; set; } = new List<Order>(); 
}