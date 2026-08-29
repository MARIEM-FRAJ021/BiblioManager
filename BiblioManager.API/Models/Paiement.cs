using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BiblioManager.API.Models
{
    public class Paiement
    {
        [Key]
        public int IdPaiement { get; set; }
        [ForeignKey("Utilisateur")]
        public int IdUtilisateur { get; set; }
        [JsonIgnore]
        public Utilisateur? Utilisateur { get; set; }
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; } = DateTime.UtcNow;
        public ModePaiementEnum Mode { get; set; } = ModePaiementEnum.Carte;
        // Pour paiement carte Stripe
        public string StripeSessionId { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public PaiementStatutEnum Statut { get; set; }

        public TypePaiement Type { get; set; } = TypePaiement.Abonnement;
    }
    public enum ModePaiementEnum
    {
        Especes,
        Carte,
        Virement
    }
    public enum PaiementStatutEnum
    {
        EnAttente,
        Valide,
        Refuse,
        Consomme
    }

    public enum TypePaiement
    {
        Abonnement,
        Penalite
    }
}
