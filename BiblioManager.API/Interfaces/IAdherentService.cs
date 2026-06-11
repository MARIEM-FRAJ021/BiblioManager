namespace BiblioManager.API.Interfaces
{
    public interface IAdherentService
    {
        Task DevenirAdherent(int utilisateurId);
        Task<bool> AdherentEstActif(int idAdherent);
        Task RenouvelerAbonnement(int idAdherent);
        Task<bool> DesactiverAdhesion(int idAdherent);
    }
}
