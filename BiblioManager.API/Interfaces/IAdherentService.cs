namespace BiblioManager.API.Interfaces
{
    public interface IAdherentService
    {
        Task DevenirAdherent(int utilisateurId);
        Task VerifierAdherentActif(int idAdherent);
        Task RenouvelerAbonnement(int idAdherent);
        Task<bool> DesactiverAdhesion(int idAdherent);
    }
}
