using System.ComponentModel.DataAnnotations;

namespace Zoo.Models
{
    public class LoginUser
    {
        [Required]
        [Length(6, 30)]
        public string? Username { get; set; }

        [Required]
        [Length(6, 30)]
        public string? Password { get; set; }
    }
}
