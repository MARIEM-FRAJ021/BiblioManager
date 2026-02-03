using BiblioManager.API.Dtos.Livre;
using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Auteur
{
    public class AuteurDto
    {
            public int IdAuteur { get; set; }
            public string Nom { get; set; }
            public string Prenom { get; set; }
    }
}
