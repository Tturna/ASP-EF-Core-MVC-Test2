using System.Diagnostics;
using ASP_EF_Core_MVC_Test2.Data;
using ASP_EF_Core_MVC_Test2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_EF_Core_MVC_Test2.Controllers;

public class ProductController(ApplicationDbContext dbContext) : Controller
{
    public IActionResult Index()
    {
        var products = dbContext.Products!.AsNoTracking().ToArray();
        
        return View(products);
    }
    
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(NewProductData productData)
    {
        if (!ModelState.IsValid)
        {
            return View(productData);
        }

        var newProduct = new Product
        {
            Name = productData.Name,
            Description = productData.Description,
            Price = productData.Price,
            ImageUrl = productData.ImageUrl,
            ImageAltText = productData.ImageAltText
        };

        try
        {
            dbContext.Add(newProduct);
            dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            ModelState.AddModelError(string.Empty, "An error occurred while saving the product.");
            return View(productData);
        }

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var product = dbContext.Products!
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product newProduct)
    {
        if (!ModelState.IsValid)
        {
            return View(newProduct);
        }

        dbContext.Products!.Update(newProduct);
        
        // var productToUpdate = await dbContext.Products!.FirstOrDefaultAsync(p => p.Id == newProduct.Id);
        //
        // if (productToUpdate == null)
        // {
        //     return NotFound();
        // }
        //
        // var canUpdate = await TryUpdateModelAsync(
        //     productToUpdate,
        //     string.Empty,
        //     p => p.Name,
        //     p => p.Description,
        //     p => p.Price,
        //     p => p.ImageUrl,
        //     p => p.ImageAltText
        // );
        //
        // if (!canUpdate) return View(newProduct);
        
        try
        {
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            ModelState.AddModelError(string.Empty, "An error occurred while editing the product.");
        }

        return View(newProduct);
    }

    public IActionResult DeleteConfirmation(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        return PartialView("_DeleteConfirmation", (int)id);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Product product)
    {
        dbContext.Products!.Remove(product);

        try
        {
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return Problem("An error occurred while deleting the product.", statusCode: 500);
        }
    }
}