using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
        public DateTime LastActive { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeadUser { get; set; }
    }
}