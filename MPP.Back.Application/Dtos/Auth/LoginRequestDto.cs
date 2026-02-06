using System.ComponentModel.DataAnnotations;

namespace MPP.Back.Application.Dtos.Auth
{
    public class LoginRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Password { get; set; } = string.Empty;
    }
}
