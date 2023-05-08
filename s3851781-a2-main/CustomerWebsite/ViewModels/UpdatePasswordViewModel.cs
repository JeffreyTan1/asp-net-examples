using System.ComponentModel.DataAnnotations;

namespace CustomerWebsite.Models;

public class UpdatePasswordViewModel
{
    [Required]
    [StringLength(10, MinimumLength = 4)]
    public string Password { get; set; }

}
