
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IAuthenticationService
    {
        void SetSession(string userName);
        void AuthenticateAdmin();
        void Authenticate();
        UserDto GetConnectedUser();
    }
}
