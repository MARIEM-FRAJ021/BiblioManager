using BiblioManager.API.Dtos.Emprunt;

namespace BiblioManager.API.Dtos
{
    public class AdherentDto
    {
        public int IdAdherent { get; set; }
        public int IdUtilisateur { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public decimal Penalite { get; set; }

        //Relation avec Emprunt
        public ICollection<EmpruntDto> Emprunts { get; set; }

    }
}
