namespace ASP_EF_Core_MVC_Test2.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Coins { get; set; }
    
    public List<Order> Orders { get; set; } // Navigation property
}