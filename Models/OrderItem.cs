namespace ASP_EF_Core_MVC_Test2.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }
    
    public Product Product { get; set; } // Navigation property
    public int ProductId { get; set; } // Foreign key
    
    public Order Order { get; set; } // Navigation property
    public int OrderId { get; set; } // Foreign key
}