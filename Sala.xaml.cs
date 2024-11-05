using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClienteGloomApp
{
    /// <summary>
    /// Lógica de interacción para Sala.xaml
    /// </summary>
    public partial class Sala : Window
    {
        private String identificadorUsuario;
        private String codigoSala;
        public Sala(String nombreUsuario, String codigo)
        {
            InitializeComponent();
            identificadorUsuario = nombreUsuario;
            codigoSala = codigo;
        }


        private void btnOres_Click(object sender, RoutedEventArgs e)
        {
            txtDescripcionFamilia.Text = Properties.Resources.salaDescripcionOres;
        }

        private void btnCorbat_Click(object sender, RoutedEventArgs e)
        {
            txtDescripcionFamilia.Text = Properties.Resources.salaDescripcionCorbat;
        }

        private void btnGarlo_Click(object sender, RoutedEventArgs e)
        {
            txtDescripcionFamilia.Text = Properties.Resources.salaDescripcionGarlo;
        }

        private void btnRamfez_Click(object sender, RoutedEventArgs e)
        {
            txtDescripcionFamilia.Text = Properties.Resources.salaDescripcionRamfez;
        }

        private void btnInvitarJugadores_Click(object sender, RoutedEventArgs e)
        {
            InvitacionJugador invitacionJugador = new InvitacionJugador(identificadorUsuario, codigoSala);
            invitacionJugador.Show();
        }
    }
}
