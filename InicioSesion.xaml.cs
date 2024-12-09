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
using System.ServiceModel;
using ClienteGloomApp.ServicioGloom;

namespace ClienteGloomApp
{
    /// <summary>
    /// Lógica de interacción para InicioSesion.xaml
    /// </summary>
    public partial class InicioSesion : Window
    {
        ValidacionCampos validar = new ValidacionCampos();
        public InicioSesion()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("esp");
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            RegistroJugador nuevaVentanada = new RegistroJugador();
            nuevaVentanada.Show();
            this.Close();
        }

        private void btnCambiarIdiomaEspañol_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.cambiarIdioma("esp");

            actualizarElementos();
        }

        private void btnCambiarIdiomaIngles_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.cambiarIdioma("en");

            actualizarElementos();
        }

        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext contexJugador = new InstanceContext(this);

            ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient();

            ServicioGloom.Jugador jugador = new ServicioGloom.Jugador();
            try
            {
                jugador.nombreUsuario = txtBoxNombre.Text;//validar.VerificarNombreUsuario(txtBoxNombre.Text);
                jugador.contraseña = passwordBox.Password;//validar.VerificarContrasena(passwordBox.Password);
                int resultado = proxy.AutenticarJugador(jugador);
                if (resultado == 1)
                {
                    Inicio nuevaVentana = new Inicio(jugador.nombreUsuario);
                    nuevaVentana.Show();
                    this.Close();
                }
            }
            catch (ArgumentException ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
            }
        }

        private void actualizarElementos()
        {
            lblNombreUsuario.Content = Properties.Resources.globalNombreUsuario;
            lblContraseña.Content = Properties.Resources.globalContraseña;
            btnIniciarSesion.Content = Properties.Resources.inicioSesionBtnIniciarSesion;
            btnJugarComoInvitado.Content = Properties.Resources.inicioSesionBtnJugarComoInvitado;
            btnRegistrarse.Content = Properties.Resources.inicioSesionBtnRegistrarse;


        }

        private void btnIniciarComoInvitado_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext contexJugador = new InstanceContext(this);

            ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient(contexJugador);

            try
            {
                var jugadorCreadoinvitado = proxy.AgregarJugadorInvitado();
               
                   Inicio nuevaVentana = new Inicio(jugadorCreadoinvitado.nombreUsuario);
                    nuevaVentana.Show();
                    this.Close();
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
            }
        }
    }
}
