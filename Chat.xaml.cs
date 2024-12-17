using System;
using System.ServiceModel;
using System.Windows;
using ClienteGloomApp.ServicioGloom;

namespace ClienteGloomApp
{
    public partial class Chat : Window, IChatCallback
    {
        private IChat proxy;
        private string nombreUsuario;
        private string idSala;

        public Chat(string nombreUsuario, string idSala)
        {
            InitializeComponent();
            this.nombreUsuario = nombreUsuario;
            this.idSala = idSala;

            InstanceContext context = new InstanceContext(this);
            proxy = new ChatClient(context); // Conexión al servicio de juego
            proxy.AgregarJugadorAChat(nombreUsuario, idSala); // Actualización para reflejar el cambio en el servicio

            // Cargar mensajes anteriores
            var historial = proxy.ObtenerHistorialMensajes(idSala);
            foreach (var mensaje in historial)
            {
                lstChat.Items.Add($"{mensaje.nombreUsuario}: {mensaje.mensaje}");
            }
        }



        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            string mensaje = txtMensaje.Text.Trim();
            if (!string.IsNullOrEmpty(mensaje))
            {
                proxy.EnviarMensaje(nombreUsuario, mensaje, idSala);
                txtMensaje.Clear();
            }
        }

        public void RecibirMensaje(ServicioGloom.Chat mensaje)
        {
            if (mensaje == null)
            {
                Console.WriteLine("El mensaje recibido es nulo.");
                return;
            }

            Console.WriteLine($"Mensaje recibido en el cliente: {mensaje.nombreUsuario}: {mensaje.mensaje}");

            lstChat.Dispatcher.Invoke(() =>
            {
                if (lstChat != null)
                {
                    lstChat.Items.Add($"{mensaje.nombreUsuario}: {mensaje.mensaje}");
                }
                else
                {
                    Console.WriteLine("lstChat no está inicializado.");
                }
<<<<<<< Updated upstream
            });
=======
            }
            catch (ArgumentException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
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
                DirigirJugadorInicioDeSesion();
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

        /// <summary>
        /// Método del callback para recibir mensajes.
        /// </summary>
        public void EnviarMensajeCliente(ServicioGloom.Chat mensajesChat)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
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
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }


        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
>>>>>>> Stashed changes
        }
    }
}
