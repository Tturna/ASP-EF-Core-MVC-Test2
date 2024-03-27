using System.ComponentModel.DataAnnotations;

namespace ASP_EF_Core_MVC_Test2.Models;

public class NewCustomerData
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}