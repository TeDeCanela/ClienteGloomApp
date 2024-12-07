using ClienteGloomApp.ServicioGloom;
using System;
using System.ServiceModel;
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
        private ChatClient proxy;

        public Chat(string nombreUsuario, string numeroSala)
        {
            InitializeComponent();
            identificadorUsuario = nombreUsuario;
            identificadorSala = numeroSala;

            try
            {
                // Configuración del contexto y proxy
                InstanceContext context = new InstanceContext(this);
                proxy = new ChatClient(context);

                // Agregar jugador al chat
                //proxy.AgregarJugadorAChat(identificadorUsuario, identificadorSala);
                Console.WriteLine($"Jugador {identificadorUsuario} agregado al chat en sala {identificadorSala}.");

                // Obtener el historial de mensajes
                var historial = proxy.ObtenerHistorialMensajes();
                foreach (var mensaje in historial)
                {
                    lstChat.Items.Add($"{mensaje.nombreUsuario}: {mensaje.mensaje}");
                }

                if (lstChat.Items.Count > 0)
                {
                    lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar el chat: {ex.Message}");
                MessageBox.Show($"No se pudo conectar al chat: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
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
                    //proxy.AgregarJugadorAChat(identificadorUsuario, identificadorSala);
                    //proxy.EnviarMensaje(identificadorUsuario, identificadorSala, txtMensaje.Text);
                    txtMensaje.Clear();
                }
                else
                {
                    MessageBox.Show("No puedes enviar un mensaje vacío.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el mensaje: {ex.Message}");
                MessageBox.Show($"Error al enviar mensaje: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        lstChat.Items.Add($"{identificadorUsuario} : {mensajesChat.mensaje}");

                        string mensajeFormateado = $"{mensajesChat.nombreUsuario}: {mensajesChat.mensaje}";
                        lstChat.Items.Add(mensajeFormateado);
                        lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
                    }
                    else
                    {
                        Console.WriteLine("lstChat no está visible o habilitado.");
                    }

                });
                Console.WriteLine($"Mensaje recibido: {mensajesChat.nombreUsuario}: {mensajesChat.mensaje}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar mensaje recibido: {ex.Message}");
                MessageBox.Show($"Error al procesar mensaje recibido: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Manejo del cierre de la ventana
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (proxy != null)
                {
                    proxy.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar la conexión: {ex.Message}");
            }
        }

    }
}
