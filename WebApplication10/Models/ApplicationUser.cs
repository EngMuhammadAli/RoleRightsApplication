using Microsoft.AspNetCore.Identity;

namespace WebApplication10.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
