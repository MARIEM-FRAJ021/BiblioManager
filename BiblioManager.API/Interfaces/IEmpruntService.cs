namespace BiblioManager.API.Interfaces
{
    public interface IEmpruntService
    {
        Task EmprunterLivre(int idAdherent, int idLivre);
        Task RetournerLivre(int idEmprunt);
    }
}
