using Microsoft.AspNetCore.Identity;

namespace Api.Models
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public ApplicationUser User { get; set; }
        public AppRole Role { get; set; }
    }
}