namespace ASP_EF_Core_MVC_Test2.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    
    public List<OrderItem> OrderItems { get; set; } // Navigation property
}