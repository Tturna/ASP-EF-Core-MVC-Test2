using System.Diagnostics;
using ASP_EF_Core_MVC_Test2.Data;
using ASP_EF_Core_MVC_Test2.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_EF_Core_MVC_Test2.Controllers;

// Primary constructor syntax
public class CustomerController(ApplicationDbContext dbContext) : Controller
{
    private readonly ApplicationDbContext? _dbContext = dbContext;
    
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    // bind data to the NewCustomerData model instead of the Customer model to prevent overposting
    public IActionResult Create(NewCustomerData customerData)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", customerData);
        }
        
        var newCustomer = new Customer
        {
            Name = customerData.Name,
            Email = customerData.Email,
            Coins = 100
        };

        try
        {
            _dbContext!.Customers.Add(newCustomer);
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            ModelState.AddModelError(string.Empty, "An error occurred while saving the customer.");
            return View("Index", customerData);
        }
        
        ViewData["Message"] = "Customer created!";
        return View("Index");
    }
}