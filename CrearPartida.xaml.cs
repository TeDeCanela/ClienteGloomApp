using ClienteGloomApp.ServicioGloom;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class CrearPartida : Window
    {
        private string identificadorUsuario;
        public CrearPartida(String nombreUsuario)
        {
            InitializeComponent();
            identificadorUsuario = nombreUsuario;
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext contextoPartida = new InstanceContext(this);

            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoPartida);

            ServicioGloom.Sala sala = new ServicioGloom.Sala();

            sala.nombreSala = txtNombreSala.Text;
            sala.tipoSala = txtTipoSala.Text;
            sala.tipoPartida = txtTipoPartida.Text;
            sala.noJugadores = int.Parse(txtNumeroJugadores.Text);
            sala.idAdministrador = identificadorUsuario;
            sala.ganador = "En partida";
            sala.fecha = ObtenerFecha();

            try
            {
                int resultadoOperacion = proxy.CrearPartida(sala);
                if (resultadoOperacion == 1)
                {
                    MessageBox.Show(Properties.Resources.mensajePartidaCreadaExitosa, "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    string codigo = ObtenerCodigoSala(sala.idAdministrador, sala.nombreSala);
                    LimpiarCampos();
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }

            Sala salaVentana = new Sala(identificadorUsuario, codigo);
            salaVentana.Show();
            this.Close();

        }

        private string ObtenerFecha()
        {
            System.DateTime datoFecha = DateTime.Now;

            string fechaFormato = datoFecha.ToString("dd/MM/yy");

            return fechaFormato;
        }

        private void cambiarDeVentana(string usuario, string codigo)
        {
            Sala salaVentana = new Sala(usuario);
            salaVentana.Show();
            this.Close();
        }

        private string ObtenerCodigoSala(string administrador, string nombreSala)
        {
 
            
            InstanceContext contextoPartida = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoPartida);

            string codigoSala = proxy.BuscarCodigoSala(administrador, nombreSala);

            return codigoSala;


        }


        private void LimpiarCampos()
        {
            txtNombreSala.Text = string.Empty;
            txtNumeroJugadores.Text = string.Empty;
            txtTipoPartida.Text = string.Empty;
            txtTipoSala.Text = string.Empty;
        }

        
    }
}
