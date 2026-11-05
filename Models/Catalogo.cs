using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UnivManagement.Models
{
    public class Catalogo
    {

        public int CatalogoId { get; set; }

        public required string ISBN { get; set; }

        public required string Titulo { get; set; }

        public int AutorId { get; set; }

        [ValidateNever]
        public Autor Autor { get; set; } = null!;

        public ICollection<Exemplar> Exemplares { get; set; }
            = new List<Exemplar>();

    }
}