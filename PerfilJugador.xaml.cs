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
    /// Lógica de interacción para PerfilJugador.xaml
    /// </summary>
    public partial class PerfilJugador : Window //, ServicioGloom.IServicioAdministradorCallback
    {
        public PerfilJugador()
        {
            InitializeComponent();
        }
        //se agrega un metodo de la interfaz, el response
        /*private void btnCambiarDatos_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext contextoJugador = new InstanceContext(this);

            ServicioGloom.ServicioAdministradorClient proxy = new ServicioGloom.ServicioAdministradorClient(contextoJugador);

            ServicioGloom.Jugador jugador = new ServicioGloom.Jugador();

            jugador.nombreUsuario = txtNombreUsuario.Text;
            jugador.nombre = txtNombre.Text;
            jugador.apellidos = txtApellidos.Text;
            jugador.correo = txtCorreo.Text;
            jugador.contraseña = pwdContrasena.Password;
            jugador.tipo = "Jugador";
            jugador.icono = "icono1";

            proxy.ActualizarJugador(jugador);
        }
        */
       
    }
}
