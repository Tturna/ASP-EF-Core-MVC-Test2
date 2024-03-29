using System.ComponentModel.DataAnnotations;

namespace ASP_EF_Core_MVC_Test2.Models;

public class NewProductData
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    [Required]
    public int Price { get; set; }
    [Url]
    public string? ImageUrl { get; set; }
    [MaxLength(100)]
    public string? ImageAltText { get; set; }
}