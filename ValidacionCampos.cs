using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClienteGloomApp
{
    public class ValidacionCampos
    {
        public string VerificarCorreo(string correo)
        {
            string correoRegex = @"^[a-zA-Z0-9._][a-zA-Z0-9._-]{3,62}@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]{2,13})+$";

            if (string.IsNullOrWhiteSpace(correo))
            {
                throw new ArgumentException("11");
            }

            if (!Regex.IsMatch(correo, correoRegex))
            {
                throw new ArgumentException("11");
            }

            return correo;
        }

        public string VerificarInconoSeleccionado(string icono)
        {
            if (icono.Equals("sin incono"))
            {
                throw new ArgumentException("12");
            }
            return icono;
        }

        public string VerificarNombreYApellidos(string nombre)
        {
            string nombreRegex = @"^(?! )(?!.*[\\!\\#\\$%\\&'\\(\\)\\*\\+\\-\\.,\\/\\:\\;<\\=\\>\\?\\@\\[\\\\\\]\\^_`\\{\\|\\}\\~])(?!.* {2})(?!.*\d)[\p{L} ]{4,255}(?<! )$";

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("35");
            }

            if (!Regex.IsMatch(nombre, nombreRegex))
            {
                throw new ArgumentException("35");
            }

            return nombre;
        }

        public string VerificarNombreUsuario(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("36");
            }

            ValidarFormatoNombre(nombre);
            ValidarNombreProhibido(nombre);

            return nombre;
        }

        private void ValidarFormatoNombre(string nombre)
        {
            string nombreRegex = @"^(?! )(?!.* {2})[a-zA-Z0-9_ ]+(?<! )$";

            if (!Regex.IsMatch(nombre, nombreRegex))
            {
                throw new ArgumentException("36");
            }
        }

        private void ValidarNombreProhibido(string nombre)
        {
            if (nombre.Equals("Sin ganador", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("El nombre no puede ser 'Sin ganador'.");
            }

            if (Regex.IsMatch(nombre, @"^Invitado\d+$"))
            {
                throw new ArgumentException("El nombre no puede comenzar con 'Invitado#'.");
            }
        }

        public string VerificarContrasena(string nombre)
        {
            string nombreRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%?&])[A-Za-z\d@$!%?&]{8,15}$";

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("37");
            }

            if (!Regex.IsMatch(nombre, nombreRegex))
            {
                throw new ArgumentException("37");
            }

            return nombre;
        }

        public string VerificarNombrePartida(string nombrePartida)
        {
            string nombreRegex = @"^(?! )(?!.*[\\!\\#\\$%\\&'\\(\\)\\*\\+\\-\\.,\\/\\:\\;<\\=\\>\\?\\@\\[\\\\\\]\\^_`\\{\\|\\}\\~])(?!.* {2})(?!.*\d)[\p{L} ]{4,255}(?<! )$";

            if (string.IsNullOrWhiteSpace(nombrePartida))
            {
                throw new ArgumentException("56");
            }

            if (!Regex.IsMatch(nombrePartida, nombreRegex))
            {
                throw new ArgumentException("56");
            }

            return nombrePartida;
        }

        public string VerificarMensajeChat(string mensaje)
        {
            string mensajeRegex = @"^(?! )(?!.*[\\!\\#\\$%\\&'\\(\\)\\*\\+\\-\\.,\\/\\:\\;<\\=\\>\\?\\@\\[\\\\\\]\\^_`\\{\\|\\}\\~])(?!.* {2})(?!.*\d)[\p{L} ]{4,255}(?<! )$";

            if (string.IsNullOrWhiteSpace(mensaje))
            {
                throw new ArgumentException("57");
            }

            if (!Regex.IsMatch(mensaje, mensajeRegex))
            {
                throw new ArgumentException("57");
            }

            return mensaje;
        }
    }
}
