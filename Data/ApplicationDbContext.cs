﻿using ASP_EF_Core_MVC_Test2.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_EF_Core_MVC_Test2.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}