using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClienteGloomApp
{
    public static class MensajesEmergentes
    {
        private static readonly Dictionary<string, string> mensajesErrores;

        static MensajesEmergentes()
        {
            mensajesErrores = new Dictionary<string, string>
        {
            { "1", Properties.Resources.mensajeExp01},
            { "2", Properties.Resources.mensajeExp02},
            { "3", Properties.Resources.mensajeExp03},
            { "4", Properties.Resources.mensajeExp04},
            { "5", Properties.Resources.mensajeExp05},
            { "6", Properties.Resources.mensajeExp06},
            { "7", Properties.Resources.mensajeExp07},
            { "10", Properties.Resources.mensajeExp10},
            { "53", Properties.Resources.mensajeExp53},
            { "4060", Properties.Resources.mensajeExp54060},
            { "10054", Properties.Resources.mensajeExp10054},
            { "10060", Properties.Resources.mensajeExp10060},
            { "11001", Properties.Resources.mensajeExp11001},
            { "18456", Properties.Resources.mensajeExp18456}
        };

        }
        public static void MostrarMensaje(string codigoError, string mensajeError)
        {
            string mensaje = mensajesErrores.TryGetValue(codigoError, out string valor)
            ? valor
            : mensajeError;

            MessageBox.Show(mensaje, "Msg", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
