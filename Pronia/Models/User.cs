using Microsoft.AspNetCore.Identity;

namespace Pronia.Models
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }

    }
}
