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

    public partial class PerfilJugador : Window , ServicioGloom.IServicioAdministradorCallback
    {
        private String identificadorUsuario;
        private String iconoSeleccionado;
        public PerfilJugador(String nombreUsuario)
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            identificadorUsuario = nombreUsuario;   
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RellenarCamposDesdeJugador();
        }

        public void RellenarCamposDesdeJugador()
        {
            InstanceContext contextoJugador = new InstanceContext(this);

            ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient(contextoJugador);

            ServicioGloom.Jugador jugador = new ServicioGloom.Jugador();

            jugador = proxy.ObtenerJugador(identificadorUsuario);
            try {
                string rutaRelativa = jugador.icono; ;
            BitmapImage bitmap = new BitmapImage(new Uri($"pack://application:,,,{rutaRelativa}", UriKind.Absolute));
            imgFotoPerfil.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}");
            }

            lblNombreUsuarioRegistrado.Content = jugador.nombreUsuario;
            txtNombre.Text = jugador.nombre;
            txtApellidos.Text = jugador.apellidos;
            txtCorreo.Text = jugador.correo;
            pwdContrasena.Password = jugador.contraseña;
            
        }
        private void btnCambiarDatos_Click(object sender, RoutedEventArgs e)
        {


            InstanceContext contextoJugador = new InstanceContext(this);

            ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient(contextoJugador);

            ServicioGloom.Jugador jugador = new ServicioGloom.Jugador();

            jugador.nombreUsuario = lblNombreUsuarioRegistrado.Content.ToString();
            jugador.nombre = txtNombre.Text;
            jugador.apellidos = txtApellidos.Text;
            jugador.correo = txtCorreo.Text;
            jugador.contraseña = pwdContrasena.Password;
            jugador.tipo = "Jugador";
            jugador.icono = iconoSeleccionado;

            try
            {
                int resulatdoIperacion = proxy.ActualizarJugador(jugador);
                if (resulatdoIperacion == 1)
                {
                    MessageBox.Show(Properties.Resources.mensajeActualizacionExitosa, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }

        }

        void IServicioAdministradorCallback.Response(int result)
        {
            throw new NotImplementedException();
        }

        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            Inicio nuevaVentana = new Inicio(lblNombreUsuarioRegistrado.Content.ToString());
            nuevaVentana.Show();
            this.Close();
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
    }
}
