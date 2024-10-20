using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Lógica de interacción para Inicio.xaml
    /// </summary>
    public partial class Inicio : Window {
        private String identificadorUsuario;

        public Inicio(String nombreDelUsuario)
        {
            InitializeComponent();
            lblNombreUsuario.Content = nombreDelUsuario;
  
        }

        public void Response(int result)
        {
            throw new NotImplementedException();
        }

        private void btnPerfil_Click(object sender, RoutedEventArgs e)
        {
            PerfilJugador nuevaVentana = new PerfilJugador(lblNombreUsuario.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }
    }
}
