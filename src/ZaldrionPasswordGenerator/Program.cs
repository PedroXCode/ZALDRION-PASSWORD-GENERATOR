using System.Text;
using ZaldrionPasswordGenerator.Models;
using ZaldrionPasswordGenerator.Services;
using ZaldrionPasswordGenerator.Utils;

namespace ZaldrionPasswordGenerator
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Zaldrion Password Generator";

            ShowHeader();

            bool exit = false;
            while (!exit)
            {
                ShowMenu();
                int option = InputHelper.ReadInt("Selecciona una opción: ", 1, 5);
                Console.WriteLine();

                switch (option)
                {
                    case 1:
                        QuickPassword();
                        break;
                    case 2:
                        CustomPassword();
                        break;
                    case 3:
                        MultiplePasswords();
                        break;
                    case 4:
                        EvaluateExistingPassword();
                        break;
                    case 5:
                        exit = true;
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine();
                    Console.WriteLine("Pulsa cualquier tecla para volver al menú...");
                    Console.ReadKey(true);
                    Console.Clear();
                    ShowHeader();
                }
            }

            Console.WriteLine("Gracias por usar Zaldrion Password Generator. ¡Hasta luego!");
        }

        private static void ShowHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==============================================");
            Console.WriteLine("      ZALDRION PASSWORD GENERATOR (C#)       ");
            Console.WriteLine("==============================================");
            Console.ResetColor();
            Console.WriteLine("Generación y análisis de contraseñas.");
            Console.WriteLine();
        }

        private static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Menú principal");
            Console.ResetColor();
            Console.WriteLine("1) Generar contraseña rápida (débil/media/fuerte)");
            Console.WriteLine("2) Generar contraseña personalizada");
            Console.WriteLine("3) Generar múltiples contraseñas");
            Console.WriteLine("4) Evaluar fortaleza de una contraseña existente");
            Console.WriteLine("5) Salir");
            Console.WriteLine();
        }

        private static void QuickPassword()
        {
            Console.WriteLine("== Generación rápida ==");
            Console.WriteLine("1) Débil   (8 caracteres)");
            Console.WriteLine("2) Media   (12 caracteres)");
            Console.WriteLine("3) Fuerte  (16 caracteres)");
            int choice = InputHelper.ReadInt("Elige nivel: ", 1, 3);

            var options = new PasswordOptions
            {
                IncludeLowercase = true,
                IncludeUppercase = true,
                IncludeDigits = true,
                IncludeSymbols = choice >= 2,
                Length = choice switch
                {
                    1 => 8,
                    2 => 12,
                    3 => 16,
                    _ => 12
                }
            };

            bool excludeAmbiguous = InputHelper.ReadYesNo("¿Quieres excluir caracteres ambiguos (O/0, l/1, etc.)?");
            options.ExcludeAmbiguous = excludeAmbiguous;

            string password = PasswordGenerator.GeneratePassword(options);
            PrintGeneratedPassword(password);
        }

        private static PasswordOptions AskCustomOptions()
        {
            Console.WriteLine("== Configuración personalizada ==");

            int length = InputHelper.ReadInt("Longitud de la contraseña (mínimo 4, recomendado 12+): ", 4, 128);

            bool lower = InputHelper.ReadYesNo("¿Incluir letras minúsculas?");
            bool upper = InputHelper.ReadYesNo("¿Incluir letras mayúsculas?");
            bool digits = InputHelper.ReadYesNo("¿Incluir dígitos (0-9)?");
            bool symbols = InputHelper.ReadYesNo("¿Incluir símbolos (!@#$, etc.)?");
            bool excludeAmbiguous = InputHelper.ReadYesNo("¿Excluir caracteres ambiguos (O/0, l/1, etc.)?");

            if (!lower && !upper && !digits && !symbols)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No seleccionaste ningún tipo de carácter. Se usarán minúsculas por defecto.");
                Console.ResetColor();
                lower = true;
            }

            return new PasswordOptions
            {
                Length = length,
                IncludeLowercase = lower,
                IncludeUppercase = upper,
                IncludeDigits = digits,
                IncludeSymbols = symbols,
                ExcludeAmbiguous = excludeAmbiguous
            };
        }

        private static void CustomPassword()
        {
            var options = AskCustomOptions();
            string password = PasswordGenerator.GeneratePassword(options);
            PrintGeneratedPassword(password);
        }

        private static void MultiplePasswords()
        {
            var options = AskCustomOptions();
            int count = InputHelper.ReadInt("¿Cuántas contraseñas quieres generar? (1–100): ", 1, 100);

            var passwords = PasswordGenerator.GenerateMany(options, count);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Contraseñas generadas:");
            Console.ResetColor();

            int i = 1;
            foreach (var pwd in passwords)
            {
                Console.WriteLine($"{i:00}: {pwd}");
                i++;
            }
        }

        private static void EvaluateExistingPassword()
        {
            Console.WriteLine("== Evaluar fortaleza de contraseña ==");
            string password = InputHelper.ReadNonEmpty("Introduce la contraseña a evaluar (no se guarda en ningún sitio): ");

            var result = PasswordStrengthEvaluator.Evaluate(password);

            Console.WriteLine();
            Console.Write("Nivel: ");
            switch (result.Level)
            {
                case PasswordStrengthLevel.MuyDebil:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case PasswordStrengthLevel.Debil:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case PasswordStrengthLevel.Media:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case PasswordStrengthLevel.Fuerte:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case PasswordStrengthLevel.MuyFuerte:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
            }

            Console.WriteLine($"{result.Level} ({result.Score}/100)");
            Console.ResetColor();

            if (!string.IsNullOrWhiteSpace(result.Description))
            {
                Console.WriteLine();
                Console.WriteLine("Detalles:");
                Console.WriteLine(result.Description);
            }
        }

        private static void PrintGeneratedPassword(string password)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Contraseña generada:");
            Console.ResetColor();
            Console.WriteLine(password);

            var strength = PasswordStrengthEvaluator.Evaluate(password);
            Console.WriteLine();
            Console.WriteLine("Evaluación de la contraseña generada:");

            Console.Write("Nivel: ");
            switch (strength.Level)
            {
                case PasswordStrengthLevel.MuyDebil:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case PasswordStrengthLevel.Debil:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case PasswordStrengthLevel.Media:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case PasswordStrengthLevel.Fuerte:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case PasswordStrengthLevel.MuyFuerte:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
            }

            Console.WriteLine($"{strength.Level} ({strength.Score}/100)");
            Console.ResetColor();
        }
    }
}
