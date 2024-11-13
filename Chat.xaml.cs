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
    /// Lógica de interacción para Chat.xaml
    /// </summary>
    public partial class Chat : Window, IChatCallback
    {
        private String identificadorUsuario;
        public Chat()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext context = new InstanceContext(this);
            ServicioGloom.ChatClient proxy = new ChatClient(context);

            proxy.agregarJugador(identificadorUsuario);

           // proxy.enviarMensaje(identificadorUsuario, InputMensaje.Text);
        }

        void IChatCallback.enviarMensajeCliente(ServicioGloom.Chat mensajesChat)
        {
            /*try
            {
                lstChat.Dispatcher.Invoke(() =>
                {
                    if (lstChat.Visibility == Visibility.Visible && lstChat.IsEnabled)
                    {
                        lstChat.Items.Add($"{identificadorUsuario} : {mensajesChat.mensaje}");
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
            }*/
        }
    }
}
