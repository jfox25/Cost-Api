using Api.Models;

namespace Api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}