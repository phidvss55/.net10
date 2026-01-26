using webapi.Models;

namespace webapi.Contracts;

public interface ITokenService
{
    string CreateToken(AppUser user);
}