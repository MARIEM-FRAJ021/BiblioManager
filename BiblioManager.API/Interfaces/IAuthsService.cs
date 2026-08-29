using BiblioManager.API.Dtos.Auth;

namespace BiblioManager.API.Interfaces
{
    public interface IAuthsService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
