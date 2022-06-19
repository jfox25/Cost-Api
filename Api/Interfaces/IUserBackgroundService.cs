using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Interfaces
{
    public interface IUserBackgroundService
    {
        Task UpdateUserStatus();
        void DeleteUserData(ApplicationUser user);
    }
}