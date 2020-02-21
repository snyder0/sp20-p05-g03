using System.ComponentModel.DataAnnotations;

namespace SP20.P05.Web.Features.Authentication
{
    public class CreateCustomerDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}