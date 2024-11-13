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
    public partial class PartidaMiniJuego : Window, IServicioJuegoTableroCallback, ISalaCallback
    {
        List<Carta> mazoDeJugador;
        public PartidaMiniJuego(string nombreUsuario, int cantidadJugadores, string numeroSala)     
        {
            InitializeComponent();
            lblJugador1.Content = nombreUsuario;
            InstanceContext contextoTableroJuego = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTableroJuego);

            proxy.IniciarPartidaPorAdministrador(lblJugador1.Content.ToString(), numeroSala, cantidadJugadores);
            AsignarJugadores();
            PonerImagenCarta(nombreUsuario);

        }

        public void ActualizarImagenPersonaje(string personaje, string personajeAnterior)
        {
            throw new NotImplementedException();
        }

        public void ActualizarNumeroJugadores()
        {
            throw new NotImplementedException();
        }

        public void EmpezarJuego()
        {
            throw new NotImplementedException();
        }

        public void EnviarTurno(string nombreDelUsusarioEnTurno)
        {
            Console.WriteLine(nombreDelUsusarioEnTurno);
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

            if (personajesPorUsuario.TryGetValue(lblJugador1.Content.ToString(), out var personajeUsuario))
            {
                var (nombrePersonaje, vida) = personajeUsuario;

                if (rutaImagenesPorPersonaje.TryGetValue(nombrePersonaje, out var rutaImagen))
                {
                    lblJugador1.Content = lblJugador1.Content.ToString();
                    imgJugador1.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                }

                personajesPorUsuario.Remove(lblJugador1.Content.ToString());
            }
            int i = 1;
            foreach (var usuario in personajesPorUsuario)
            {
                var (nombrePersonaje, vida) = usuario.Value;

                if (rutaImagenesPorPersonaje.TryGetValue(nombrePersonaje, out var rutaImagen))
                {
                    switch (i)
                    {
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

        private void PonerImagenCarta(string nombreUsuario)
        {
            InstanceContext contextoCarta = new InstanceContext(this);
            ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient(contextoCarta);
            mazoDeJugador = proxy.ObtenerMazoJugador(nombreUsuario).ToList();

            Dictionary<int, Button> botonesCartas = new Dictionary<int, Button>
                {
                    { 0, btnCarta1 },
                    { 1, btnCarta2 },
                    { 2, btnCarta3 },
                    { 3, btnCarta4 },
                    { 4, btnCarta5 },
                    { 5, btnCarta6 },
                    { 6, btnCarta7 }
                };
            for (int i = 0; i < mazoDeJugador.Count && i < botonesCartas.Count; i++)
            {
                var carta = mazoDeJugador[i];
                string identificador = carta.identificador;
                botonesCartas[i].Tag = i;

                if (RutasDeCartas.RutasImagenesPorIdentificador.TryGetValue(identificador, out var rutaImagen))
                {
                    botonesCartas[i].Background = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute)));
                }
            }
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            panCarta.Visibility = Visibility.Collapsed;
        }

        private void BtnVerCarta_Click(object sender, RoutedEventArgs e)
        {
            panCarta.Visibility = Visibility.Visible;
            Button botonSeleccionado = sender as Button;

            if (botonSeleccionado != null && botonSeleccionado.Tag != null)
            {
                int indice = int.Parse(botonSeleccionado.Tag.ToString());

                if (indice >= 0 && indice < mazoDeJugador.Count)
                {
                    Carta cartaSeleccionada = mazoDeJugador[indice];

                    if (botonSeleccionado.Background is ImageBrush rutaImagen && rutaImagen.ImageSource is BitmapImage rutaImagenOriginal)
                    {
                        imgCarta.Source = rutaImagenOriginal;
                    }
                }
            }
        }

       // private void 
    }
}
