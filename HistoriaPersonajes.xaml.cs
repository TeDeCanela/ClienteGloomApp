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
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class HistoriaPersonajes : Window
    {
        public HistoriaPersonajes(String nombreUsuario)
        {
            InitializeComponent();
            lblNombreUsuarioRegistrado.Content = nombreUsuario;
        }

        private void btnMerit_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaMerit;
        }

        private void btnNeferu_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaNeferu;
        }

        private void btnSeti_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaSeti;
        }

        private void btnSobek_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaSobek;
        }

        private void btnTucani_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaTucani;
        }

        private void btnLusiel_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaLusiel;
        }

        private void btnAngelus_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaAngelus;
        }

        private void btnLuan_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaLuan;
        }

        private void btnGaia_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaGaia;
        }

        private void btnArialyn_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaArialyn;
        }

        private void btnAris_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaAris;
        }

        private void btnAbelith_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaAbelith;
        }

        private void btnDidorian_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaDidorian;
        }

        private void btnZael_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaZael;
        }

        private void btnPablian_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaPablian;
        }

        private void btnLorenzeo_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaLorenzeo;
        }

        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            Inicio nuevaVentana = new Inicio(lblNombreUsuarioRegistrado.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }
    }
}
