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
using ServicioGlomm;

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
            App app = (App)Application.Current;
            app.cambiarIdioma("esp");

            actualizarElementos();
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            RegistroJugador nuevaVentanada = new RegistroJugador();
            nuevaVentanada.Show();
            this.Close();
        }

        private void BtnCambiarIdiomaEspañol_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.cambiarIdioma("esp");

            actualizarElementos();
        }

        private void BtnCambiarIdiomaIngles_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.cambiarIdioma("en");

            actualizarElementos();
        }

        private void BtnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contexJugador = new InstanceContext(this);

                ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient();

                ServicioGloom.Jugador jugador = new ServicioGloom.Jugador();
                jugador.nombreUsuario = validar.VerificarNombreUsuario(txtBoxNombre.Text);
                jugador.contraseña = validar.VerificarContrasena(passwordBox.Password);
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
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensajeAdvertencia(ex.Message, ex.Message);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
            }
            catch (EndpointNotFoundException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
            }
            catch (TimeoutException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
            }
            catch (Exception ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
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

        private void BtnIniciarComoInvitado_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InstanceContext contexJugador = new InstanceContext(this);

            ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient();

            try
            {
                var jugadorCreadoinvitado = proxy.AgregarJugadorInvitado();
               
                   Inicio nuevaVentana = new Inicio(jugadorCreadoinvitado.nombreUsuario);
                    nuevaVentana.Show();
                    this.Close();
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
            }
            catch (EndpointNotFoundException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
            }
            catch (TimeoutException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
            }
            catch (Exception ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
            }

        }
        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }
    }
}
