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
    public partial class RegistroJugador : Window
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
            ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient();

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
                    MessageBox.Show(Properties.Resources.mensajeRegistroJugadorExito, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCampos();
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
    }

}
