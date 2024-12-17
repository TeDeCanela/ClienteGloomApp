using ClienteGloomApp.ServicioGloom;
using ServicioGlomm;
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

    public partial class PerfilJugador : Window
    {
        private String identificadorUsuario;
        private String iconoSeleccionado;
        ValidacionCampos validar = new ValidacionCampos();

        public PerfilJugador(String nombreUsuario)
        {
            InitializeComponent();
            lblNombreUsuarioRegistrado.Content = nombreUsuario;
            identificadorUsuario = nombreUsuario;
            RellenarCamposDesdeJugador();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RellenarCamposDesdeJugador();
        }

        public void RellenarCamposDesdeJugador()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoJugador = new InstanceContext(this);

                ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient();

                ServicioGloom.Jugador jugador = new ServicioGloom.Jugador();

                jugador = proxy.ObtenerJugador(identificadorUsuario);
                if (RutasDeCartas.RutasImagenesPerfiles.TryGetValue(jugador.icono, out var rutaImagen))
                {
                    imgFotoPerfil.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                }


               
                txtNombre.Text = jugador.nombre;
                txtApellidos.Text = jugador.apellidos;
                txtCorreo.Text = jugador.correo;
                iconoSeleccionado = jugador.icono;

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
                DeshabilitarCampos();
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }
        private void BtnCambiarDatos_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());

            ServicioGloom.Jugador jugador = new ServicioGloom.Jugador();
            try
            {

                jugador.nombreUsuario = lblNombreUsuarioRegistrado.Content.ToString();
                jugador.nombre = validar.VerificarNombreYApellidos(txtNombre.Text);
                jugador.apellidos = validar.VerificarNombreYApellidos(txtApellidos.Text);
                jugador.correo = validar.VerificarCorreo(txtCorreo.Text);
                jugador.tipo = "Jugador";
                jugador.icono = iconoSeleccionado;

                ValidarTipoDeCambio(jugador);

            }
            catch (ArgumentException ex)
            {
                MensajesEmergentes.MostrarMensajeAdvertencia(ex.Message, ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }

        }

        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            Inicio nuevaVentana = new Inicio(lblNombreUsuarioRegistrado.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }
        private void BtnPerfilCalavera_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilCalavera.png";
        }

        private void BtnPerfilCorazon_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilCorazon.png";
        }

        private void BtnPerfilDiamante_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilDiamante.png";
        }

        private void BtnPerfilCastillo_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilCastillo.png";
        }

        private void BtnPerfilCorona_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilCorona.png";
        }

        private void BtnPerfilCastillo2_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilCastillo2.png";
        }

        private void BtnPerfilUnicornio_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilUnicornio.png";
        }

        private void BtnPerfilVela_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilVela.png";
        }

        private void BtnPerfilEspada_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilEspada.png";
        }

        private void BtnPerfilEscudo_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilEscudo.png";
        }

        private void CambiarEstiloBotones(object sender)
        {
            Button botonSeleccionada = sender as Button;
            botonSeleccionada.BorderBrush = new SolidColorBrush(Colors.Magenta);
            botonSeleccionada.BorderThickness = new Thickness(4);

            foreach (var child in panelbotones.Children)
            {
                if (child is Button botonesDeContendero && botonesDeContendero != botonSeleccionada)
                {

                    botonesDeContendero.BorderBrush = null;
                }
            }
        }

        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }

        private void DeshabilitarCampos()
        {
            txtApellidos.IsEnabled = false;
            txtCorreo.IsEnabled = false;
            txtNombre.IsEnabled = false;
        }

        private void ValidarTipoDeCambio(Jugador jugador)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InstanceContext contextoJugador = new InstanceContext(this);
            ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient();
            try
            {
                if (string.IsNullOrEmpty(pwdContrasena.Password))
                {
                    jugador.contraseña = "sin contraseña nueva";
                    proxy.ActualizarJugadorSinContrasena(jugador);
                    MessageBox.Show(Properties.Resources.mensajeActualizacionExitosa, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    jugador.contraseña = validar.VerificarContrasena(pwdContrasena.Password);
                    proxy.ActualizarJugador(jugador);
                    MessageBox.Show(Properties.Resources.mensajeActualizacionExitosa, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (ArgumentException ex)
            {
                MensajesEmergentes.MostrarMensajeAdvertencia(ex.Message, ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
            }

        }
    }
}
