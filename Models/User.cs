using Microsoft.AspNetCore.Identity;

namespace NetProductivity.Models
{
    public class User : IdentityUser
    {
        public string Role { get; set; }
    }
}
