using ClienteGloomApp.ServicioGloom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para FinPartidaMini.xaml
    /// </summary>
    public partial class FinPartidaMini : Window
    {
        string jugadorPropietario;
        string tipoJugador;
        string identificadorSala;
        public FinPartidaMini(string nombreUsuario, string ganador, string idSala)
        {
            InitializeComponent();
            jugadorPropietario = nombreUsuario;
            identificadorSala = idSala;
            AsignarJugadores();
            lblDialogoGanador.Content="(" + ganador + "): "+ Properties.Resources.finMiniComentarioGanador;
        }

        private void AsignarJugadores()
        {
            InstanceContext contextoPartida= new InstanceContext(this);
            ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient();
            var personajesPorUsuario = proxy.ObtenerUsuariosYPersonajes(identificadorSala);

            var rutaImagenesPorPersonaje = new Dictionary<string, string>
            {
                { "Tucani", "ImagenesFamilia/Tucani.png" },
                { "Luan", "ImagenesFamilia/Luan.jpg" },
                { "Lusiel", "ImagenesFamilia/Lusiel.jpg" },
                { "Angelus", "ImagenesFamilia/Angelus.png" }
            };

            if (personajesPorUsuario.TryGetValue(jugadorPropietario, out var personajeUsuario))
            {
                var (nombrePersonaje, vida) = personajeUsuario;

                if (rutaImagenesPorPersonaje.TryGetValue(nombrePersonaje, out var rutaImagen))
                {
                    lblJugador1.Content = lblJugador1.Content = personajeUsuario.Item2.ToString() + " " + Properties.Resources.finMiniIntruccionPuntos;
                    imgJugador1.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                }

                personajesPorUsuario.Remove(jugadorPropietario);
            }

            var labels = new List<Label> { lblJugador2, lblJugador3, lblJugador4 };
            var images = new List<Image> { imgJugador2, imgJugador3, imgJugador4 };

            int i = 0;
            foreach (var usuario in personajesPorUsuario)
            {
                if (i >= labels.Count) break;

                var (nombrePersonaje, vida) = usuario.Value;

                if (rutaImagenesPorPersonaje.TryGetValue(nombrePersonaje, out var rutaImagen))
                {
                    labels[i].Content = vida.ToString() + " " + Properties.Resources.finMiniIntruccionPuntos;
                    images[i].Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                    i++;
                }
            }
        }
        private void ValidarTipoJugador(string nombre)
        {
            string patron = @"^Invitado\d+$";

            if (Regex.IsMatch(nombre, patron))
            {
                tipoJugador = "Invitado";
            }
            else
            {
                tipoJugador = "Registrado";
            }
        }

        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
             Inicio nuevaVentana = new Inicio(jugadorPropietario);
             nuevaVentana.Show();
             this.Close();
        

        }
    }
}
