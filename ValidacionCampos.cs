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
            string nombreRegex = @"^(?! )(?!.*[\\!\\#\\$%\\&'\\(\\)\\*\\+\\-\\.,\\/\\:\\;<\\=\\>\\?\\@\\[\\\\\\]\\^_`\\{\\|\\}\\~])(?!.* {2})(?!.*\d)[\p{L} ]{5,255}(?<! )$";

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
            string nombreRegex = @"^(?! )(?!.* {2})[a-zA-Z0-9_ ]+(?<! )$";

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("36");
            }

            if (!Regex.IsMatch(nombre, nombreRegex))
            {
                throw new ArgumentException("36");
            }

            return nombre;
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
    }
}
