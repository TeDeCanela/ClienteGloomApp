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
    /// Lógica de interacción para PartidaMiniJuego.xaml
    /// </summary>
    public partial class PartidaMiniJuego : Window, IServicioJuegoTableroCallback
    {
        public PartidaMiniJuego(string nombreUsuario, int cantidadJugadores, string numeroSala)     
        {
            InitializeComponent();
            lblJugador1.Content = nombreUsuario;
            InstanceContext contextoTableroJuego = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTableroJuego);

            proxy.IngresarJugadorAJuego(lblJugador1.Content.ToString(), numeroSala, cantidadJugadores);
            AsignarJugadores();

        }

        public void RecibirTurno(bool validarTurno)
        {
            throw new NotImplementedException();
        }

        private void AsignarJugadores()
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            var personajesPorUsuario = proxy.ObtenerUsuariosYPersonajes();

            var rutaImagenesPorPersonaje = new Dictionary<string, string>
            {
                { "Tucani", "ImagenesFamilia/Tucani.png" },
                { "Luan", "ImagenesFamilia/Luan.jpg" },
                { "Lusiel", "ImagenesFamilia/Lusiel.jpg" },
                { "Angelus", "ImagenesFamilia/Angelus.png" }
            };

            int i = 0;
            foreach (var usuario in personajesPorUsuario)
            {
                var (nombrePersonaje, vida) = usuario.Value;

                if (rutaImagenesPorPersonaje.TryGetValue(nombrePersonaje, out var rutaImagen))
                {
                    switch (i)
                    {
                        case 0:
                            lblJugador1.Content = usuario.Key;
                            imgJugador1.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                            break;
                        case 1:
                            lblJugador2.Content = usuario.Key;
                            imgJugador2.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                            break;
                        case 2:
                            lblJugador3.Content = usuario.Key;
                            imgJugador3.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                            break;
                        case 3:
                            lblJugador4.Content = usuario.Key;
                            imgJugador4.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                            break;
                    }
                }
                i++;
                if (i >= 4) break;
            }
        }
    }
}
