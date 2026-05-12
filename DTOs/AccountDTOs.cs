using System.ComponentModel.DataAnnotations;
namespace PracticeWeb.DTOs
{
    public record AccountCredentials(
        [Required][StringLength(12, MinimumLength = 5)] string Name,
        [Required] string Password
    );
}