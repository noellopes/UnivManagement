using System.ComponentModel.DataAnnotations;
namespace UnivManagement.Models
{
    public class Obra : IValidatableObject
    {
        
        public int ObraId { get; set; }

        public string? ISBN_ISSN { get; set; }

        [Required(ErrorMessage = "Introduz o título da obra.")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "Seleciona o tipo da obra.")]
        public int? TipoObraId { get; set; }

        public TipoObra? Tipo { get; set; }

        [Required(ErrorMessage = "Introduz o valor da multa.")]
        [Range(0, double.MaxValue,
    ErrorMessage = "Introduz um valor de multa válido, igual ou superior a 0 €.")]
        public required double ValorMulta { get; set; }

        public ICollection<Exemplar> Exemplares { get; set; }
            = new List<Exemplar>();

        public ICollection<ObraAutor> ObrasAutores { get; set; }
        = new List<ObraAutor>();

        public IEnumerable<ValidationResult> Validate(
    ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public static bool ISBNValido(string valor)
        {
            string isbn = valor.Trim();

            if (isbn.StartsWith("ISBN ", StringComparison.OrdinalIgnoreCase))
                isbn = isbn[5..];

            isbn = isbn.Replace("-", "").Replace(" ", "");

            if (isbn.Length != 13 ||
    !isbn.All(c => c >= '0' && c <= '9'))
            {
                return false;
            }

            if (!isbn.StartsWith("978") && !isbn.StartsWith("979"))
            {
                return false;
            }   

            int soma = 0;

            for (int i = 0; i < 13; i++)
            {
                int digito = isbn[i] - '0';
                soma += digito * (i % 2 == 0 ? 1 : 3);
            }

            return soma % 10 == 0;
        }

        public static bool ISSNValido(string valor)
        {
            string issn = valor.Trim();

            if (issn.StartsWith("ISSN ", StringComparison.OrdinalIgnoreCase))
                issn = issn[5..];

            issn = issn.Replace("-", "").Replace(" ", "");

            if (issn.Length != 8)
                return false;

            if (!issn[..7].All(char.IsDigit))
                return false;

            if (!char.IsDigit(issn[7]) && issn[7] != 'X')
                return false;

            int soma = 0;

            for (int i = 0; i < 8; i++)
            {
                int digito = issn[i] == 'X' ? 10 : issn[i] - '0';
                soma += digito * (8 - i);
            }

            return soma % 11 == 0;
        }

        public static string? NormalizarIdentificador(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return null;

            string identificador = valor.Trim().ToUpperInvariant();

            if (identificador.StartsWith("ISBN ") ||
                identificador.StartsWith("ISSN "))
            {
                identificador = identificador[5..];
            }

            return identificador.Replace("-", "").Replace(" ", "");
        }
    }
}
