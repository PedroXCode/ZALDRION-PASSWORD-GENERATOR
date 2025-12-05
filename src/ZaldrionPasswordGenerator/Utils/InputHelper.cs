namespace ZaldrionPasswordGenerator.Utils
{
    /// <summary>
    /// Funciones auxiliares para leer y validar entradas de consola.
    /// </summary>
    public static class InputHelper
    {
        public static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Por favor, introduce un número entre {min} y {max}.");
                Console.ResetColor();
            }
        }

        public static bool ReadYesNo(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (s/n): ");
                string? input = Console.ReadLine()?.Trim().ToLowerInvariant();

                if (input == "s" || input == "si" || input == "sí")
                    return true;
                if (input == "n" || input == "no")
                    return false;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Respuesta inválida. Escribe 's' o 'n'.");
                Console.ResetColor();
            }
        }

        public static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("El valor no puede estar vacío.");
                Console.ResetColor();
            }
        }
    }
}
