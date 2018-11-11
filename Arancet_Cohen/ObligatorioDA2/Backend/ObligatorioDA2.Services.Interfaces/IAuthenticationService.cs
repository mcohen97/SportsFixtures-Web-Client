
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IAuthenticationService
    {
        UserDto Login(string username, string password);
        void SetSession(string userName);
        void AuthenticateAdmin();
        void Authenticate();
    }
}
