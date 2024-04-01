using System.Text.Json;
using ASP_EF_Core_MVC_Test2.Data;
using ASP_EF_Core_MVC_Test2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_EF_Core_MVC_Test2.Controllers;

public class ShoppingCartController(ApplicationDbContext dbContext) : Controller
{
    public IActionResult Index()
    {
        var cartString = HttpContext.Session.GetString("cart");
        var cart = string.IsNullOrEmpty(cartString)
            ? new Dictionary<int, int>()
            : JsonSerializer.Deserialize<Dictionary<int, int>>(cartString);
        
        var productIds = cart!.Keys.ToList();
        var individualProducts = dbContext.Products!.Where(p => productIds.Contains(p.Id)).ToList();

        var products = individualProducts.Select(ip =>
            new CartProductData()
            {
                Product = ip,
                Quantity = cart[ip.Id]
            }
        ).ToList();
        
        return View(products);
    }

    [HttpPost]
    public IActionResult AddToCart([FromBody] NewCartProductData? cartProductData)
    {
        if (cartProductData == null || cartProductData.ProductId == default || string.IsNullOrEmpty(cartProductData.ProductName))
        {
            return BadRequest();
        }
        
        // Consider making a helper method for this
        var cartString = HttpContext.Session.GetString("cart");
        var cart = string.IsNullOrEmpty(cartString)
            ? new Dictionary<int, int>()
            : JsonSerializer.Deserialize<Dictionary<int, int>>(cartString);

        cart!.TryGetValue(cartProductData.ProductId, out var value);
        cart[cartProductData.ProductId] = ++value;
        
        cartString = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString("cart", cartString);
        
        return Json($"Added {cartProductData.ProductName} to cart");
    }

    public IActionResult RemoveFromCart(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        // Consider making a helper method for this
        var cartString = HttpContext.Session.GetString("cart");
        var cart = string.IsNullOrEmpty(cartString)
            ? new Dictionary<int, int>()
            : JsonSerializer.Deserialize<Dictionary<int, int>>(cartString);

        cart!.TryGetValue((int)id, out var value);
        
        if (value > 1)
        {
            cart[(int)id] = --value;
        }
        else
        {
            cart.Remove((int)id);
        }

        cartString = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString("cart", cartString);

        return RedirectToAction("Index");
    }
}