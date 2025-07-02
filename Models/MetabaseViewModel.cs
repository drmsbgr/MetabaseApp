using System.ComponentModel.DataAnnotations;

namespace MetabaseApp.Models;

public class MetabaseViewModel
{
    [Required, MaxLength(20)]
    public string? UserName { get; set; }
    public bool IHaveToken { get; set; }
    public string? Token { get; set; }
}
