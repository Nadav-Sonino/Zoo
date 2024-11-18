using Zoo.Models;

namespace Zoo.Services
{
    public interface ITokenService
    {
        string CreateToken(Admin admin);
    }
}
