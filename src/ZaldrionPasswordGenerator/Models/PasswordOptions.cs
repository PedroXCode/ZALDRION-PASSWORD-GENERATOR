namespace ZaldrionPasswordGenerator.Models
{
    /// <summary>
    /// Configuración para la generación de contraseñas.
    /// </summary>
    public class PasswordOptions
    {
        public int Length { get; set; } = 12;

        public bool IncludeLowercase { get; set; } = true;
        public bool IncludeUppercase { get; set; } = true;
        public bool IncludeDigits { get; set; } = true;
        public bool IncludeSymbols { get; set; } = true;

        /// <summary>
        /// Excluir caracteres ambiguos como O/0, l/1, etc.
        /// </summary>
        public bool ExcludeAmbiguous { get; set; } = false;
    }
}
