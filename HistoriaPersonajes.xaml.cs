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

        private void BtnMerit_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaMerit;
        }

        private void BtnNeferu_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaNeferu;
        }

        private void BtnSeti_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaSeti;
        }

        private void BtnSobek_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaSobek;
        }

        private void BtnTucani_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaTucani;
        }

        private void BtnLusiel_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaLusiel;
        }

        private void BtnAngelus_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaAngelus;
        }

        private void BtnLuan_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaLuan;
        }

        private void BtnGaia_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaGaia;
        }

        private void BtnArialyn_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaArialyn;
        }

        private void BtnAris_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaAris;
        }

        private void BtnAbelith_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaAbelith;
        }

        private void BtnDidorian_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaDidorian;
        }

        private void BtnZael_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaZael;
        }

        private void BtnPablian_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaPablian;
        }

        private void BtnLorenzeo_Click(object sender, RoutedEventArgs e)
        {
            txtHistoria.Text = Properties.Resources.historiaLorenzeo;
        }

        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            Inicio nuevaVentana = new Inicio(lblNombreUsuarioRegistrado.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }

    }
}
