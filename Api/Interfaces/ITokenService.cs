using System.Threading.Tasks;
using Api.Dto;
using Api.Models;

namespace Api.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
        RefreshTokenDto GenerateRefreshToken();
    }
}