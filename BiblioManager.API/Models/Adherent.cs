using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BiblioManager.API.Models
{
    public class Adherent
    {
        [Key]
        public int IdAdherent { get; set; }
        public string Prenom {  get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public bool Actif { get; set; } 
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public decimal Penalite { get; set; }

        //Relation avec Emprunt
        public ICollection<Emprunt> Emprunts { get; set; }

        [ForeignKey("Utilisateur")]
        public int IdUtilisateur { get; set; }
        [JsonIgnore]
        public Utilisateur Utilisateur { get; set; }

    }
    public enum StatutAdherentEnum
    {
        NonAdherent,
        Expire,
        Desactive,
        Actif
    }
}
