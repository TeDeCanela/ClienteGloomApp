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
    public partial class RegistroJugador : Window, IServicioJuegoTableroCallback
    {
        private String iconoSeleccionado="sin incono";
        ValidacionCampos validar = new ValidacionCampos();
        public RegistroJugador()
        {
            InitializeComponent();
            
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext contextoJugador = new InstanceContext(this);
            ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient(contextoJugador);
            ServicioGloom.Jugador jugador = new ServicioGloom.Jugador();

            try
            {
                jugador.nombreUsuario = txtBoxNombreUsuario.Text;
                jugador.nombre = txtBoxNombre.Text;
                jugador.apellidos = txtBoxApellidos.Text;
                jugador.correo = validar.VerificarCorreo(txtBoxCorreo.Text);
                jugador.contraseña = pwdContrasena.Password;
                jugador.tipo = "JugadorRegistrado";
                jugador.icono= validar.VerificarInconoSeleccionado(iconoSeleccionado);

                int resulatdoIperacion = proxy.AgregarJugador(jugador);

                if (resulatdoIperacion == 1)
                {
                    MessageBox.Show(Properties.Resources.mensajeRegistroJugadorExito, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCampos();
                }
            }
            catch (ArgumentException ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Message.ToString(), ex.Message.ToString());
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
            
        }
        private void LimpiarCampos()
        {
            txtBoxNombreUsuario.Text = string.Empty;
            txtBoxNombre.Text = string.Empty;
            txtBoxApellidos.Text = string.Empty;
            txtBoxCorreo.Text = string.Empty;
            pwdContrasena.Password = string.Empty;
        }

        private void btnPerfilCalavera_Click(object sender, RoutedEventArgs e)
        {
            cambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilCalavera.png";
        }

        private void btnPerfilCorazon_Click(object sender, RoutedEventArgs e)
        {
            cambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilCorazon.png";
        }

        private void btnPerfilDiamante_Click(object sender, RoutedEventArgs e)
        {
            cambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilDiamante.png";
        }

        private void btnPerfilCastillo_Click(object sender, RoutedEventArgs e)
        {
            cambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilCastillo.png";
        }

        private void btnPerfilCorona_Click(object sender, RoutedEventArgs e)
        {
            cambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilCorona.png";
        }

        private void btnPerfilCastillo2_Click(object sender, RoutedEventArgs e)
        {
            cambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilCastillo2.png";
        }

        private void btnPerfilUnicornio_Click(object sender, RoutedEventArgs e)
        {
            cambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilUnicornio.png";
        }

        private void btnPerfilVela_Click(object sender, RoutedEventArgs e)
        {
            cambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilVela.png";
        }

        private void btnPerfilEspada_Click(object sender, RoutedEventArgs e)
        {
            cambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilEspada.png";
        }

        private void btnPerfilEscudo_Click(object sender, RoutedEventArgs e)
        {
            cambiarEstiloBotones(sender);
            iconoSeleccionado = "/Imagenes/PerfilEscudo.png";
        }

        private void cambiarEstiloBotones(object sender)
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

        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }

        public void EnviarTurno(string nombreDelUsusarioEnTurno)
        {
            throw new NotImplementedException();
        }
    }

}
