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
    /// Lógica de interacción para ListaAmigos.xaml
    /// </summary>
    public partial class ListaAmigos : Window
    {
        public ListaAmigos(String nombreusuarioRegistrado)
        {
            InitializeComponent();
            lblNombreUsuarioRegistrado.Content = nombreusuarioRegistrado;
        }

        private void TxtBuscador_TextChanged(object sender, TextChangedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {

                InstanceContext contextoJugador = new InstanceContext(this);
                ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient();

                var jugadores = proxy.BuscarJugadoresPorNombreUsuario(txtBuscador.Text).Select(j => new Jugador { nombreUsuario = j.nombreUsuario })
                                     .ToList();

                dgResultados.ItemsSource = jugadores;

                proxy.Close();
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
        private void BtnAñadirAmigo_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoAmistad = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient();

                ServicioGloom.Amistad solicitud = new ServicioGloom.Amistad();
                var jugadorUsuario = new ServicioGloom.Jugador
                {
                    nombreUsuario = lblNombreUsuarioRegistrado.Content?.ToString(),
                };
                var jugadorAmigo = new ServicioGloom.Jugador
                {
                    nombreUsuario = ObtenerCeldaSeleccionada(),
                    
                    
                };

                solicitud.jugadorAmigo = jugadorAmigo;
                solicitud.nombreUsuario = jugadorUsuario;
                solicitud.estado = "Pendiente";

                int resultado = proxy.EnviarSolcitudAmistad(solicitud);
                proxy.Close();

                MessageBox.Show(Properties.Resources.mensajeSolcitudEnviadaExito, Properties.Resources.mensajeTituloExito, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidOperationException ex)
            {
                administradorLogger.RegistroError(ex);
                MessageBox.Show(ex.Message, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
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

        public void Response(int result)
        {
            throw new NotImplementedException();
        }

        private String ObtenerCeldaSeleccionada()
        {
            if (dgResultados.SelectedItem == null)
            {
                throw new InvalidOperationException(Properties.Resources.mensajeExp07);
            }
            var jugadorSeleccionado = dgResultados.SelectedItem as Jugador;
            return jugadorSeleccionado.nombreUsuario;
        }

        private void BtnVerSolicitudes_Click(object sender, RoutedEventArgs e)
        {
            panelSolicitudes.Visibility = Visibility.Visible;
            LlenarTablaSolcitudes();
        }

        private void BtnFlechaPanel_Click(object sender, RoutedEventArgs e)
        {
            panelSolicitudes.Visibility = Visibility.Collapsed;
        }

        private void BtnAceptarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try { 

            InstanceContext contextoAmistad = new InstanceContext(this);
            ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient();
            ServicioGloom.Amistad solicitud = new ServicioGloom.Amistad();

            var jugadorUsuario = new ServicioGloom.Jugador
            {
                nombreUsuario = ObtenerCeldaSeleccionadaSolicitudes(),
            };
            var jugadorAmigo = new ServicioGloom.Jugador
            {
                nombreUsuario = lblNombreUsuarioRegistrado.Content?.ToString(),
            };
            solicitud.jugadorAmigo = jugadorAmigo;
            solicitud.nombreUsuario = jugadorUsuario;
            solicitud.estado = "Aceptado";

            int resultado = proxy.ValidarSolcitudAmistad(solicitud);

                RegistrarJugador(lblNombreUsuarioRegistrado.Content?.ToString(), ObtenerCeldaSeleccionadaSolicitudes());
                LlenarTablaSolcitudes();
                proxy.Close();
            }
            catch (InvalidOperationException ex)
            {
                administradorLogger.RegistroError(ex);
                MessageBox.Show(ex.Message, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void LlenarTablaSolcitudes()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoAmistad = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient();

                var solicitudes = proxy.ObtenerSolicitudesDeAmistadPorJugador(lblNombreUsuarioRegistrado.Content.ToString());

                if (solicitudes != null)
                {
                    // Filtrar para excluir al jugador presente
                    var jugadores = solicitudes
                        .Where(j => j.nombreUsuario.nombreUsuario != lblNombreUsuarioRegistrado.Content.ToString()) // Excluir al jugador presente
                        .Select(j => new Jugador { nombreUsuario = j.nombreUsuario.nombreUsuario }) // Crear nuevos objetos Jugador
                        .ToList();

                    // Asignar la lista filtrada a la fuente de datos de la tabla
                    dgSolicitudes.ItemsSource = jugadores;
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

        private void BtnRechazarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {

                InstanceContext contextoAmistad = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient();
                ServicioGloom.Amistad solicitud = new ServicioGloom.Amistad();

                var jugadorUsuario = new ServicioGloom.Jugador
                {
                    nombreUsuario = ObtenerCeldaSeleccionadaSolicitudes(),
                };
                var jugadorAmigo = new ServicioGloom.Jugador
                {
                    nombreUsuario = lblNombreUsuarioRegistrado.Content?.ToString(),
                };
                solicitud.jugadorAmigo = jugadorAmigo;
                solicitud.nombreUsuario = jugadorUsuario;
                solicitud.estado = "Aceptado";

                int resultado = proxy.ArchivarAmistad(solicitud);
                LlenarTablaSolcitudes();
                proxy.Close();
            }
            catch (InvalidOperationException ex)
            {
                administradorLogger.RegistroError(ex);
                MessageBox.Show(ex.Message, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private String ObtenerCeldaSeleccionadaSolicitudes()
        {
            if (dgSolicitudes.SelectedItem == null)
            {
                throw new InvalidOperationException(Properties.Resources.mensajeExp07);
            }
            var jugadorSeleccionado = dgSolicitudes.SelectedItem as Jugador;
            return jugadorSeleccionado.nombreUsuario;
        }

        private void BtnMostrarMisAmigos_Click(object sender, RoutedEventArgs e)
        {
            panelAmigos.Visibility = Visibility.Visible;
            LlenarTablaAmigos();
        }

        private void BtnFlechaPanelAmigos_Click(object sender, RoutedEventArgs e)
        {
            panelAmigos.Visibility = Visibility.Collapsed;
        }

        private void BtnEliminarAmigos_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {

                InstanceContext contextoAmistad = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient();
                ServicioGloom.Amistad solicitud = new ServicioGloom.Amistad();

                var jugadorUsuario = new ServicioGloom.Jugador
                {
                    nombreUsuario = ObtenerCeldaSeleccionadaAmigos(),
                };
                var jugadorAmigo = new ServicioGloom.Jugador
                {
                    nombreUsuario = lblNombreUsuarioRegistrado.Content?.ToString(),
                };
                solicitud.jugadorAmigo = jugadorAmigo;
                solicitud.nombreUsuario = jugadorUsuario;
                solicitud.estado = "Aceptado";

                int resultado = proxy.ArchivarAmistad(solicitud);
                LlenarTablaAmigos();
                proxy.Close();
            }
            catch (InvalidOperationException ex)
            {
                administradorLogger.RegistroError(ex);
                MessageBox.Show(ex.Message, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private String ObtenerCeldaSeleccionadaAmigos()
        {
            if (dgAmigos.SelectedItem == null)
            {
                throw new InvalidOperationException(Properties.Resources.mensajeExp07);
            }
            var jugadorSeleccionado = dgAmigos.SelectedItem as Jugador;
            return jugadorSeleccionado.nombreUsuario;
        }

        private void LlenarTablaAmigos()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoAmistad = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient();

                var solicitudes = proxy.ObtenerListaAmigos(lblNombreUsuarioRegistrado.Content.ToString());

                if (solicitudes != null)
                {
                    var jugadores = solicitudes.Select(j => new Jugador { nombreUsuario = j.jugadorAmigo.nombreUsuario }).ToList();

                    dgAmigos.ItemsSource = jugadores;
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
        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }

        public void RegistrarJugador(string jugadorUsuarioActual, string nombreUsuarioAmigo)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoAmistad = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient();
                ServicioGloom.Amistad solicitud = new ServicioGloom.Amistad();

                var jugadorUsuario = new ServicioGloom.Jugador
                {
                    nombreUsuario = jugadorUsuarioActual,
                };
                var jugadorAmigo = new ServicioGloom.Jugador
                {
                    nombreUsuario = nombreUsuarioAmigo,

                };
                solicitud.jugadorAmigo = jugadorAmigo;
                solicitud.nombreUsuario = jugadorUsuario;
                solicitud.estado = "Aceptado";

                int resultado = proxy.EnviarSolcitudAmistad(solicitud);
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
    }
}
