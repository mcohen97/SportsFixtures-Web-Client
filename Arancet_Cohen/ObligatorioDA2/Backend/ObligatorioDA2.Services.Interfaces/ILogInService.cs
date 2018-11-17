using ObligatorioDA2.Services.Interfaces.Dtos;


namespace ObligatorioDA2.Services.Interfaces
{
    public interface ILogInService
    {
        UserDto Login(string username, string password);
    }
}
