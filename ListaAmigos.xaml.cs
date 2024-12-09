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
    /// Lógica de interacción para ListaAmigos.xaml
    /// </summary>
    public partial class ListaAmigos : Window
    {
        /*public ListaAmigos(String nombreusuarioRegistrado)
        {
            InitializeComponent();
            lblNombreUsuarioRegistrado.Content = nombreusuarioRegistrado;
        }

        private void txtBuscador_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                InstanceContext contextoJugador = new InstanceContext(this);
                ServicioGloom.JugadorClient proxy = new ServicioGloom.JugadorClient(contextoJugador);

                var jugadores = proxy.BuscarJugadoresPorNombreUsuario(txtBuscador.Text).Select(j => new Jugador { nombreUsuario = j.nombreUsuario })
                                     .ToList();

                dgResultados.ItemsSource = jugadores;

                proxy.Close();
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }

        }

        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
           Inicio nuevaVentana = new Inicio(lblNombreUsuarioRegistrado.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }
        private void btnAñadirAmigo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext contextoAmistad = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient(contextoAmistad);

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
                MessageBox.Show(ex.Message, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
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

        private void btnVerSolicitudes_Click(object sender, RoutedEventArgs e)
        {
            panelSolicitudes.Visibility = Visibility.Visible;
            LlenarTablaSolcitudes();
        }

        private void btnFlechaPanel_Click(object sender, RoutedEventArgs e)
        {
            panelSolicitudes.Visibility = Visibility.Collapsed;
        }

        private void btnAceptarSolicitud_Click(object sender, RoutedEventArgs e)
        {
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
                LlenarTablaSolcitudes();
                proxy.Close();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }

        }

        private void LlenarTablaSolcitudes()
        {
            try
            {
                InstanceContext contextoAmistad = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient();
               
                    var solicitudes = proxy.ObtenerSolicitudesDeAmistadPorJugador(lblNombreUsuarioRegistrado.Content.ToString());

                    if (solicitudes != null)
                    {
                        var jugadores = solicitudes.Select(j => new Jugador { nombreUsuario = j.nombreUsuario.nombreUsuario}).ToList();

                        dgSolicitudes.ItemsSource = jugadores;
                    }
               
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }

        private void btnRechazarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                InstanceContext contextoAmistad = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient(contextoAmistad);
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
                MessageBox.Show(ex.Message, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
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

        private void btnMostrarMisAmigos_Click(object sender, RoutedEventArgs e)
        {
            panelAmigos.Visibility = Visibility.Visible;
            LlenarTablaAmigos();
        }

        private void btnFlechaPanelAmigos_Click(object sender, RoutedEventArgs e)
        {
            panelAmigos.Visibility = Visibility.Collapsed;
        }

        private void btnEliminarAmigos_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                InstanceContext contextoAmistad = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient(contextoAmistad);
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
                MessageBox.Show(ex.Message, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
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
            try
            {
                InstanceContext contextoAmistad = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient(new InstanceContext(this));

                var solicitudes = proxy.ObtenerListaAmigos(lblNombreUsuarioRegistrado.Content.ToString());

                if (solicitudes != null)
                {
                    var jugadores = solicitudes.Select(j => new Jugador { nombreUsuario = j.nombreUsuario.nombreUsuario }).ToList();

                    dgAmigos.ItemsSource = jugadores;
                }

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }*/
    }
}
