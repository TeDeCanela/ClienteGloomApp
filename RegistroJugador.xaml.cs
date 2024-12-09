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
    /// <summary>
    /// Lógica de interacción para RegistroUsuario.xaml
    /// </summary>
    public partial class RegistroJugador : Window
    {
        private String iconoSeleccionado="sin incono";
        ValidacionCampos validar = new ValidacionCampos();
        public RegistroJugador()
        {
            InitializeComponent();
            
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InstanceContext contextoJugador = new InstanceContext(this);
            ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient(contextoJugador);

            ServicioGloom.Jugador jugador = new ServicioGloom.Jugador();

            try
            {
                jugador.nombreUsuario = validar.VerificarNombreUsuario(txtBoxNombreUsuario.Text);
                jugador.nombre = validar.VerificarNombreYApellidos(txtBoxNombre.Text);
                jugador.apellidos = validar.VerificarNombreYApellidos(txtBoxApellidos.Text);
                jugador.correo = validar.VerificarCorreo(txtBoxCorreo.Text);
                jugador.contraseña = validar.VerificarContrasena(pwdContrasena.Password);
                jugador.tipo = "JugadorRegistrado";
                jugador.icono= validar.VerificarInconoSeleccionado(iconoSeleccionado);

                int resulatdoIperacion = proxy.AgregarJugador(jugador);

                if (resulatdoIperacion == 1)
                {
                    MessageBox.Show(Properties.Resources.mensajeRegistroJugadorExito, Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCampos();
                }
            }
            catch (ArgumentException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
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
        private void LimpiarCampos()
        {
            txtBoxNombreUsuario.Text = string.Empty;
            txtBoxNombre.Text = string.Empty;
            txtBoxApellidos.Text = string.Empty;
            txtBoxCorreo.Text = string.Empty;
            pwdContrasena.Password = string.Empty;
            iconoSeleccionado = "sin incono";

            foreach (var conjuntoBotones in panelbotones.Children)
            {
                if (conjuntoBotones is Button boton)
                {
                    boton.BorderBrush = null;
                    boton.BorderThickness = new Thickness(1);

                }
            }
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

        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }

        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }
    }

}
