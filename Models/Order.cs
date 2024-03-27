namespace ASP_EF_Core_MVC_Test2.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    
    public Customer Customer { get; set; } // Navigation property
    public int CustomerId { get; set; } // Foreign key
    
    public List<OrderItem> OrderItems { get; set; } // Navigation property
}