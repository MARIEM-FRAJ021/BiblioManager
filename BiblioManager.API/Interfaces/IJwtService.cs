using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Utilisateur user);
    }
}
