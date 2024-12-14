using ClienteGloomApp.ServicioGloom;
using ServicioGlomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
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

        List<Carta> mazoDeJugador = new List<Carta>();
        Carta cartaSeleccionada = new Carta();
        String jugadorPropietario;
        Carta cartaBonusSeleccionada = new Carta();
        string jugadorSeleccionadoParaCastigo="sin jugador";
        string jugadorSeleccionadoParaModificar = "sin jugador";
        string jugadorSeleciconadoParaMatar="sin jugador";
        bool jugadorSeleccionado=false;
        

        private InstanceContext contextoJugador;
        private ServicioGloom.ServicioJuegoTableroClient proxyJugador;


        public PartidaMiniJuego(string nombreUsuario, int cantidadJugadores, string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InitializeComponent();
            lblJugador1.Content = nombreUsuario;
            jugadorPropietario = nombreUsuario;
            lblNumeroSala.Content = numeroSala;
            contextoJugador = new InstanceContext(this);
            proxyJugador = new ServicioGloom.ServicioJuegoTableroClient(contextoJugador);
            InstanceContext contextoTableroJuego = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTableroJuego);

            decCarta.Visibility = Visibility.Visible;
            decCartaBonus.Visibility = Visibility.Visible;
            btnUsar.IsEnabled = false;
            btnDescartar.IsEnabled = false;
            decCartaBonusFinal.Visibility = Visibility.Collapsed;
            decCartaFinal.Visibility = Visibility.Collapsed;
            try
            {
                proxy.IniciarPartidaPorAdministrador(jugadorPropietario, numeroSala, cantidadJugadores);
                proxy.ConectarConTablero(jugadorPropietario, numeroSala);

                AsignarJugadores();
                PonerImagenCarta(nombreUsuario);
                ObtenerPirmerTurno();

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }
        private void ObtenerPirmerTurno()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoTableroJuego = new InstanceContext(this);
                ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTableroJuego);
                string nombreDelUsuarioEnTurno = proxy.AsignarPrimerTurno(lblNumeroSala.Content.ToString());

                if (nombreDelUsuarioEnTurno.Equals(jugadorPropietario))
                {
                    btnDescartar.IsEnabled = true;
                    btnUsar.IsEnabled = true;
                    decCarta.Visibility = Visibility.Collapsed;
                    decCartaBonus.Visibility = Visibility.Collapsed;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }
        public void EnviarTurno(string nombreDelUsuarioEnTurno)
        {
            btnDescartar.IsEnabled = nombreDelUsuarioEnTurno.Equals(jugadorPropietario);
            btnUsar.IsEnabled = nombreDelUsuarioEnTurno.Equals(jugadorPropietario);
            btnMazoCartas.IsEnabled = nombreDelUsuarioEnTurno.Equals(jugadorPropietario);
        }

        private void AsignarJugadores()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
                {
                    InstanceContext contextoTablero = new InstanceContext(this);
                    ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient();
                    var personajesPorUsuario = proxy.ObtenerUsuariosYPersonajes(lblNumeroSala.Content.ToString());
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
                            imgJugador1.Tag = jugadorPropietario;
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
                                imgJugador2.Tag = usuario.Key;
                                break;
                            case 2:
                                lblJugador3.Content = usuario.Key;
                                imgJugador3.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                imgJugador3.Tag = usuario.Key;
                                break;
                            case 3:
                                lblJugador4.Content = usuario.Key;
                                imgJugador4.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                                imgJugador4.Tag = usuario.Key;
                                break;
                        }
                    }
                    i++;
                    if (i >= 4) break;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }

        private void PonerImagenCarta(string nombreUsuario)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient();
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
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
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
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient();
                proxy.AgregarCartaAMazoJugador(jugadorPropietario);
                PonerImagenCarta(jugadorPropietario);
                DeshabilitaCampos();
                ActualizarTurnoLlamar();

            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }

        private void BtnDescartar_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                panCarta.Visibility = Visibility.Collapsed;
                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient();
                proxy.QuitarCartaDeMazoJugador(jugadorPropietario, cartaSeleccionada);
                PonerImagenCarta(jugadorPropietario);
                cartaSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
                ActualizarTurnoLlamar();
                DeshabilitaCampos();
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }

        private void BtnUsar_Click(object sender, RoutedEventArgs e)
        {
            panCarta.Visibility = Visibility.Collapsed;
            ValidarTipoCarta(cartaSeleccionada.tipo);
            PonerImagenCarta(jugadorPropietario);
            DeshabilitaCampos();
            ActualizarTurnoLlamar();
        }

        private void BtnCartaBonus_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            panCartaBonus.Visibility = Visibility.Visible;
            InstanceContext contextoCarta = new InstanceContext(this);
            ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient();
            try
            {
                Carta carta = proxy.ObtenerCartasBonus();
                cartaBonusSeleccionada = carta;
                string identificador = carta.identificador;

                if (RutasDeCartas.RutasImagenesCartaBonus.TryGetValue(identificador, out var rutaImagen))
                {
                    imgCartaBonus.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                }
                PonerInformacionCartaBonus(carta.tipo);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }

        private void ActualizarTurnoLlamar()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                proxyJugador.CambiarTurno(lblNumeroSala.Content.ToString());
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }

        private void ValidarTipoCarta(string tipo)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InstanceContext contextoTablero = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);
            InstanceContext contextoCarta = new InstanceContext(this);
            ServicioGloom.ServicioCartaClient proxyCarta = new ServicioGloom.ServicioCartaClient();
            try
            {
                if (tipo.Equals("modificador"))
                {
                    SeleciconarJugadorParaModificar();
                    proxyCarta.QuitarCartaDeMazoJugador(jugadorPropietario, cartaSeleccionada);
                    decCarta.Visibility = Visibility.Visible;
                    decCartaBonus.Visibility = Visibility.Visible;
                }
                else if (tipo.Equals("muerte"))
                {

                    SeleccionarJugadorParaMatar();
                    proxyCarta.QuitarCartaDeMazoJugador(jugadorPropietario, cartaSeleccionada);

                    decCarta.Visibility = Visibility.Visible;
                    decCartaBonus.Visibility = Visibility.Visible;


                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }

        public void EnviarGanador(string jugador)
        {
            FinPartidaMini nuevaVentana = new FinPartidaMini(jugadorPropietario, jugador, lblNumeroSala.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }

        public void ActualizarTurno(string nombreDelUsuarioEnTurno)
        {
            if (nombreDelUsuarioEnTurno.Equals(lblJugador1.Content.ToString()))
            {
                btnDescartar.IsEnabled = true;
                btnUsar.IsEnabled = true;
                decCarta.Visibility = Visibility.Collapsed;
                decCartaBonus.Visibility = Visibility.Collapsed;
            }
        }

        public void DeshabilitaCampos()
        {
            btnDescartar.IsEnabled = false;
            btnUsar.IsEnabled = false;
            decCarta.Visibility = Visibility.Visible;
            decCartaBonus.Visibility = Visibility.Visible;
        }

        public void ActualizarImagenMazoCartaSobrante()
        {
            btnMazoCartas.Visibility = Visibility.Collapsed;
            decCartaFinal.Visibility = Visibility.Visible;
        }

        public void ActualizarImagenMazoCartaBonus()
        {
            btnCartaBonus.Visibility = Visibility.Collapsed;
            decCartaBonusFinal.Visibility= Visibility.Visible;
        }

        private void PonerInformacionCartaBonus(string tipo)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoTablero = new InstanceContext(this);
                ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient();
                var personajesPorUsuario = proxy.ObtenerUsuariosYPersonajes(lblNumeroSala.Content.ToString());
                int botonIndex = 0;
                var botonesJugadores = new List<Button> { btnJugador2, btnJugador3, btnJugador4 };

                switch (tipo)
                {
                    case "saltarJugador":
                        lblCarta.Content = Properties.Resources.cartaSaltarJugador;
                        foreach (var usuario in personajesPorUsuario.Keys)
                        {
                            if (usuario != jugadorPropietario && botonIndex < botonesJugadores.Count)
                            {
                                botonesJugadores[botonIndex].Content = usuario;
                                botonesJugadores[botonIndex].Visibility = Visibility.Visible;
                                botonIndex++;
                            }
                        }
                        break;

                    case "robar2Cartas":
                        lblCarta.Content = Properties.Resources.cartaRobarCarta2;
                        break;

                    case "robar1Cartas":
                        lblCarta.Content = Properties.Resources.cartaRobarCarta1;
                        break;

                    case "QuitarCarta":
                        lblCarta.Content = Properties.Resources.cartaQuitarCarta;
                        foreach (var usuario in personajesPorUsuario.Keys)
                        {
                            if (usuario != jugadorPropietario && botonIndex < botonesJugadores.Count)
                            {
                                botonesJugadores[botonIndex].Content = usuario;
                                botonesJugadores[botonIndex].Visibility = Visibility.Visible;
                                botonIndex++;
                            }
                        }
                        break;

                    case "PerderTurno":
                        lblCarta.Content = Properties.Resources.cartaPerderTurno;
                        break;
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }

        private void CerrarCartaBonus_Click(object sender, RoutedEventArgs e)
        {
            panCartaBonus.Visibility = Visibility.Collapsed;
            AplicarCartaBonus();
            cartaBonusSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
            ActualizarTurnoLlamar();
            DeshabilitaCampos();
            jugadorSeleccionadoParaCastigo = "";
            btnJugador2.BorderThickness = new Thickness(0);
            btnJugador3.BorderThickness = new Thickness(0);
            btnJugador4.BorderThickness = new Thickness(0);
            btnJugador2.Visibility = Visibility.Collapsed;
            btnJugador3.Visibility = Visibility.Collapsed;
            btnJugador4.Visibility = Visibility.Collapsed;
        }

        private void CambiarJugador_Click(object sender, RoutedEventArgs e)
        {
            Button botonSeleccionado = sender as Button;
            jugadorSeleccionadoParaCastigo = botonSeleccionado.Content.ToString();
            btnJugador2.BorderThickness = new Thickness(0);
            btnJugador3.BorderThickness = new Thickness(0);
            btnJugador4.BorderThickness = new Thickness(0);
            botonSeleccionado.BorderBrush = new SolidColorBrush(Colors.Red);
            botonSeleccionado.BorderThickness = new Thickness(2);
        }

        private void AplicarCartaBonus()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InstanceContext contextoTablero = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);
            InstanceContext contextoCarta = new InstanceContext(this);
            ServicioGloom.ServicioCartaClient proxyCarta = new ServicioGloom.ServicioCartaClient();
            try
            {
                switch (cartaBonusSeleccionada.tipo)
                {
                    case "saltarJugador":
                        ValidarSeleccionJugadorParaCastigo();
                        proxy.AgregarCastigo(jugadorSeleccionadoParaCastigo);
                        jugadorSeleccionadoParaCastigo = "sin jugador";
                        break;

                    case "robar2Cartas":
                        proxyCarta.AgregarCartaAMazoJugador(jugadorPropietario);
                        proxyCarta.AgregarCartaAMazoJugador(jugadorPropietario);
                        PonerImagenCarta(jugadorPropietario); lblCarta.Content = Properties.Resources.cartaRobarCarta2;
                        break;

                    case "robar1Cartas":
                        proxyCarta.AgregarCartaAMazoJugador(jugadorPropietario);
                        PonerImagenCarta(jugadorPropietario);
                        break;

                    case "QuitarCarta":
                        ValidarSeleccionJugadorParaCastigo();
                        proxyCarta.QuitarCartaDeMazoJugadorExterno(jugadorSeleccionadoParaCastigo);
                        jugadorSeleccionadoParaCastigo = "sin jugador";
                        break;
                    case "PerderTurno":
                        proxy.AgregarCastigo(jugadorPropietario);

                        break;
                }
            }
            catch (InvalidOperationException ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }

        public void ActualizarMazoJugador()
        {
            PonerImagenCarta(jugadorPropietario);
            jugadorSeleccionadoParaCastigo = "sin jugador";
        }

        private void ValidarSeleccionJugadorParaCastigo()
        {
            if (jugadorSeleccionadoParaCastigo.Equals("sin jugador"))
            {
                throw new InvalidOperationException("40");
            }
        }

        private async void SeleciconarJugadorParaModificar()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            decCarta.Visibility = Visibility.Visible;
            decCartaBonus.Visibility = Visibility.Visible;
            CambiarColorContornoJugadores();
            try
            {
                int tiempoEspera = 5000;
                if (await EsperarSeleccionJugador(tiempoEspera))
                {
                    InstanceContext contextoTablero = new InstanceContext(this);
                    ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);
                    proxy.SumarVidaPersonaje(lblNumeroSala.Content.ToString(), jugadorSeleccionadoParaModificar, cartaSeleccionada.valor);
                    jugadorSeleccionadoParaModificar = "sin jugador";
                    ResetearBordes();
                    cartaSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
                }
                else
                {
                    ResetearBordes();
                    MessageBox.Show(Properties.Resources.mensajeMiniPartidaTiempoAgotado);
                }
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }

        }

        private async void SeleccionarJugadorParaMatar()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            decCarta.Visibility = Visibility.Visible;
            decCartaBonus.Visibility = Visibility.Visible;
            CambiarColorContornoJugadores();
            try
            {
                int tiempoEspera = 5000;
                if (await EsperarSeleccionJugador(tiempoEspera))
                {
                    InstanceContext contextoTablero = new InstanceContext(this);
                    ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);
                    proxy.MatarJugador(lblNumeroSala.Content.ToString(), jugadorSeleccionadoParaModificar, jugadorPropietario);
                    jugadorSeleccionadoParaModificar = "sin jugador";
                    ResetearBordes();
                    cartaSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
                }
                else
                {
                    ResetearBordes();
                    MessageBox.Show(Properties.Resources.mensajeMiniPartidaTiempoAgotado);
                }
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }

        }

        private void CambiarColorContornoJugadores()
        {
            var jugadores = new List<(Border border, Label lbl)>
            {
                (borderJugador1, lblJugador1),
                (borderJugador2, lblJugador2),
                (borderJugador3, lblJugador3),
                (borderJugador4, lblJugador4)
            };

            ResetearBordes();

            foreach (var (border, lbl) in jugadores)
            {
                if (!string.IsNullOrEmpty(lbl.Content?.ToString()))
                {
                    border.BorderBrush = System.Windows.Media.Brushes.Red;
                    border.BorderThickness = new Thickness(3);
                }
            }
        }

        private void ResetearBordes()
        {
            var jugadores = new List<(Border border, Label lbl)>
            {
                (borderJugador1, lblJugador1),
                (borderJugador2, lblJugador2),
                (borderJugador3, lblJugador3),
                (borderJugador4, lblJugador4)
            };

            foreach (var (border, lbl) in jugadores)
            {
                border.BorderBrush = System.Windows.Media.Brushes.Transparent;
                border.BorderThickness = new Thickness(0);
            }
        }

        private void ImgJugador_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image imagenSeleccionada)
            {
                string informacion = imagenSeleccionada.Tag?.ToString();
                jugadorSeleccionadoParaModificar = informacion;
            }
            jugadorSeleccionado = true;
        }

        private async Task<bool> EsperarSeleccionJugador(int tiempoEsperaMilisegundos)
        {
            jugadorSeleccionado = false;

            using (var cancelarToken = new CancellationTokenSource())
            {
                var tarea = Task.Delay(tiempoEsperaMilisegundos, cancelarToken.Token);

                while (!jugadorSeleccionado)
                {
                    if (tarea.IsCompleted)
                    {
                        break;
                    }

                    await Task.Delay(100);
                }

                if (jugadorSeleccionado)
                {
                    cancelarToken.Cancel();
                }

                return jugadorSeleccionado;
            }
        }

        public void ActualizarJugadorMuerto(string jugadorMuerto)
        {
            if (jugadorMuerto.Equals(jugadorPropietario))
            {
                decCartaBonus.Visibility = Visibility.Visible;
                decCarta.Visibility = Visibility.Visible;
                btnUsar.IsEnabled = false;
                btnDescartar.IsEnabled = false;
            }
        }

        public void RecibirExpulsion(string jugadorObjetivo)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                MessageBox.Show(Properties.Resources.mensajeHaSidoExpulsado, jugadorObjetivo, MessageBoxButton.OK, MessageBoxImage.Warning);

                Inicio ventanaInicio = new Inicio(lblJugador1.Content.ToString());
                ventanaInicio.Show();
                this.Close();
            });
        }

        public void ActualizarInterfazExpulsion(string jugadorExpulsado)
        {
            var jugadores = new[]
    {
        (Label: lblJugador1, Image: imgJugador1),
        (Label: lblJugador2, Image: imgJugador2),
        (Label: lblJugador3, Image: imgJugador3),
        (Label: lblJugador4, Image: imgJugador4)
    };

            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var jugador in jugadores)
                {
                    if (jugador.Label.Content?.ToString() == jugadorExpulsado)
                    {
                        jugador.Label.Content = string.Empty;

                        if (jugador.Image != null)
                        {
                            jugador.Image.Source = null;
                            jugador.Image.Tag = null;
                        }

                        break;
                    }
                }
            });
        }

        public void NotificarVotacionExpulsion(string jugadorPropuesto)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ActualizarInterfazExpulsion(jugadorPropuesto);
                MessageBox.Show(jugadorPropuesto + " " + Properties.Resources.mensajeHaSidoExpulsado, Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void BtnChat_Click(object sender, RoutedEventArgs e)
        {

            Chat ventanaChat = Chat.ObtenerInstancia(lblJugador1.Content.ToString());
            ventanaChat.Show();
            ventanaChat.Focus();

        }



        private void BtnIniciarVotacion_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoSala);

            try
            {
                var jugadores = proxy.ObtenerJugadoresPartida(lblNumeroSala.Content.ToString())
                                     .Where(j => j != lblJugador1.Content.ToString())
                                     .ToList();


                if (jugadores.Count == 0)
                {
                    MessageBox.Show(Properties.Resources.mensajeNoJugadoresParaExpulsar, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                lbxJugadoresParaExpulsion.ItemsSource = jugadores;
                panExpulsion.Visibility = Visibility.Visible;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
            }
        }


        private void BtnConfirmarExpulsion_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (lbxJugadoresParaExpulsion.SelectedItem == null)
            {
                MessageBox.Show(Properties.Resources.mensajeSeleccionJugador, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string jugadorSeleccionado = lbxJugadoresParaExpulsion.SelectedItem.ToString();

            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoSala);

                proxy.SolicitarExpulsion(lblJugador1.Content.ToString(), jugadorSeleccionado, lblNumeroSala.Content.ToString());

                MessageBox.Show(Properties.Resources.mensajeExpuslion + jugadorSeleccionado, Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);

                panExpulsion.Visibility = Visibility.Collapsed;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
                administradorLogger.RegistroError(ex);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
                administradorLogger.RegistroError(ex);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
                administradorLogger.RegistroError(ex);
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
        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }
    }
}