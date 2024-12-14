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
    /// Lógica de interacción para FinPartidaNormal.xaml
    /// </summary>
    public partial class FinPartidaNormal : Window
    {
        private string jugadorPropietario;
        private string identificadorSala;

        public FinPartidaNormal(string nombreUsuario, string ganador, string idSala)
        {
            InitializeComponent();
            jugadorPropietario = nombreUsuario;
            identificadorSala = idSala;

            lblJugador1.Content = Properties.Resources.palabraGanador + ": " + ganador;
            AsignarJugadores();
            //lblDialogoGanador.Content = Properties.Resources.finPartidaFelicitacionLeyenda + "(" + ganador + "): ";
        }


        private void AsignarJugadores()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoTableroJuego = new InstanceContext(this);
                ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient();

                var resumenFamilias = proxy.ObtenerResumenFamiliasPorSala(identificadorSala);
                var rutaImagenesPorFamilia = new Dictionary<string, string>
                {
                    { "Ores", "Imagenes/EscudoOres.jpg" },
                    { "Corbat", "Imagenes/EscudoCorbat.jpg" },
                    { "Garlo", "Imagenes/EscudoGarlo.jpg" },
                    { "Ramfez", "Imagenes/EscudoRamfez.jpg" }
                };

                var labels = new[] { lblJugador1, lblJugador2, lblJugador3, lblJugador4 };
                var images = new[] { imgFamilia1, imgFamilia2, imgFamilia3, imgFamilia4 };

                int index = 0;
                foreach (var jugador in resumenFamilias.OrderBy(j => j.Value.Item2))
                {
                    if (index >= labels.Length) break;

                    string nombreJugador = jugador.Key;
                    string familia = jugador.Value.Item1;
                    int vidaTotal = jugador.Value.Item2;

                    labels[index].Content = Properties.Resources.palabraJugador + ": " + nombreJugador + ", " + Properties.Resources.palabraFamilia +": "+ familia + ", " + Properties.Resources.palabraVidaTotal +": " + vidaTotal;
                    if (rutaImagenesPorFamilia.TryGetValue(familia, out var rutaImagen))
                    {
                        images[index].Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                    }

                    index++;
                }
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


        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            
                Inicio nuevaVentana = new Inicio(jugadorPropietario);
                nuevaVentana.Show();
                this.Close();
            
        }


        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }
    }
}
    
