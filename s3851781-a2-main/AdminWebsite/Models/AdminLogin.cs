using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminWebsite.Models;

public partial class AdminLogin
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
