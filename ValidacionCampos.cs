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

        public string VerificarNombreUsario(string nombreUsuario)
        {
            string correoRegex = @"^[a-zA-Z0-9._-]{3,16}$";

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                throw new ArgumentException("11");
            }

            if (!Regex.IsMatch(nombreUsuario, correoRegex))
            {
                throw new ArgumentException("11");
            }

            return nombreUsuario;
        }

    }
}
