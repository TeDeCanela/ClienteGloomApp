using ClienteGloomApp.ServicioGloom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Lógica de interacción para InvitacionJugador.xaml
    /// </summary>
    public partial class InvitacionJugador : Window
    {
        
        private String identificadorUsuario;
        private String codigoSala;
        private String correoAmigo;
        public readonly IInvitacion invitacion = new ServicioGloom.InvitacionClient();
         
        public InvitacionJugador(String nombreUsuario, String codigo)
        {
            InitializeComponent();
            identificadorUsuario = nombreUsuario;
            codigoSala = codigo;
            CargarListaAmigos(nombreUsuario);
        }
        private void CargarListaAmigos(string usuario)
        {
            try
            {
                InstanceContext contexto = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient(contexto);
                var listaAmigos = proxy.ObtenerListaAmigos(usuario);

                var nombresAmigos = listaAmigos.Select(a => a.jugadorAmigo.nombreUsuario).ToList();

                lstListaAmigos.ItemsSource = nombresAmigos;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
            }
        }
        private void btnInvitarCorreo_Click(object sender, RoutedEventArgs e)
        {
            EnviarCorreoInvitado();
        }
        private string ObtenerCorreoAmigo(string nombreUsuarioAmigo)
        {
            string correo = "";
            try
            {
                InstanceContext contexto = new InstanceContext(this);
                ServicioGloom.AmigosClient proxy = new ServicioGloom.AmigosClient(contexto);
                correo = proxy.ObtenerCorreoAmigo(nombreUsuarioAmigo);
                
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
            }
            return correo;
        }

        private void EnviarCorreoInvitado()
        {
            try
            {
                bool resultadoEnvioInvitacion = invitacion.EnviarInvitacion(txtCorreo.Text, codigoSala, identificadorUsuario);
                if (resultadoEnvioInvitacion)
                {
                    MessageBox.Show(Properties.Resources.mensajeCorreoEnviadoExitoso, Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.mensajeCorreoEnviadoFallido, Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
            }
        }
        private void lstListaAmigos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string usuarioAmigoSeleccionado = (string)lstListaAmigos.SelectedItem;
            if (string.IsNullOrEmpty(usuarioAmigoSeleccionado))
            {
                MessageBox.Show(Properties.Resources.mensajeCorreoEnviadoFallido, Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            correoAmigo = ObtenerCorreoAmigo(usuarioAmigoSeleccionado);
        }
        private void btnInvitar_Click(object sender, RoutedEventArgs e)
        {
            EnviarCorreoAmigo(correoAmigo);
        }
        private void EnviarCorreoAmigo(String correoAmigo)
        {
            try
            {
                bool resultadoEnvioInvitacion = invitacion.EnviarInvitacion(correoAmigo, codigoSala, identificadorUsuario);
                if (resultadoEnvioInvitacion)
                {
                    MessageBox.Show(Properties.Resources.mensajeCorreoEnviadoExitoso, Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.mensajeCorreoEnviadoFallido, Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
            }
        }

        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }
    }
}