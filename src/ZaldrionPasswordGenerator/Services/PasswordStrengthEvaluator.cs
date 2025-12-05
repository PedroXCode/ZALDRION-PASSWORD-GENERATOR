using System.Text;

namespace ZaldrionPasswordGenerator.Services
{
    public enum PasswordStrengthLevel
    {
        MuyDebil,
        Debil,
        Media,
        Fuerte,
        MuyFuerte
    }

    public class PasswordStrengthResult
    {
        public PasswordStrengthLevel Level { get; set; }
        public int Score { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Evalúa la fortaleza de contraseñas de forma simple.
    /// </summary>
    public static class PasswordStrengthEvaluator
    {
        public static PasswordStrengthResult Evaluate(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return new PasswordStrengthResult
                {
                    Level = PasswordStrengthLevel.MuyDebil,
                    Score = 0,
                    Description = "Contraseña vacía."
                };
            }

            int score = 0;
            var sb = new StringBuilder();

            int length = password.Length;

            // 1. Longitud
            if (length < 6)
            {
                score += 5;
                sb.AppendLine("• Muy corta (menos de 6 caracteres).");
            }
            else if (length < 10)
            {
                score += 15;
                sb.AppendLine("• Longitud aceptable (6–9 caracteres).");
            }
            else if (length < 14)
            {
                score += 25;
                sb.AppendLine("• Buena longitud (10–13 caracteres).");
            }
            else
            {
                score += 35;
                sb.AppendLine("• Excelente longitud (14+ caracteres).");
            }

            // 2. Tipos de caracteres
            bool hasLower = password.Any(char.IsLower);
            bool hasUpper = password.Any(char.IsUpper);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(c => !char.IsLetterOrDigit(c));

            int varietyCount = new[] { hasLower, hasUpper, hasDigit, hasSymbol }.Count(x => x);

            if (varietyCount == 1)
            {
                score += 5;
                sb.AppendLine("• Solo usa un tipo de caracteres.");
            }
            else if (varietyCount == 2)
            {
                score += 15;
                sb.AppendLine("• Usa dos tipos de caracteres.");
            }
            else if (varietyCount == 3)
            {
                score += 25;
                sb.AppendLine("• Buena mezcla de caracteres.");
            }
            else if (varietyCount == 4)
            {
                score += 35;
                sb.AppendLine("• Excelente mezcla (mayúsculas, minúsculas, números y símbolos).");
            }

            // 3. Penalizar patrones simples
            if (IsSequential(password))
            {
                score -= 15;
                sb.AppendLine("• Parece contener secuencias sencillas (ej: abc, 123).");
            }

            if (HasRepeatedCharacters(password))
            {
                score -= 10;
                sb.AppendLine("• Contiene muchos caracteres repetidos.");
            }

            if (score < 0) score = 0;
            if (score > 100) score = 100;

            PasswordStrengthLevel level;
            if (score < 20) level = PasswordStrengthLevel.MuyDebil;
            else if (score < 40) level = PasswordStrengthLevel.Debil;
            else if (score < 60) level = PasswordStrengthLevel.Media;
            else if (score < 80) level = PasswordStrengthLevel.Fuerte;
            else level = PasswordStrengthLevel.MuyFuerte;

            return new PasswordStrengthResult
            {
                Level = level,
                Score = score,
                Description = sb.ToString().TrimEnd()
            };
        }

        private static bool IsSequential(string password)
        {
            if (password.Length < 3) return false;

            int sequences = 0;

            for (int i = 0; i < password.Length - 2; i++)
            {
                int a = password[i];
                int b = password[i + 1];
                int c = password[i + 2];

                if ((b == a + 1 && c == b + 1) || (b == a - 1 && c == b - 1))
                {
                    sequences++;
                    if (sequences >= 1) return true;
                }
            }

            return false;
        }

        private static bool HasRepeatedCharacters(string password)
        {
            var groups = password
                .GroupBy(c => c)
                .Select(g => g.Count());

            return groups.Any(count => count > password.Length / 2);
        }
    }
}
