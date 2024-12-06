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
    /// Lógica de interacción para HistorialPartidas.xaml
    /// </summary>
    public partial class HistorialPartidas : Window
    {
        /*public HistorialPartidas(String nombreUsuario)
        {
            InitializeComponent();
            lblNombreUsuarioRegistrado.Content = nombreUsuario;
            LLenarTabla();
        }

        private void LLenarTabla()
        {
            try
            {
                InstanceContext contextoHistorial= new InstanceContext(this);
                ServicioGloom.ServicioHistorialPartidaClient proxy = new ServicioGloom.ServicioHistorialPartidaClient(contextoHistorial);

                var historial = proxy.ObtenerDatosHistorial(lblNombreUsuarioRegistrado.Content.ToString());

                if (historial != null)
                {
                    var historialPartida = historial.Select(j => new Sala
                    {
                        ganador = j.ganador,       
                        tipoPartida = j.tipoPartida,                 
                        jugador = ObtenerListaJugadores(j.idSala),
                        idAdministrador = ValidarGanador(j.ganador),
                    }).ToList();
                    dgHistorial.ItemsSource = historialPartida;
                }

                
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }

        private String ValidarGanador(String ganador)
        {
            if (ganador.Equals(lblNombreUsuarioRegistrado.Content.ToString()))
            {
                return Properties.Resources.historialGanador;
            }
            else
            {
                return Properties.Resources.historialPerdedor;
            }
        }

        private String ObtenerListaJugadores(String idSala)
        {
            InstanceContext contextoHistorial = new InstanceContext(this);
            ServicioGloom.ServicioHistorialPartidaClient proxy = new ServicioGloom.ServicioHistorialPartidaClient(contextoHistorial);

            var historial = proxy.ObtenrParticipantesDeJuego(idSala);

               string participantes = string.Join(", ", historial);
               
                return participantes;
        }

        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            Inicio nuevaVentana = new Inicio(lblNombreUsuarioRegistrado.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }

        public void EmpezarJuego()
        {
            throw new NotImplementedException();
        }*/
    }
}

