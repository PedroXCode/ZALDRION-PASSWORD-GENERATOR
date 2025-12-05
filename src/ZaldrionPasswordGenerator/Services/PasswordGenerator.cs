using System.Security.Cryptography;
using System.Text;
using ZaldrionPasswordGenerator.Models;

namespace ZaldrionPasswordGenerator.Services
{
    public static class PasswordGenerator
    {
        // Conjuntos de caracteres base
        private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Digits = "0123456789";
        private const string Symbols = "!@#$%^&*()-_=+[]{};:,.<>/?|";

        // Caracteres ambiguos que podemos eliminar si el usuario lo desea
        private const string AmbiguousChars = "O0oIl1";

        /// <summary>
        /// Genera una contraseña con las opciones indicadas.
        /// </summary>
        public static string GeneratePassword(PasswordOptions options)
        {
            if (options.Length <= 0)
                throw new ArgumentException("La longitud debe ser mayor que cero.", nameof(options));

            var charSets = new List<string>();

            if (options.IncludeLowercase)
                charSets.Add(Lowercase);
            if (options.IncludeUppercase)
                charSets.Add(Uppercase);
            if (options.IncludeDigits)
                charSets.Add(Digits);
            if (options.IncludeSymbols)
                charSets.Add(Symbols);

            if (!charSets.Any())
                throw new InvalidOperationException("Debes seleccionar al menos un tipo de carácter.");

            // Construimos el set total de caracteres
            string allChars = string.Concat(charSets);

            if (options.ExcludeAmbiguous)
            {
                allChars = RemoveCharacters(allChars, AmbiguousChars);
                for (int i = 0; i < charSets.Count; i++)
                {
                    charSets[i] = RemoveCharacters(charSets[i], AmbiguousChars);
                }

                if (allChars.Length == 0)
                    throw new InvalidOperationException("No quedan caracteres disponibles después de excluir los ambiguos.");
            }

            // Aseguramos que haya al menos un carácter de cada categoría seleccionada
            var passwordChars = new char[options.Length];
            int index = 0;

            foreach (string set in charSets)
            {
                if (set.Length == 0) continue; // Por si se vació por excluir ambiguos
                passwordChars[index++] = GetRandomChar(set);
                if (index >= options.Length)
                    break;
            }

            // Rellenamos el resto con el set completo
            while (index < options.Length)
            {
                passwordChars[index++] = GetRandomChar(allChars);
            }

            // Mezclamos los caracteres (Fisher–Yates) usando RNG criptográfico
            Shuffle(passwordChars);

            return new string(passwordChars);
        }

        /// <summary>
        /// Genera varias contraseñas con las mismas opciones.
        /// </summary>
        public static List<string> GenerateMany(PasswordOptions options, int count)
        {
            if (count <= 0)
                throw new ArgumentException("El número de contraseñas debe ser mayor que cero.", nameof(count));

            var list = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(GeneratePassword(options));
            }
            return list;
        }

        private static string RemoveCharacters(string source, string toRemove)
        {
            var sb = new StringBuilder();
            foreach (char c in source)
            {
                if (!toRemove.Contains(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static char GetRandomChar(string characterSet)
        {
            if (string.IsNullOrEmpty(characterSet))
                throw new ArgumentException("El conjunto de caracteres no puede ser vacío.");

            // Usamos RNG criptográfico
            while (true)
            {
                Span<byte> buffer = stackalloc byte[4];
                RandomNumberGenerator.Fill(buffer);
                uint value = BitConverter.ToUInt32(buffer);
                int index = (int)(value % (uint)characterSet.Length);
                return characterSet[index];
            }
        }

        private static void Shuffle(char[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = GetRandomInt(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        private static int GetRandomInt(int minInclusive, int maxExclusive)
        {
            if (minInclusive >= maxExclusive)
                throw new ArgumentException("Rango inválido.");

            // Generamos un int en el rango usando RNG criptográfico
            uint range = (uint)(maxExclusive - minInclusive);
            uint randomValue;

            Span<byte> buffer = stackalloc byte[4];

            do
            {
                RandomNumberGenerator.Fill(buffer);
                randomValue = BitConverter.ToUInt32(buffer);
            } while (randomValue >= uint.MaxValue - (uint.MaxValue % range));

            return (int)(minInclusive + (randomValue % range));
        }
    }
}
