using ClienteGloomApp.ServicioGloom;
using System.ServiceModel;
using System;
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
    /// Lógica de interacción para Chat.xaml
    /// </summary>
    public partial class Chat : Window, IChat, IChatCallback
    {
        private String identificadorUsuario;
        private String identificadorSala;
        public Chat(String nombreUsuario, String numeroSala)
        {
            InitializeComponent();
            identificadorUsuario = nombreUsuario;
            identificadorSala = numeroSala;

            InstanceContext context = new InstanceContext(this);
            ServicioGloom.ChatClient proxy = new ServicioGloom.ChatClient(context);

            proxy.AgregarJugadorAChat(identificadorUsuario);
        }

        void IChat.AgregarJugadorAChat(string nombreUsuario)
        {
            throw new NotImplementedException();
        }

        Task IChat.AgregarJugadorAChatAsync(string nombreUsuario)
        {
            throw new NotImplementedException();
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext context = new InstanceContext(this);
            ServicioGloom.ChatClient proxy = new ChatClient(context);

            proxy.EnviarMensaje(identificadorUsuario, InputMensaje.Text);
        }

        void IChat.EnviarMensaje(string nombreUsuario, string mensaje)
        {
            throw new NotImplementedException();
        }

        Task IChat.EnviarMensajeAsync(string nombreUsuario, string mensaje)
        {
            throw new NotImplementedException();
        }

        /*void IChatCallback.EnviarMensajeCliente(Chat mensajesChat)
        {
            try
            {
                lstChat.Dispatcher.Invoke(() =>
                {
                    if (lstChat.Visibility == Visibility.Visible && lstChat.IsEnabled)
                    {
                        lstChat.Items.Add($"{identificadorUsuario} : {mensajesChat}");
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.mensajeChatNoHabilitado, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }*/

        void IChatCallback.EnviarMensajeCliente(ServicioGloom.Chat mensajesChat)
        {
            try
            {
                lstChat.Dispatcher.Invoke(() =>
                {
                    if (lstChat.Visibility == Visibility.Visible && lstChat.IsEnabled)
                    {
                        lstChat.Items.Add($"{identificadorUsuario} : {mensajesChat}");
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.mensajeChatNoHabilitado, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }

        ServicioGloom.Chat[] IChat.ObtenerHistorialMensajes()
        {
            throw new NotImplementedException();
        }

        Task<ServicioGloom.Chat[]> IChat.ObtenerHistorialMensajesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
