using System.ComponentModel.DataAnnotations;
namespace PracticeWeb.DTOs;
public record AccountCredentials(
    [Required] string Name,
    [Required] string Password
);
public record ChangePasswordCredentials(
    [Required] string password,
    [Required] string NewPassword

);