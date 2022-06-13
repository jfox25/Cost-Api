using System.Threading.Tasks;
using Api.Models;

namespace Api.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
    }
}