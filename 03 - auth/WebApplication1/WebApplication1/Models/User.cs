using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication1.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string? Username { get; set; }
    [JsonIgnore]
    public string? PasswordHash { get; set; }
}