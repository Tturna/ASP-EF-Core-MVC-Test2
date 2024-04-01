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
        var customer = dbContext.Customers!.FirstOrDefault(c => c.Id == orderData.CustomerId);
        
        if (customer == null)
        {
            ModelState.AddModelError(nameof(orderData.CustomerId), "Invalid customer");
            return View("Create", orderData);
        }
        
        var cartString = HttpContext.Session.GetString("cart");
        var cart = string.IsNullOrEmpty(cartString)
            ? new Dictionary<int, int>()
            : JsonSerializer.Deserialize<Dictionary<int, int>>(cartString);
        
        var productIds = cart!.Keys.ToList();
        
        if (productIds.Count == 0)
        {
            ModelState.AddModelError("", "Cart is empty");
            return View("Create", orderData);
        }

        var individualProducts = dbContext.Products!
            .Where(p => productIds.Contains(p.Id))
            .ToArray();

        var cartProducts = individualProducts.Select(ip => 
            new CartProductData()
            {
                Product = ip,
                Quantity = cart[ip.Id]
            }
        ).ToArray();

        var orderItems = new List<OrderItem>();
        
        var order = new Order()
        {
            OrderDate = DateTime.Now,
            CustomerId = orderData.CustomerId,
            OrderItems = orderItems,
            Customer = customer
        };
        
        var unitPrice = cartProducts.Aggregate(0, (total, next) => total + next.Product.Price) / cartProducts.Length;
        
        foreach (var cartProduct in cartProducts)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                Order = order,
                ProductId = cartProduct.Product.Id,
                Product = cartProduct.Product,
                UnitPrice = unitPrice,
                Quantity = cartProduct.Quantity
            };
            
            orderItems.Add(orderItem);
            cartProduct.Product.OrderItems.Add(orderItem);
        }

        dbContext.Orders!.Add(order);
        dbContext.SaveChanges();

        return View("Checkout", order);
    }
}