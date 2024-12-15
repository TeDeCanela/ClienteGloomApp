using ClienteGloomApp.ServicioGloom;
using ServicioGlomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
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
    public partial class PartidaNormal : Window, IServicioJuegoTableroCallback
    {
        ServicioGloom.Sala salaNormal = new ServicioGloom.Sala();

        List<Carta> mazoDeJugador = new List<Carta>();
        Carta cartaSeleccionada = new Carta();
        Carta cartaBonusSeleccionada = new Carta();
        string jugadorSeleciconadoParaCastigo = "sin jugador";
        private string identificadorUsuario;

        private InstanceContext contextoJugador;
        private ServicioGloom.ServicioJuegoTableroClient proxyJugador;

        private string usuarioObjetivoSeleccionado;
        private string personajeObjetivoSeleccionado;
      
        public PartidaNormal(String nombreUsaurio, Sala sala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InitializeComponent();
            lblJugador1.Content = nombreUsaurio;
            identificadorUsuario = nombreUsaurio;
            lblNumeroSala.Content = sala.idSala;
            salaNormal = sala;

            contextoJugador = new InstanceContext(this);
            proxyJugador = new ServicioGloom.ServicioJuegoTableroClient(contextoJugador);
            InstanceContext contextoTableroJuego = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTableroJuego);



            try
            {
                

                proxy.IniciarPartidaPorAdministrador(identificadorUsuario, sala.idSala, sala.noJugadores);
                proxy.ConectarConTablero(identificadorUsuario, sala.idSala);

                AsignarCartasPorJugador();
                PonerImagenCarta(nombreUsaurio);
                ObtenerPrimerTurno();

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


        private void ObtenerPrimerTurno()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoTableroJuego = new InstanceContext(this);
                ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTableroJuego);
                string nombreDelUsuarioEnTurno = proxy.AsignarPrimerTurno(lblNumeroSala.Content.ToString());

                if (nombreDelUsuarioEnTurno.Equals(identificadorUsuario))
                {
                    btnDescartar.IsEnabled = true;
                    btnUsar.IsEnabled = true;
                    decCarta.Visibility = Visibility.Collapsed;
                    decCartaBonus.Visibility = Visibility.Collapsed;
                }
                else
                {
                    btnDescartar.IsEnabled = false;
                    btnUsar.IsEnabled = false;
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
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                panCarta.Visibility = Visibility.Collapsed;

                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient();

                proxy.QuitarCartaDeMazoJugador(identificadorUsuario, cartaSeleccionada);

                PonerImagenCarta(identificadorUsuario);

                cartaSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };

                ActualizarTurnoLlamar();
                DeshabilitaCampos();
                panCarta.Visibility = Visibility.Collapsed;
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

            if (string.IsNullOrEmpty(cartaSeleccionada.identificador))
            {
                MensajesEmergentes.MostrarMensaje("70", "Debes seleccionar una carta antes de usarla.");
                return;
            }


            MostrarSeleccionObjetivos(cartaSeleccionada);
            panCarta.Visibility = Visibility.Collapsed;
        }



        private void BtnMazoCartas_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                VerificarFinDePartida();
                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient();
                proxy.AgregarCartaAMazoJugador(identificadorUsuario);
                PonerImagenCarta(identificadorUsuario);
                DeshabilitaCampos();
                ActualizarTurnoLlamar();

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

        private void PonerInformacionCartaBonus(string tipo)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoPartida = new InstanceContext(this);
                ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient();
                var personajesPorUsuario = proxy.ObtenerUsuariosYPersonajes(lblNumeroSala.Content.ToString());
                InstanceContext contextoTablero = new InstanceContext(this);
                ServicioGloom.ServicioJuegoTableroClient proxyTablero = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);
                int botonIndex = 0;
                var botonesJugadores = new List<Button> { btnJugador2, btnJugador3, btnJugador4 };

                switch (tipo)
                {
                    case "saltarJugador":
                        lblCarta.Content = Properties.Resources.cartaSaltarJugador;
                        var listaJJugadores = proxyTablero.ObtenerJugadoresVivos(lblNumeroSala.Content.ToString());
                        foreach (var usuario in listaJJugadores)
                        {
                            if (usuario != identificadorUsuario && botonIndex < botonesJugadores.Count)
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
                        var listaJJugadoresQuitarCarta = proxyTablero.ObtenerJugadoresVivos(lblNumeroSala.Content.ToString());
                        foreach (var usuario in listaJJugadoresQuitarCarta)
                        {
                            if (usuario != identificadorUsuario && botonIndex < botonesJugadores.Count)
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
            DeshabilitaCampos();
            cartaBonusSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
            ActualizarTurnoLlamar();
            jugadorSeleciconadoParaCastigo   = "";
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
            jugadorSeleciconadoParaCastigo = botonSeleccionado.Content.ToString();
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
            
            try{
                switch (cartaBonusSeleccionada.tipo)
                {
                    case "saltarJugador":
                        ValidarSeleccionJugadorParaCastigo();
                        proxy.AgregarCastigo(jugadorSeleciconadoParaCastigo);
                        jugadorSeleciconadoParaCastigo = "sin jugador";
                        break;

                    case "robar2Cartas":
                        proxyCarta.AgregarCartaAMazoJugador(identificadorUsuario);
                        proxyCarta.AgregarCartaAMazoJugador(identificadorUsuario);
                        PonerImagenCarta(identificadorUsuario); lblCarta.Content = Properties.Resources.cartaRobarCarta2;
                        break;

                    case "robar1Cartas":
                        proxyCarta.AgregarCartaAMazoJugador(identificadorUsuario);
                        PonerImagenCarta(identificadorUsuario);
                        break;

                    case "QuitarCarta":
                        ValidarSeleccionJugadorParaCastigo();
                        proxyCarta.QuitarCartaDeMazoJugadorExterno(jugadorSeleciconadoParaCastigo);
                        jugadorSeleciconadoParaCastigo = "sin jugador";
                        break;
                    case "PerderTurno":
                        proxy.AgregarCastigo(identificadorUsuario);

                        break;
                }
            }
            catch (InvalidOperationException ex)
            {
                MensajesEmergentes.MostrarMensajeAdvertencia(ex.Message, ex.Message);
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

        private void ValidarSeleccionJugadorParaCastigo()
        {
            if (jugadorSeleciconadoParaCastigo.Equals("sin jugador"))
            {
                throw new InvalidOperationException("40");
            }
        }

        private void SeleccionarUsuarioObjetivo(string usuario)
        {
            usuarioObjetivoSeleccionado = usuario;
            CargarPersonajesDeUsuarioSeleccionado(usuario);
        }

        private void CargarPersonajesDeUsuarioSeleccionado(string usuario)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contexto = new InstanceContext(this);
                ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient();

                var personajes = proxy.ObtenerFamiliaYPersonajesPorUsuario(salaNormal.idSala);

                if (personajes.ContainsKey(usuario))
                {
                    panPersonajesObjetivo.Children.Clear();
                    foreach (var personaje in personajes[usuario].Item2)
                    {
                        Button btnPersonaje = new Button
                        {
                            Content = personaje.Item1,
                            Tag = personaje.Item1,
                            Margin = new Thickness(5),
                            Background = Brushes.LightGreen
                        };
                        btnPersonaje.Click += (s, e) => SeleccionarPersonajeObjetivo((string)((Button)s).Tag);
                        panPersonajesObjetivo.Children.Add(btnPersonaje);
                    }
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


        private void SeleccionarPersonajeObjetivo(string personaje)
        {
            personajeObjetivoSeleccionado = personaje;
        }


        private void ValidarTipoCarta(Carta cartaSeleccionada)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InstanceContext contextoTablero = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);

            if (cartaSeleccionada.tipo.Equals("modificador"))
            {
                if (cartaSeleccionada.valor > 0)
                {
                    if (string.IsNullOrEmpty(usuarioObjetivoSeleccionado) || string.IsNullOrEmpty(personajeObjetivoSeleccionado))
                    {
                        MensajesEmergentes.MostrarMensaje("71", "Selecciona un usuario y personaje objetivo antes de continuar.");
                        return;
                    }
                    proxy.AplicarModificadorPositivo(cartaSeleccionada, usuarioObjetivoSeleccionado, personajeObjetivoSeleccionado);
                }
                else
                {
                    if (string.IsNullOrEmpty(personajeObjetivoSeleccionado))
                    {
                        MensajesEmergentes.MostrarMensaje("72", "Selecciona un personaje objetivo antes de continuar.");
                        return;
                    }
                    proxy.AplicarModificadorNegativo(cartaSeleccionada, identificadorUsuario, personajeObjetivoSeleccionado);
                }
            }
            else if (cartaSeleccionada.tipo.Equals("muerte"))
            {
                if (string.IsNullOrEmpty(personajeObjetivoSeleccionado))
                {
                    MensajesEmergentes.MostrarMensaje("73", "Selecciona un personaje objetivo antes de continuar.");
                    return;
                }

                try
                {
                    proxy.AplicarCartaMuerte(salaNormal.idSala, identificadorUsuario, personajeObjetivoSeleccionado);
                    VerificarFinDePartida();
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
        }


        private void MostrarSeleccionObjetivos(Carta cartaSeleccionada)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                panCargando.Visibility = Visibility.Visible;
                InstanceContext contextoTablero = new InstanceContext(this);
                ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);

                var jugadoresEnSala = proxy.ObtenerJugadoresPartida(salaNormal.idSala);

                if (jugadoresEnSala == null || !jugadoresEnSala.Any())
                {
                    MensajesEmergentes.MostrarMensaje("74", "No hay jugadores conectados en la sala.");
                    return;
                }

                panSeleccionObjetivos.Visibility = Visibility.Visible;
                panUsuariosObjetivo.Children.Clear();
                panPersonajesObjetivo.Children.Clear();

                if (cartaSeleccionada.tipo.Equals("modificador"))
                {
                    if (cartaSeleccionada.valor > 0)
                    {
                        lblSeleccionUsuario.Content = Properties.Resources.partidaInstruccionSeleccionarUsuario;
                        lblSeleccionPersonaje.Content = Properties.Resources.partidaInstruccionSeleccionarPersonajeObjetivo;

                        foreach (var usuario in jugadoresEnSala.Where(j => j != identificadorUsuario))
                        {
                            Button btnUsuario = new Button
                            {
                                Content = usuario,
                                Tag = usuario,
                                Margin = new Thickness(5),
                                Background = Brushes.LightBlue
                            };
                            btnUsuario.Click += (s, e) =>
                            {
                                SeleccionarUsuarioObjetivo((string)((Button)s).Tag);
                                MostrarPersonajesUsuarioSeleccionado((string)((Button)s).Tag);
                            };
                            panUsuariosObjetivo.Children.Add(btnUsuario);
                        }
                    }
                    else
                    {
                        lblSeleccionUsuario.Content = Properties.Resources.partidaIntruccionSeleciconaPersonajeFamilia;
                        lblSeleccionPersonaje.Content = string.Empty;

                        MostrarPersonajesUsuarioSeleccionado(identificadorUsuario);
                    }
                }
                else if (cartaSeleccionada.tipo.Equals("muerte"))
                {
                    lblSeleccionUsuario.Content = Properties.Resources.partidaIntruccionSeleciconaPersonajeFamiliaMuerte;
                    lblSeleccionPersonaje.Content = string.Empty;
                    MostrarPersonajesUsuarioSeleccionado(identificadorUsuario);
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
            finally
            {

                panCargando.Visibility = Visibility.Collapsed;
            }
        }


        private void MostrarPersonajesUsuarioSeleccionado(string usuario)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {

                panPersonajesObjetivo.Children.Clear();

                InstanceContext contextoTableroJuego = new InstanceContext(this);
                ServicioGloom.CreacionPartidaClient proxyPartida = new ServicioGloom.CreacionPartidaClient();
                var personajes = proxyPartida.ObtenerFamiliaYPersonajesPorUsuario(salaNormal.idSala);

                if (personajes == null || !personajes.ContainsKey(usuario))
                {
                    MensajesEmergentes.MostrarMensaje("75", "No se encontraron personajes para el usuario seleccionado.");
                    return;
                }

                var personajesDelUsuario = personajes[usuario].Item2;

                foreach (var personaje in personajesDelUsuario)
                {
                    Button btnPersonaje = new Button
                    {
                        Content = personaje.Item1,
                        Tag = personaje.Item1,
                        Margin = new Thickness(5),
                        Background = Brushes.LightGreen
                    };
                    btnPersonaje.Click += (s, e) => SeleccionarPersonajeObjetivo((string)((Button)s).Tag);
                    panPersonajesObjetivo.Children.Add(btnPersonaje);
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




        private void BtnConfirmarSeleccion_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (string.IsNullOrEmpty(personajeObjetivoSeleccionado))
            {
                MensajesEmergentes.MostrarMensaje("76", "Debes seleccionar un personaje objetivo para continuar.");
                return;
            }

            if (cartaSeleccionada.tipo.Equals("modificador") && cartaSeleccionada.valor > 0 && string.IsNullOrEmpty(usuarioObjetivoSeleccionado))
            {
                MensajesEmergentes.MostrarMensaje("73", "Debes seleccionar un usuario y un personaje objetivo para continuar.");
                return;
            }

            try
            {
                ValidarTipoCarta(cartaSeleccionada);

                panSeleccionObjetivos.Visibility = Visibility.Collapsed;

                ActualizarTurnoLlamar();
                DeshabilitaCampos();

                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient();

                proxy.QuitarCartaDeMazoJugador(identificadorUsuario, cartaSeleccionada);

                PonerImagenCarta(identificadorUsuario);

                cartaSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
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



        private void BtnCancelarSeleccion_Click(object sender, RoutedEventArgs e)
        {
            panSeleccionObjetivos.Visibility = Visibility.Collapsed;

            usuarioObjetivoSeleccionado = null;
            personajeObjetivoSeleccionado = null;

            panUsuariosObjetivo.Children.Clear();
            panPersonajesObjetivo.Children.Clear();

        }



        public void DeshabilitaCampos()
        {
            btnDescartar.IsEnabled = false;
            btnUsar.IsEnabled = false;
            decCarta.Visibility = Visibility.Visible;
            decCartaBonus.Visibility = Visibility.Visible;
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


        private void AsignarCartasPorJugador()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InstanceContext contextoTablero = new InstanceContext(this);
            ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient();

            try
            {
                var familiaPorJugador = proxy.ObtenerFamiliaPorJugador(salaNormal.idSala);
                if (familiaPorJugador == null || !familiaPorJugador.Any())
                {
                    return;
                }


                var familias = proxy.ObtenerFamiliasYPersonajes(salaNormal.idSala);


                var rutaImagenesPorPersonaje = new Dictionary<string, string>
        {
            { "Didorian", "ImagenesFamilia/Didorian.jpg" },
            { "Zael", "ImagenesFamilia/Zael.jpg" },
            { "Pablian", "ImagenesFamilia/Pablian.jpg" },
            { "Lorenzeo", "ImagenesFamilia/Lorenzeo.jpg" },
            { "Gaia", "ImagenesFamilia/Gaia.png" },
            { "Arialyn", "ImagenesFamilia/Arialyn.jpg" },
            { "Aris", "ImagenesFamilia/Aris.jpg" },
            { "Abelith", "ImagenesFamilia/Abelith.jpg" },
            { "Tucani", "ImagenesFamilia/Tucani.png" },
            { "Luan", "ImagenesFamilia/Luan.jpg" },
            { "Lusiel", "ImagenesFamilia/Lusiel.jpg" },
            { "Angelus", "ImagenesFamilia/Angelus.png" },
            { "Seti", "ImagenesFamilia/Seti.jpg" },
            { "Merit", "ImagenesFamilia/Merit.jpg" },
            { "Neferu", "ImagenesFamilia/Neferu.jpg" },
            { "Sobek", "ImagenesFamilia/Sobek.jpg" }
        };

                var labelJugadorMap = new[] { lblJugador1, lblJugador2, lblJugador3, lblJugador4 };
                var imgPersonajesMap = new[]
                {
            new[] { imgPersonaje1, imgPersonaje2, imgPersonaje3, imgPersonaje4 },
            new[] { imgPersonaje5, imgPersonaje6, imgPersonaje7, imgPersonaje8 },
            new[] { imgPersonaje9, imgPersonaje10, imgPersonaje11, imgPersonaje12 },
            new[] { imgPersonaje13, imgPersonaje14, imgPersonaje15, imgPersonaje16 }
        };

                int jugadorIndex = 0;
                string nombreUsuarioActual = identificadorUsuario;

                var jugadoresOrdenados = familiaPorJugador.Keys.OrderBy(j => j == nombreUsuarioActual ? 0 : 1).ToList();

                foreach (var nombreJugador in jugadoresOrdenados)
                {
                    var familia = familiaPorJugador[nombreJugador];



                    if (familias.TryGetValue(familia, out var personajes) && personajes != null && personajes.Count() > 0)
                    {
                        labelJugadorMap[jugadorIndex].Content = nombreJugador;

                        var imgPersonajes = imgPersonajesMap[jugadorIndex];

                        for (int i = 0; i < Math.Min(personajes.Count(), imgPersonajes.Length); i++)
                        {
                            var (nombrePersonaje, vida) = personajes[i];

                            if (rutaImagenesPorPersonaje.TryGetValue(nombrePersonaje, out var rutaImagen))
                            {
                                imgPersonajes[i].Tag = new { Familia = familia, Vida = vida, NombrePersonaje = nombrePersonaje };
                                imgPersonajes[i].Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                            }
                        }
                    }


                    jugadorIndex++;
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


        private void BtnChat_Click(object sender, RoutedEventArgs e)
        {

            Chat ventanaChat = new Chat(lblJugador1.Content.ToString());
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


        private void VerificarFinDePartida()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoTablero = new InstanceContext(this);
                ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);

                proxy.TerminarPartidaNormal(salaNormal.idSala);
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

        private string DeterminarGanador(Dictionary<string, (string, int)[]> familiasYPersonajes)
        {
            string ganador = string.Empty;
            int vidaMaxima = int.MinValue;

            foreach (var jugador in familiasYPersonajes)
            {
                int vidaTotal = jugador.Value.Sum(p => p.Item2);

                if (vidaTotal > vidaMaxima)
                {
                    vidaMaxima = vidaTotal;
                    ganador = jugador.Key;
                }
            }

            return ganador;
        }




        



        public void EnviarTurno(string nombreDelUsuarioEnTurno)
        {
            btnDescartar.IsEnabled = nombreDelUsuarioEnTurno.Equals(identificadorUsuario);
            btnUsar.IsEnabled = nombreDelUsuarioEnTurno.Equals(identificadorUsuario);
            btnMazoCartas.IsEnabled = nombreDelUsuarioEnTurno.Equals(identificadorUsuario);
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

        public void ActualizarImagenMazoCartaSobrante()
        {

            btnMazoCartas.Visibility = Visibility.Collapsed;
        }

        public void ActualizarImagenMazoCartaBonus()
        {
            btnCartaBonus.Visibility = Visibility.Collapsed;
            decCartaBonusFinal.Visibility = Visibility.Visible;
        }

        public void ActualizarMazoJugador()
        {
            PonerImagenCarta(identificadorUsuario);
            jugadorSeleciconadoParaCastigo = "sin jugador";
        }

        public void EnviarGanador(string jugador)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                string mensaje = jugador.Equals("Sin ganador") ? Properties.Resources.partidaFinSinGanador : Properties.Resources.finPartidaFelicitacionLeyenda +" "+ jugador;
                MessageBox.Show(mensaje, Properties.Resources.partidaFinPartida, MessageBoxButton.OK, MessageBoxImage.Information);

                FinPartidaNormal ventanaFin = new FinPartidaNormal(identificadorUsuario, jugador, lblNumeroSala.Content.ToString());
                ventanaFin.Show();
                this.Close();
            });

        }

        public void NotificarVotacionExpulsion(string jugadorPropuesto)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                
                bool votoAFavor = MessageBox.Show(
                    string.Format(Properties.Resources.mensajePreguntaExpulsionDeJugador, jugadorPropuesto),
                    Properties.Resources.mensajeTituloVotacionExpulsion, 
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                )== MessageBoxResult.Yes;


                InstanceContext contexto = new InstanceContext(this);
                ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contexto);

                proxy.RegistrarVotoExpulsion(lblJugador1.Content.ToString(), jugadorPropuesto, votoAFavor);
            });
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
            var etiquetas = new[] { lblJugador1, lblJugador2, lblJugador3, lblJugador4 };
            var imagenes = new[]
            {
        new[] { imgPersonaje1, imgPersonaje2, imgPersonaje3, imgPersonaje4 },
        new[] { imgPersonaje5, imgPersonaje6, imgPersonaje7, imgPersonaje8 },
        new[] { imgPersonaje9, imgPersonaje10, imgPersonaje11, imgPersonaje12 },
        new[] { imgPersonaje13, imgPersonaje14, imgPersonaje15, imgPersonaje16 }
    };

            for (int i = 0; i < etiquetas.Length; i++)
            {
                if (etiquetas[i]?.Content != null && etiquetas[i].Content.ToString() == jugadorExpulsado)
                {
                    etiquetas[i].Content = string.Empty;

                    foreach (var imagen in imagenes[i])
                    {
                        if (imagen != null)
                        {
                            imagen.Source = null;
                            imagen.Tag = null;
                        }
                    }

                    break;
                }
            }
        }


        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }

        public void ActualizarJugadorMuerto(string jugadorMuerto)
        {
            Inicio nuevaVentana = new Inicio(identificadorUsuario);
            nuevaVentana.Show();
            this.Close();
        }

        public void NotificarResultadoVotacion(string mensaje)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(mensaje, Properties.Resources.mensajeResultadoVotacion, MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
    }
}