namespace BiblioManager.API.Interfaces
{
    public interface IPasswordResetService
    {
        Task RequestPasswordResetAsync(string email);
        Task ResetPasswordAsync(string token, string newPassword);
    }
}
