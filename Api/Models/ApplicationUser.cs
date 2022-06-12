using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}