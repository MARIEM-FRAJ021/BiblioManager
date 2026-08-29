namespace BiblioManager.API.Dtos.Emprunt
{
    public class EmpruntDto
    {
        public int IdEmprunt { get; set; }
        public int IdAdherent { get; set; }
        public string NomAdherent { get; set; } = string.Empty;
        public int IdLivre { get; set; }
        public string TitreLivre { get; set; } = string.Empty;
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetourPrevue { get; set; }
        public DateTime? DateRetourEffective { get; set; }
        public string Statut { get; set; } = string.Empty;
    }

}
