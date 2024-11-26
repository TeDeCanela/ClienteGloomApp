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
        List<Carta> mazoDeJugador = new List<Carta>();
        Carta cartaSeleccionada = new Carta();
        String jugadorPropietario;

        public PartidaMiniJuego(string nombreUsuario, int cantidadJugadores, string numeroSala)     
        {
            InitializeComponent();
            lblJugador1.Content = nombreUsuario;
            jugadorPropietario = nombreUsuario;
            lblNumeroSala.Content = numeroSala;
            InstanceContext contextoTableroJuego = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTableroJuego);
            try
            {
                proxy.IniciarPartidaPorAdministrador(jugadorPropietario, numeroSala, cantidadJugadores);
                AsignarJugadores();
                PonerImagenCarta(nombreUsuario);

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);

            }
            

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

        public void EnviarTurno(string nombreDelUsuarioEnTurno)
        {
            if (!nombreDelUsuarioEnTurno.Equals(jugadorPropietario))
            {
                btnDescartar.IsEnabled = false;
                btnUsar.IsEnabled = false;
                btnMazoCartas.IsEnabled = false;
            }
            else
            {
                btnDescartar.IsEnabled = true;
                btnUsar.IsEnabled = true;
                btnMazoCartas.IsEnabled = true;
            }
        }

        private void AsignarJugadores()
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            var personajesPorUsuario = proxy.ObtenerUsuariosYPersonajesSala();

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
                    lblJugador1.Content = jugadorPropietario;
                    imgJugador1.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                }

                personajesPorUsuario.Remove(jugadorPropietario);
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
                    cartaSeleccionada = mazoDeJugador[indice];

                    if (botonSeleccionado.Background is ImageBrush rutaImagen && rutaImagen.ImageSource is BitmapImage rutaImagenOriginal)
                    {
                        imgCarta.Source = rutaImagenOriginal;
                    }
                }
            }
        }

        private void BtnMazoCartas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient(contextoCarta);
                //proxy.AgregarCartaAMazoJugador(jugadorPropietario);
                PonerImagenCarta(jugadorPropietario);
                ActualizarTurno();
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
            
        }

        private void BtnDescartar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient(contextoCarta);
                proxy.QuitarCartaDeMazoJugador(jugadorPropietario, cartaSeleccionada);
                PonerImagenCarta(jugadorPropietario);
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
            PonerImagenCarta(jugadorPropietario);
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

            }else if (tipo.Equals("muerte"))
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

        public void EnviarGanador(string jugador)
        {
            Console.WriteLine(jugador);
        }

        private void BtnChat_Click(object sender, RoutedEventArgs e)
        {
            // Abre la ventana de chat con el nombre de usuario actual y el número de sala
            //var chatWindow = new Chat(lblJugador1.Content.ToString(), lblNumeroSala.Content.ToString());
            //chatWindow.Show();
        }


        void ISalaCallback.ActualizarSalasActivas(Sala[] salasActivas)
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ResultadoUnirseASala(string idSala, string codigo, bool esExitoso)
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarSeleccionFamilia(string nombreUsuario, string nombreFamilia)
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.ActualizarTurno(string nombreDelUsuarioEnTurno)
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.ActualizarImagenMazoCartaSobrante()
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.ActualizarImagenMazoCartaBonus()
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.ActualizarMazoJugador()
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.NotificarVotacionExpulsion(string jugadorPropuesto)
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.NotificarResultadoExpulsion(string jugadorExpulsado, bool expulsado)
        {
            throw new NotImplementedException();
        }
    }
}
