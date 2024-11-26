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
    /// Lógica de interacción para CrearPartida.xaml
    /// </summary>
    public partial class CrearPartida : Window, ISalaCallback
    {
        private string identificadorUsuario;
        private string tipoSalaSeleccionada; // Almacena si es "Normal" o "Mini historia"
        private int numeroJugadoresSeleccionado; // Almacena el número de jugadores (2, 3 o 4)
        private string tipoPartidaSeleccionada;
        //private string codigoSala;
        public CrearPartida(String nombreUsuario)
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            identificadorUsuario = nombreUsuario;
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext contextoPartida = new InstanceContext(this);

            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoPartida);

            ServicioGloom.Sala sala = new ServicioGloom.Sala();

            sala.nombreSala = txtNombreSala.Text;
            sala.tipoSala = tipoSalaSeleccionada;
            sala.tipoPartida = tipoPartidaSeleccionada;
            sala.noJugadores = numeroJugadoresSeleccionado;
            sala.idAdministrador = identificadorUsuario;
            sala.ganador = "Sin ganador";
            sala.fecha = ObtenerFecha();

            try
            {
                int resultadoOperacion = proxy.CrearPartida(sala);
                if (resultadoOperacion == 1)
                {

                    MessageBox.Show(Properties.Resources.mensajePartidaCreadaExitosa, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    if(tipoSalaSeleccionada == "Mini historia")
                    {
                        CambiarASalaMiniJuego();
                        //var salaMiniJuego = proxy.BuscarSalaExistente();
                    }
                    else{
                        CambiarASalaNormal();
                    }
                    
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }


        }

        private string ObtenerFecha()
        {
            System.DateTime datoFecha = DateTime.Now;

            string fechaFormato = datoFecha.ToString("dd/MM/yy");

            return fechaFormato;
        }

        

        void CambiarASalaNormal()
        {
            string codigoSalaNormal = ObtenerCodigoDeSala(identificadorUsuario, txtNombreSala.Text);

            InstanceContext contexto = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contexto);

            var salaNormal = proxy.BuscarSalaExistente(codigoSalaNormal, codigoSalaNormal);

            SalaNormal sala = new SalaNormal(identificadorUsuario, salaNormal);
            sala.Show();
            this.Close();
        }

        void CambiarASalaMiniJuego()
        {
            string codigoSalaMini = ObtenerCodigoDeSala(identificadorUsuario, txtNombreSala.Text);

            InstanceContext contexto = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contexto);

            var salaMini = proxy.BuscarSalaExistente(codigoSalaMini, codigoSalaMini);
            //string codigoSalaMini = ObtenerCodigoDeSala(identificadorUsuario, txtNombreSala.Text);
            SalaMiniJuego sala = new SalaMiniJuego(identificadorUsuario, salaMini);
            sala.Show();
            this.Close();
        }

        private string ObtenerCodigoDeSala(string usuarioAdminsitrador, string nombreSala)
        {
            InstanceContext contexto = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contexto);
            //ServicioGloom.Amistad amistad = new ServicioGloom.Amistad();
            string codigo = proxy.ObtenerCodigoSala(usuarioAdminsitrador, nombreSala);
            return codigo;
        }


        void ISalaCallback.EmpezarJuego()
        {
            // Implementación del método
        }

        void ISalaCallback.ActualizarNumeroJugadores()
        {
            // Implementación del método
        }

        void ISalaCallback.ActualizarImagenPersonaje(string personaje, string imagen)
        {
            // Implementación del método
        }

        //void ISalaCallback.ActualizarSalasActivas(List<Sala> salasActivas)
        //{
            // Implementación del método
        //}

        void ISalaCallback.ResultadoUnirseASala(string resultado, string mensaje, bool exito)
        {
            // Implementación del método
        }

        void ISalaCallback.ActualizarSalasActivas(ServicioGloom.Sala[] salasActivas)
        {
            throw new NotImplementedException();
        }

        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            Inicio nuevaVentana = new Inicio(identificadorUsuario);
            nuevaVentana.Show();
            this.Close();
        }

        void ISalaCallback.ActualizarSeleccionFamilia(string nombreUsuario, string nombreFamilia)
        {
            throw new NotImplementedException();
        }

        private void BtnSalaNormal_Click(object sender, RoutedEventArgs e)
        {
            tipoSalaSeleccionada = "Normal";
        }

        private void BtnSalaMiniHistoria_Click(object sender, RoutedEventArgs e)
        {
            tipoSalaSeleccionada = "Mini historia";
        }

        private void BtnNo2_Click(object sender, RoutedEventArgs e)
        {
            numeroJugadoresSeleccionado = 2;
        }

        private void BtnNo3_Click(object sender, RoutedEventArgs e)
        {
            numeroJugadoresSeleccionado = 3;
        }

        private void BtnNo4_Click(object sender, RoutedEventArgs e)
        {
            numeroJugadoresSeleccionado = 4;
        }

        private void BtnPartidaPublica_Click(object sender, RoutedEventArgs e)
        {
            tipoPartidaSeleccionada = "Pública";
        }

        private void BtnPartidaPrivada_Click(object sender, RoutedEventArgs e)
        {
            tipoPartidaSeleccionada = "Privada";
        }
    }
    
}


