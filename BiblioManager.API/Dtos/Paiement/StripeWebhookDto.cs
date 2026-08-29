using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Paiement
{
    public class StripeWebhookDto
    {
        [Required]
        public string StripeSessionId { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
