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
    /// Lógica de interacción para HistorialPartidas.xaml
    /// </summary>
    public partial class HistorialPartidas : Window
    {
        public HistorialPartidas(String nombreUsuario)
        {
            InitializeComponent();
            lblNombreUsuarioRegistrado.Content = nombreUsuario;
            LLenarTabla();
        }

        private void LLenarTabla()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
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
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            string participantes="";
            try
            {
                InstanceContext contextoHistorial = new InstanceContext(this);
                ServicioGloom.ServicioHistorialPartidaClient proxy = new ServicioGloom.ServicioHistorialPartidaClient(contextoHistorial);

                var historial = proxy.ObtenrParticipantesDeJuego(idSala);

                participantes = string.Join(", ", historial);
            }
            catch (EndpointNotFoundException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
            }
            catch (TimeoutException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
            }
            catch (Exception ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
            }
            return participantes;
        }

        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            Inicio nuevaVentana = new Inicio(lblNombreUsuarioRegistrado.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }

        public void EmpezarJuego()
        {
            throw new NotImplementedException();
        }

        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }
    }
}

