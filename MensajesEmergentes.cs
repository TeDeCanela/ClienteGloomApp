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
        private static Dictionary<string, string> ObtenerMensajesErrores()
        {
            return new Dictionary<string, string>
        {
<<<<<<< Updated upstream
            mensajesErrores = new Dictionary<string, string>
        {
            { "1", Properties.Resources.mensajeExp01},
            { "2", Properties.Resources.mensajeExp02},
            { "3", Properties.Resources.mensajeExp03},
            { "4", Properties.Resources.mensajeExp04},
            { "5", Properties.Resources.mensajeExp05},
            { "6", Properties.Resources.mensajeExp06},
            { "7", Properties.Resources.mensajeExp07},
            { "8", Properties.Resources.mensajeExp08},
            { "9", Properties.Resources.mensajeExp09},
            { "10", Properties.Resources.mensajeExp10},
            { "11", Properties.Resources.mensajeExp11},
            { "12", Properties.Resources.mensajeExp12},
            { "14", Properties.Resources.mensajeExp14},
            { "15", Properties.Resources.mensajeExp15},
            { "16", Properties.Resources.mensajeExp16},
            { "17", Properties.Resources.mensajeExp17},
            { "18", Properties.Resources.mensajeExp18},
            { "19", Properties.Resources.mensajeExp19},
            { "20", Properties.Resources.mensajeExp20},
            { "21", Properties.Resources.mensajeExp21},
            { "22", Properties.Resources.mensajeExp22},
            { "34", Properties.Resources.mensajeExp34},
            { "35", Properties.Resources.mensajeExp35},
            { "36", Properties.Resources.mensajeExp36},
            { "37", Properties.Resources.mensajeExp37},
            { "38", Properties.Resources.mensajeExp38},
            { "39", Properties.Resources.mensajeExp39},
            { "40", Properties.Resources.mensajeExp40},
            { "53", Properties.Resources.mensajeExp53},
            { "4060", Properties.Resources.mensajeExp54060},
            { "10054", Properties.Resources.mensajeExp10054},
            { "10060", Properties.Resources.mensajeExp10060},
            { "11001", Properties.Resources.mensajeExp11001},
            { "18456", Properties.Resources.mensajeExp18456}
        };
=======
        { "1", Properties.Resources.mensajeExp01},
        { "2", Properties.Resources.mensajeExp02},
        { "3", Properties.Resources.mensajeExp03},
        { "4", Properties.Resources.mensajeExp04},
        { "5", Properties.Resources.mensajeExp05},
        { "6", Properties.Resources.mensajeExp06},
        { "7", Properties.Resources.mensajeExp07},
        { "8", Properties.Resources.mensajeExp08},
        { "9", Properties.Resources.mensajeExp09},
        { "10", Properties.Resources.mensajeExp10},
        { "11", Properties.Resources.mensajeExp11},
        { "12", Properties.Resources.mensajeExp12},
        { "13", Properties.Resources.mensajeExp13},
        { "14", Properties.Resources.mensajeExp14},
        { "15", Properties.Resources.mensajeExp15},
        { "16", Properties.Resources.mensajeExp16},
        { "17", Properties.Resources.mensajeExp17},
        { "18", Properties.Resources.mensajeExp18},
        { "19", Properties.Resources.mensajeExp19},
        { "20", Properties.Resources.mensajeExp20},
        { "21", Properties.Resources.mensajeExp21},
        { "22", Properties.Resources.mensajeExp22},
        { "23", Properties.Resources.mensajeExp23},
        { "24", Properties.Resources.mensajeExp24},
        { "25", Properties.Resources.mensajeExp25},
        { "26", Properties.Resources.mensajeExp26},
        { "27", Properties.Resources.mensajeExp27},
        { "28", Properties.Resources.mensajeExp28},
        { "29", Properties.Resources.mensajeExp29},
        { "30", Properties.Resources.mensajeExp30},
        { "31", Properties.Resources.mensajeExp31},
        { "32", Properties.Resources.mensajeExp32},
        { "33", Properties.Resources.mensajeExp33},
        { "34", Properties.Resources.mensajeExp34},
        { "35", Properties.Resources.mensajeExp35},
        { "36", Properties.Resources.mensajeExp36},
        { "37", Properties.Resources.mensajeExp37},
        { "38", Properties.Resources.mensajeExp38},
        { "39", Properties.Resources.mensajeExp39},
        { "40", Properties.Resources.mensajeExp40},
        { "41", Properties.Resources.mensajeExp41},
        { "42", Properties.Resources.mensajeExp42},
        { "43", Properties.Resources.mensajeExp43},
        { "44", Properties.Resources.mensajeExp44},
        { "45", Properties.Resources.mensajeExp45},
        { "46", Properties.Resources.mensajeExp46},
        { "47", Properties.Resources.mensajeExp47},
        { "48", Properties.Resources.mensajeExp48},
        { "49", Properties.Resources.mensajeExp49},
        { "50", Properties.Resources.mensajeExp50},
        { "51", Properties.Resources.mensajeExp51},
        { "52", Properties.Resources.mensajeExp52},
        { "53", Properties.Resources.mensajeExp53},
        { "54", Properties.Resources.mensajeExp54},
        { "55", Properties.Resources.mensajeExp55},
        { "56", Properties.Resources.mensajeExp56},
        { "57", Properties.Resources.mensajeExp57},
        { "58", Properties.Resources.mensajeExp58},
        { "59", Properties.Resources.mensajeExp59},
        { "60", Properties.Resources.mensajeExp60},
        { "61", Properties.Resources.mensajeExp61},
        { "62", Properties.Resources.mensajeExp62},
        { "63", Properties.Resources.mensajeExp63},
        { "64", Properties.Resources.mensajeExp64},
        { "65", Properties.Resources.mensajeExp65},
        { "66", Properties.Resources.mensajeExp66},
        { "67", Properties.Resources.mensajeExp67},
        { "68", Properties.Resources.mensajeExp68},
        { "69", Properties.Resources.mensajeExp69},
        { "70", Properties.Resources.mensajeExp70},
        { "71", Properties.Resources.mensajeExp71},
        { "72", Properties.Resources.mensajeExp72},
        { "73", Properties.Resources.mensajeExp73},
        { "74", Properties.Resources.mensajeExp74},
        { "75", Properties.Resources.mensajeExp75},
        { "76", Properties.Resources.mensajeExp76},
        { "77", Properties.Resources.mensajeExp77},
        { "4060", Properties.Resources.mensajeExp54060},
        { "10054", Properties.Resources.mensajeExp10054},
        { "10060", Properties.Resources.mensajeExp10060},
        { "11001", Properties.Resources.mensajeExp11001},
        { "18456", Properties.Resources.mensajeExp18456}
    };
        }
>>>>>>> Stashed changes

        }
        public static void MostrarMensaje(string codigoError, string mensajeError)
        {
            var mensajesErrores = ObtenerMensajesErrores();

            string mensaje = mensajesErrores.TryGetValue(codigoError, out string valor)
                ? valor
                : mensajeError;

            MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
<<<<<<< Updated upstream
=======

        public static void MostrarMensajeAdvertencia(string codigoError, string mensajeError)
        {
            var mensajesErrores = ObtenerMensajesErrores();

            string mensaje = mensajesErrores.TryGetValue(codigoError, out string valor)
                ? valor
                : mensajeError;

            MessageBox.Show(mensaje, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
>>>>>>> Stashed changes
    }
}