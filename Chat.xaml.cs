using ClienteGloomApp.ServicioGloom;
using System;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace ClienteGloomApp
{
    /// <summary>
    /// Lógica de interacción para Chat.xaml
    /// </summary>
    public partial class Chat : Window, IChatCallback
    {
        private string identificadorUsuario;
        private string identificadorSala;
        private ChatClient proxyChat;
        ValidacionCampos validar = new ValidacionCampos();

        private static Chat instanciaUnica;

        public static Chat ObtenerInstancia(string nombreUsuario)
        {
            if (instanciaUnica == null || !instanciaUnica.IsVisible)
            {
                instanciaUnica = new Chat(nombreUsuario);
            }
            else
            {
                instanciaUnica.Focus(); 
            }
            
            return instanciaUnica;
        }

        private Chat(string nombreUsuario)
        {
            InitializeComponent();
            identificadorUsuario = nombreUsuario;


            try
            {
                InstanceContext contexto = new InstanceContext(this);  
                proxyChat = new ChatClient(contexto);

                proxyChat.AgregarJugadorAChat(identificadorUsuario);
               
                var historial = proxyChat.ObtenerHistorialMensajes();
                foreach (var mensaje in historial)
                {
                    lstChat.Items.Add($"{mensaje.nombreUsuario}: {mensaje.mensaje}");
                }
                if (lstChat.Items.Count > 0)
                {
                    lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
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

        /// <summary>
        /// Método para enviar mensajes desde el cliente.
        /// </summary>
        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtMensaje.Text))
                {
                    string mensajeVerificado = validar.VerificarMensajeChat(txtMensaje.Text);
                    proxyChat.EnviarMensaje(identificadorUsuario, mensajeVerificado);
                    txtMensaje.Clear();
                }
                else
                {
                    MessageBox.Show("68", Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
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

        /// <summary>
        /// Método del callback para recibir mensajes.
        /// </summary>
        public void EnviarMensajeCliente(ServicioGloom.Chat mensajesChat)
        {
            try
            {
                lstChat.Dispatcher.Invoke(() =>
                {
                    if (lstChat.Visibility == Visibility.Visible && lstChat.IsEnabled)
                    {
                        lstChat.Items.Add($"{mensajesChat.nombreUsuario} : {mensajesChat.mensaje}");

                        lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
                    }

                });
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }

        /// <summary>
        /// Manejo del cierre de la ventana
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            try
            {
                if (proxyChat != null)
                {
                    proxyChat.Close();
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
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