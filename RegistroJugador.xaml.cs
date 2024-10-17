using ClienteGloomApp.ServicioGloom;
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
    /// Lógica de interacción para RegistroUsuario.xaml
    /// </summary>
    public partial class RegistroJugador : Window, ServicioGloom.IServicioAdministradorCallback
    {
        public RegistroJugador()
        {
            InitializeComponent();
        }

        void IServicioAdministradorCallback.Response(int result)
        {
            throw new NotImplementedException();
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext contextoJugador = new InstanceContext(this);

            ServicioGloom.ServicioAdministradorClient proxy = new ServicioGloom.ServicioAdministradorClient(contextoJugador);

            ServicioGloom.Jugador jugador = new ServicioGloom.Jugador();

            //se obtienen los datos de los txt

            jugador.nombreUsuario = txtBoxNombreUsuario.Text;
            jugador.nombre = txtBoxNombre.Text;
            jugador.apellidos = txtBoxApellidos.Text;
            jugador.correo = txtBoxCorreo.Text;
            jugador.contraseña = passwordBox;
            jugador.tipo = "Jugador";
            jugador.icono = 1;




            //proxy.AgregarJugador(jugador);
        }
    }
}
