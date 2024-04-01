using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_EF_Core_MVC_Test2.Models;

public class NewOrderData
{
    public int CustomerId { get; set; }
    public List<SelectListItem> CustomerOptions { get; set; } = [];
}