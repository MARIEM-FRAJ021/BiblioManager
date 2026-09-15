using BiblioManager.API.Dtos.Auth;

namespace BiblioManager.API.Interfaces
{
    public interface IAuthsService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task LogoutAsync(string refreshToken);
        Task RegisterAsync(RegisterDto registerDto);
    }
}
