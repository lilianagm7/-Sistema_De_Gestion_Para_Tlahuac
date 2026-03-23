using System.Globalization;
using System.Text;

namespace Sistema_Actividades_Tlahuac.Helpers
{
    public class TextHelper
    {
        public static string NormalizarTexto(string texto)
        {
            var normalized = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = Char.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC).ToUpper().Trim();
        }
    }
}
