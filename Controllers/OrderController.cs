using System.Text.Json;
using ASP_EF_Core_MVC_Test2.Data;
using ASP_EF_Core_MVC_Test2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP_EF_Core_MVC_Test2.Controllers;

public class OrderController(ApplicationDbContext dbContext) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        var newOrderData = new NewOrderData
        {
            CustomerOptions = dbContext.Customers!.Select(c =>
                new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }
            ).ToList()
        };

        return View(newOrderData);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(NewOrderData orderData)
    {
        var cartString = HttpContext.Session.GetString("cart");
        var cart = new List<int>();
        
        if (cartString != null)
        {
            cart = JsonSerializer.Deserialize<List<int>>(cartString);
        }

        var products = dbContext.Products!
            .AsNoTracking()
            .Where(p => cart.Contains(p.Id))
            .ToArray();

        var orderItems = new List<OrderItem>();
        var unitPrice = products.Aggregate(0, (total, next) => total + next.Price) / products.Length;

        foreach (var product in products)
        {
            var existing = orderItems.FirstOrDefault(oi => oi.ProductId == product.Id);

            if (existing != null) 
            {
                existing.Quantity++;
                continue;
            }
            
            orderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                Product = product,
                UnitPrice = unitPrice,
                Quantity = 1
            });
        }

        var customer = dbContext.Customers!
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == orderData.CustomerId);

        var order = new Order()
        {
            OrderDate = DateTime.Now,
            CustomerId = orderData.CustomerId,
            OrderItems = orderItems,
            Customer = customer
        };
    }
    
    public IActionResult Checkout()
    {
        return View();
    }
}