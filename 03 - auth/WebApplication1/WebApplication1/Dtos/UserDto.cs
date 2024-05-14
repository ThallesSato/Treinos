using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public class UserDto
{
    [MaxLength(11,ErrorMessage = "The field Username must be a string type with a length of '11'.")]
    [MinLength(11,ErrorMessage = "The field Username must be a string type with a length of '11'.")]
    public required string Username { get; set; }
    public required string Password { get; set; }
}