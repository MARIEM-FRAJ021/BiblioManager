using BiblioManager.API.Dtos.Emprunt;
using BiblioManager.API.Models;

namespace BiblioManager.API.Dtos
{
    public class AdherentDto
    {
        public int IdAdherent { get; set; }
        public int IdUtilisateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public decimal Penalite { get; set; }

        //Relation avec Emprunt
        public ICollection<EmpruntDto> Emprunts { get; set; }
        /// <summary>
        /// Relation avec Paiements
        /// </summary>

    }
}
