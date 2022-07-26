using System.ComponentModel.DataAnnotations;

namespace Api.Dto
{
  public class LoginUserDto
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
  
  }
}