using System.Text.Json;
using ASP_EF_Core_MVC_Test2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_EF_Core_MVC_Test2.Controllers;

public class ShoppingCartController(ApplicationDbContext dbContext) : Controller
{
    public IActionResult Index()
    {
        var cartString = HttpContext.Session.GetString("cart");
        var cart = new List<int>();
        
        if (cartString != null)
        {
            cart = JsonSerializer.Deserialize<List<int>>(cartString);
        }
        
        var products = dbContext.Products!.AsNoTracking().Where(p => cart!.Contains(p.Id)).ToArray();
        
        return View(products);
    }

    public IActionResult AddToCart(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        // Consider making a helper method for this
        var cartString = HttpContext.Session.GetString("cart");
        var cart = new List<int>();
        
        if (cartString != null)
        {
            cart = JsonSerializer.Deserialize<List<int>>(cartString);
        }
        
        cart!.Add((int)id);
        cartString = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString("cart", cartString);
        
        return Json("Added to cart");
    }

    public IActionResult RemoveFromCart(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        // Consider making a helper method for this
        var cartString = HttpContext.Session.GetString("cart");
        var cart = new List<int>();

        if (cartString != null)
        {
            cart = JsonSerializer.Deserialize<List<int>>(cartString);
        }

        cart!.Remove((int)id);
        cartString = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString("cart", cartString);

        return RedirectToAction("Index");
    }
}