# ZaldrionPasswordGenerator 🔐

Generador de contraseñas avanzado en **C#**, pensado como proyecto de ciberseguridad (defensivo) para prácticas y portafolio.

Incluye:

- Generación rápida de contraseñas (débil / media / fuerte).
- Generación **personalizada** (longitud, tipos de caracteres, exclusión de caracteres ambiguos).
- Generación de **múltiples contraseñas** en lote.
- Evaluación básica de la **fortaleza** de una contraseña (score 0–100 y nivel).

---

## 🧰 Tecnologías

- Lenguaje: **C#**
- Framework: **.NET 8.0** (puede ajustarse a .NET 6 si es necesario).
- Tipo de proyecto: **Consola**.

---

## 🚀 Cómo ejecutar

1. Clona este repositorio o descarga el `.zip`:

   ```bash
   git clone https://github.com/TU-USUARIO/ZaldrionPasswordGenerator.git
   ```

2. Entra a la carpeta del proyecto:

   ```bash
   cd ZaldrionPasswordGenerator/src/ZaldrionPasswordGenerator
   ```

3. Ejecuta con .NET CLI:

   ```bash
   dotnet run
   ```

O abre `ZaldrionPasswordGenerator.csproj` en **Visual Studio 2022** y ejecuta desde ahí.

---

## 📖 Funcionalidades

### 1. Generación rápida

- Débil: 8 caracteres.
- Media: 12 caracteres.
- Fuerte: 16 caracteres.
- Opción para excluir caracteres ambiguos (O/0, l/1, etc.).

### 2. Generación personalizada

Puedes elegir:

- Longitud (recomendado 12+).
- Incluir:
  - Minúsculas
  - Mayúsculas
  - Dígitos
  - Símbolos
- Excluir caracteres ambiguos.

### 3. Múltiples contraseñas

Genera varias contraseñas con la misma configuración (por ejemplo, 20 contraseñas fuertes para diferentes cuentas).

### 4. Evaluación de fortaleza

Introduce una contraseña y el programa devuelve:

- **Nivel**: MuyDébil, Débil, Media, Fuerte, MuyFuerte.
- **Puntuación** (0–100).
- Comentarios sobre longitud, variedad de caracteres, secuencias sencillas y repeticiones.

---

## 🔐 Enfoque de ciberseguridad

Este proyecto es 100% **defensivo** y educativo:

- Muestra buenas prácticas para generar contraseñas robustas.
- Usa `RandomNumberGenerator` (API criptográfica de .NET) para mayor seguridad.
- Ayuda a comprender qué hace una contraseña más fuerte o más débil.

---

## 📄 Licencia

Este proyecto está bajo licencia **MIT**. Consulta el archivo `LICENSE` para más detalles.
