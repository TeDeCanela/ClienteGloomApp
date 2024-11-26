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
    /// Lógica de interacción para PartidaNormal.xaml
    /// </summary>
    public partial class PartidaNormal : Window
    {
        ServicioGloom.Sala salaNormal = new ServicioGloom.Sala();

        List<Carta> mazoDeJugador = new List<Carta>();
        Carta cartaSeleccionada = new Carta();
        private string identificadorSala;
        private string identificadorUsuario;
        public PartidaNormal(String nombreUsaurio, Sala sala)
        {
            InitializeComponent();
            lblJugador1.Content = nombreUsaurio;
            identificadorUsuario = nombreUsaurio;
            salaNormal = sala;
            AsignarCartasPorJugador();
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
            for (int i = 0; i < botonesCartas.Count; i++)
            {
                if (i < mazoDeJugador.Count)
                {
                    var carta = mazoDeJugador[i];
                    string identificador = carta.identificador;
                    botonesCartas[i].Tag = i;
                    botonesCartas[i].Visibility = Visibility.Visible;

                    if (RutasDeCartas.RutasImagenesPorIdentificador.TryGetValue(identificador, out var rutaImagen))
                    {
                        botonesCartas[i].Background = new ImageBrush(new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute)));
                    }
                }
                else
                {
                    botonesCartas[i].Visibility = Visibility.Collapsed;
                }
            }
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
                    cartaSeleccionada = mazoDeJugador[indice];

                    if (botonSeleccionado.Background is ImageBrush rutaImagen && rutaImagen.ImageSource is BitmapImage rutaImagenOriginal)
                    {
                        imgCarta.Source = rutaImagenOriginal;
                    }
                }
            }
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            panCarta.Visibility = Visibility.Collapsed;
        }

        private void BtnDescartar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient(contextoCarta);
                proxy.QuitarCartaDeMazoJugador(identificadorUsuario, cartaSeleccionada);
                PonerImagenCarta(identificadorUsuario);
                cartaSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
                ActualizarTurno();
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }


        }

        private void BtnUsar_Click(object sender, RoutedEventArgs e)
        {
            ValidarTipoCarta(cartaSeleccionada.tipo);
            PonerImagenCarta(identificadorUsuario);
            cartaSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
            ActualizarTurno();

        }

        private void ActualizarTurno()
        {
            InstanceContext contextoJugador = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxyJugador = new ServicioGloom.ServicioJuegoTableroClient(contextoJugador);
            //proxyJugador.CambiarTurno(lblNumeroSala.Content.ToString());
        }

        private void ValidarTipoCarta(string tipo)
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            if (tipo.Equals("modificador"))
            {
                //proxy.SumarVidaPersonaje(jugadorPropietario, cartaSeleccionada.valor);

            }
            else if (tipo.Equals("muerte"))
            {
                try
                {
                    //proxy.TerminarPartidaMiniJuego();
                }
                catch (FaultException<ManejadorExcepciones> ex)
                {
                    MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
                }

            }
        }

        private void BtnChat_Click(object sender, RoutedEventArgs e)
        {
            Chat ventanaChat = new Chat(lblJugador1.Content.ToString(), salaNormal.idSala);
            ventanaChat.Show();
        }

        private void btnIniciarVotacion_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoSala);
            /*var jugadores = proxy.ObtenerJugadoresConectados(salaNormal.idSala).Where(j => j != salaNormal.idAdministrador).ToList();

            if (jugadores.Count == 0)
            {
                MessageBox.Show("No hay jugadores disponibles para expulsar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            lbxJugadoresParaExpulsion.ItemsSource = jugadores;

            panExpulsion.Visibility = Visibility.Visible;*/
        }

        private void BtnConfirmarExpulsion_Click(object sender, RoutedEventArgs e)
        {
            if (lbxJugadoresParaExpulsion.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un jugador para continuar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string jugadorSeleccionado = lbxJugadoresParaExpulsion.SelectedItem.ToString();

            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoSala);
               // proxy.SolicitarExpulsion(identificadorUsuario, jugadorSeleccionado, identificadorSala);
                MessageBox.Show($"Se ha iniciado la votación para expulsar a {jugadorSeleccionado}.", "Votación iniciada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MessageBox.Show(ex.Detail.mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                panExpulsion.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnCancelarExpulsion_Click(object sender, RoutedEventArgs e)
        {
            panExpulsion.Visibility = Visibility.Collapsed;
        }

        private void AsignarCartasPorJugador()
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

            // Obtener la relación jugador-familia desde el servicio
            var familiaPorJugador = proxy.ObtenerFamiliaPorJugador();
            var familias = proxy.ObtenerFamiliasYPersonajes();

            // Diccionario para las rutas de imágenes de las cartas
            var rutaImagenesPorPersonaje = new Dictionary<string, string>
            {
                { "Didorian", "ImagenesFamilia/Didorian.png" },
                { "Zael", "ImagenesFamilia/Zael.jpg" },
                { "Pablian", "ImagenesFamilia/Pablian.jpg" },
                { "Lorenzeo", "ImagenesFamilia/Lorenzeo.png" },
                { "Gaia", "ImagenesFamilia/Gaia.png" },
                { "Arialyn", "ImagenesFamilia/Arialyn.jpg" },
                { "Aris", "ImagenesFamilia/Aris.jpg" },
                { "Abelith", "ImagenesFamilia/Abelith.png" },
                { "Tucani", "ImagenesFamilia/Tucani.png" },
                { "Luan", "ImagenesFamilia/Luan.jpg" },
                { "Lusiel", "ImagenesFamilia/Lusiel.jpg" },
                { "Angelus", "ImagenesFamilia/Angelus.png" },
                { "Seti", "ImagenesFamilia/Seti.png" },
                { "Merit", "ImagenesFamilia/Merit.jpg" },
                { "Neferu", "ImagenesFamilia/Neferu.jpg" },
                { "Sobek", "ImagenesFamilia/Sobek.png" }
            };

            if (familiaPorJugador.TryGetValue(identificadorUsuario, out var familiaDeUsuario))
            {
                if (familias.TryGetValue(familiaDeUsuario, out var personajes))
                {
                    lblJugador1.Content = identificadorUsuario;
                    for (int j = 0; j < personajes.Count(); j++)
                    {
                        var (nombrePersonaje, _) = personajes[j];

                        if (rutaImagenesPorPersonaje.TryGetValue(nombrePersonaje, out var rutaImagen))
                        {
                            imgPersonaje1.Tag = familias;
                            imgPersonaje1.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                            imgPersonaje2.Tag = familias;
                            imgPersonaje2.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                            imgPersonaje3.Tag = familias;
                            imgPersonaje3.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                            imgPersonaje4.Tag = familias;
                            imgPersonaje4.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));

                        }

                    }
                }
            }

              

            // Asignar las cartas a los jugadores
            int i = 1;
            foreach (var jugador in familiaPorJugador)
            {
                var nombreJugador = jugador.Key;
                var familia = jugador.Value;

                if (familias.TryGetValue(familia, out var personajes))
                {
                    foreach ((string nombrePersonaje, int vida) in personajes)
                    {
                        if (rutaImagenesPorPersonaje.TryGetValue(nombrePersonaje, out var rutaImagen))
                        {
                            switch (i)
                            {
                                case 1:
                                    lblJugador1.Content = personajes;
                                    imgPersonaje1.Tag = familias;
                                    imgPersonaje1.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje2.Tag = familias;
                                    imgPersonaje2.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje3.Tag = familias;
                                    imgPersonaje3.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje4.Tag = familias;
                                    imgPersonaje4.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    break;
                                case 2:
                                    lblJugador2.Content = personajes;
                                    imgPersonaje5.Tag = familias;
                                    imgPersonaje5.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje6.Tag = familias;
                                    imgPersonaje6.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje7.Tag = familias;
                                    imgPersonaje7.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje8.Tag = familias;
                                    imgPersonaje8.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    break;
                                case 3:
                                    lblJugador2.Content = personajes;
                                    imgPersonaje9.Tag = familias;
                                    imgPersonaje9.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje10.Tag = familias;
                                    imgPersonaje10.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje11.Tag = familias;
                                    imgPersonaje11.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje12.Tag = familias;
                                    imgPersonaje12.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    break;
                                case 4:
                                    lblJugador2.Content = personajes;
                                    imgPersonaje13.Tag = familias;
                                    imgPersonaje13.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje14.Tag = familias;
                                    imgPersonaje14.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje15.Tag = familias;
                                    imgPersonaje15.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    imgPersonaje16.Tag = familias;
                                    imgPersonaje16.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                    break;
                            }
                        }
                    }
                }
                i++;
                if (i >= 4) break; // Limitar a 4 jugadores
            }
        }
        
    }
}
