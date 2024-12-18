﻿using ClienteGloomApp.ServicioGloom;
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
    public partial class PartidaNormal : Window, IServicioJuegoTableroCallback, IServicioCartaCallback, ICreacionPartidaCallback, ISalaCallback
    {
        ServicioGloom.Sala salaNormal = new ServicioGloom.Sala();

        List<Carta> mazoDeJugador = new List<Carta>();
        Carta cartaSeleccionada = new Carta();
        Carta cartaBonusSeleciconada = new Carta();
        string jugadorSeleciconadoParaCastigo = "sin jugador";
        private string identificadorUsuario;

        private InstanceContext contextoJugador;
        private ServicioGloom.ServicioJuegoTableroClient proxyJugador;

        private string usuarioObjetivoSeleccionado;
        private string personajeObjetivoSeleccionado;

        public PartidaNormal(String nombreUsaurio, Sala sala)
        {
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
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxySala = new ServicioGloom.SalaClient(contextoSala);
                int jugadoresConectados = proxySala.ObtenerJugadoresConectados(sala.idSala).Count();

                proxy.IniciarPartidaPorAdministrador(identificadorUsuario, sala.idSala, jugadoresConectados);
                proxy.ConectarConTablero(identificadorUsuario, sala.idSala);

                AsignarCartasPorJugador();
                PonerImagenCarta(nombreUsaurio);
                ObtenerPrimerTurno();

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);

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


        private void ObtenerPrimerTurno()
        {
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
                MessageBox.Show($"Error al asignar el primer turno: {ex.Detail.mensaje}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                panCarta.Visibility = Visibility.Collapsed;

                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient(contextoCarta);

                proxy.QuitarCartaDeMazoJugador(identificadorUsuario, cartaSeleccionada);

                PonerImagenCarta(identificadorUsuario);

                cartaSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };

                ActualizarTurnoLlamar();
                DeshabilitaCampos();
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }



        private void BtnUsar_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(cartaSeleccionada.identificador))
            {
                MensajesEmergentes.MostrarMensaje("Debes seleccionar una carta antes de usarla.", "Advertencia");
                return;
            }


                MostrarSeleccionObjetivos(cartaSeleccionada);
        }



        private void BtnMazoCartas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VerificarFinDePartida();
                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient(contextoCarta);
                proxy.AgregarCartaAMazoJugador(identificadorUsuario);
                PonerImagenCarta(identificadorUsuario);
                DeshabilitaCampos();
                ActualizarTurnoLlamar();

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }

        }

        private void BtnCartaBonus_Click(object sender, RoutedEventArgs e)
        {
            panCartaBonus.Visibility = Visibility.Visible;
            InstanceContext contextoCarta = new InstanceContext(this);
            ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient(contextoCarta);
            Carta carta = proxy.ObtenerCartasBonus();
            cartaBonusSeleciconada = carta;
            string identificador = carta.identificador;

            if (RutasDeCartas.RutasImagenesCartaBonus.TryGetValue(identificador, out var rutaImagen))
            {
                imgCartaBonus.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
            }
            PonerInformacionCartaBonus(carta.tipo);
        }


        private void PonerInformacionCartaBonus(string tipo)
        {
            try
            {
                InstanceContext contexto = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contexto);

                var jugadoresEnSala = proxy.ObtenerJugadoresConectados(lblNumeroSala.Content.ToString());

                panCartaBonus.Children.Clear();

                Grid gridPrincipal = new Grid();
                gridPrincipal.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.7, GridUnitType.Star) }); // Carta (más espacio)
                gridPrincipal.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.3, GridUnitType.Star) }); // Botones (menos espacio)

                StackPanel panelCarta = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                switch (tipo)
                {
                    case "saltarJugador":
                        lblCarta.Content = Properties.Resources.cartaSaltarJugador;
                        break;
                    case "robar2Cartas":
                        lblCarta.Content = Properties.Resources.cartaRobarCarta2;
                        break;
                    case "robar1Cartas":
                        lblCarta.Content = Properties.Resources.cartaRobarCarta1;
                        break;
                    case "QuitarCarta":
                        lblCarta.Content = Properties.Resources.cartaQuitarCarta;
                        break;
                    case "PerderTurno":
                        lblCarta.Content = Properties.Resources.cartaPerderTurno;
                        break;
                    default:
                        lblCarta.Content = null;
                        break;
                }

                panelCarta.Children.Add(new Label
                {
                    Content = lblCarta.Content,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold
                });

                StackPanel panelBotones = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5)
                };

                if (tipo == "saltarJugador" || tipo == "QuitarCarta")
                {
                    foreach (var usuario in jugadoresEnSala.Where(u => u != identificadorUsuario))
                    {
                        Button btnUsuario = new Button
                        {
                            Content = usuario,
                            Tag = usuario,
                            Width = 80,
                            Height = 30,
                            Margin = new Thickness(5),
                            Background = Brushes.LightBlue,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };

                        btnUsuario.Click += (s, e) =>
                        {
                            CambiarJugadorBonus((string)((Button)s).Tag, btnUsuario);
                        };

                        panelBotones.Children.Add(btnUsuario);
                    }

                }


                Grid.SetRow(panelCarta, 0); 
                Grid.SetRow(panelBotones, 1); 
                gridPrincipal.Children.Add(panelCarta);
                gridPrincipal.Children.Add(panelBotones);

                // Agregar el Grid al contenedor principal
                panCartaBonus.Children.Add(gridPrincipal);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
            
        }



        private void CambiarJugadorBonus(string usuarioSeleccionado, Button botonSeleccionado)
        {
            jugadorSeleciconadoParaCastigo = usuarioSeleccionado;

            foreach (Button btn in panCartaBonus.Children.OfType<Button>())
            {
                btn.BorderThickness = new Thickness(0);
                btn.BorderBrush = Brushes.Transparent;
            }

            botonSeleccionado.BorderBrush = new SolidColorBrush(Colors.Red);
            botonSeleccionado.BorderThickness = new Thickness(2);

        }


        private void CerrarCartaBonus_Click(object sender, RoutedEventArgs e)
        {
            if ((cartaBonusSeleciconada.tipo == "saltarJugador" || cartaBonusSeleciconada.tipo == "QuitarCarta") &&
                string.IsNullOrEmpty(jugadorSeleciconadoParaCastigo))
            {
                MensajesEmergentes.MostrarMensaje("Debe seleccionar un jugador antes de aplicar esta carta.", "Advertencia");
                return;
            }

            panCartaBonus.Visibility = Visibility.Collapsed;
            AplicarCartaBonus();

            cartaBonusSeleciconada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
            ActualizarTurnoLlamar();
            DeshabilitaCampos();

            jugadorSeleciconadoParaCastigo = "";
            foreach (Button btn in panCartaBonus.Children.OfType<Button>())
            {
                btn.BorderThickness = new Thickness(0);
                btn.Visibility = Visibility.Collapsed;
            }

            panCartaBonus.Children.Clear();
        }




        /*private void CerrarCartaBonus_Click(object sender, RoutedEventArgs e)
        {
            panCartaBonus.Visibility = Visibility.Collapsed;
            AplicarCartaBonus();
            cartaBonusSeleciconada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
            ActualizarTurnoLlamar();
            DeshabilitaCampos();
            jugadorSeleciconadoParaCastigo = "";
            btnJugador2.BorderThickness = new Thickness(0);
            btnJugador3.BorderThickness = new Thickness(0);
            btnJugador4.BorderThickness = new Thickness(0);
            btnJugador2.Visibility = Visibility.Collapsed;
            btnJugador3.Visibility = Visibility.Collapsed;
            btnJugador4.Visibility = Visibility.Collapsed;
        }*/


        private void SeleccionarUsuarioObjetivo(string usuario)
        {
            usuarioObjetivoSeleccionado = usuario;
            CargarPersonajesDeUsuarioSeleccionado(usuario);
        }

        private void CargarPersonajesDeUsuarioSeleccionado(string usuario)
        {
            try
            {
                InstanceContext contexto = new InstanceContext(this);
                ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient(contexto);

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
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }


        private void SeleccionarPersonajeObjetivo(string personaje)
        {
            personajeObjetivoSeleccionado = personaje;
            Console.WriteLine($"Personaje objetivo seleccionado: {personajeObjetivoSeleccionado}");
        }


        private void ValidarTipoCarta(Carta cartaSeleccionada)
        {
            InstanceContext contextoTablero = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);

            if (cartaSeleccionada.tipo.Equals("modificador"))
            {
                if (cartaSeleccionada.valor > 0)
                {
                    if (string.IsNullOrEmpty(usuarioObjetivoSeleccionado) || string.IsNullOrEmpty(personajeObjetivoSeleccionado))
                    {
                        MensajesEmergentes.MostrarMensaje("Selecciona un usuario y personaje objetivo antes de continuar.", "Advertencia");
                        return;
                    }
                    proxy.AplicarModificadorPositivo(cartaSeleccionada, usuarioObjetivoSeleccionado, personajeObjetivoSeleccionado);
                }
                else
                {
                    if (string.IsNullOrEmpty(personajeObjetivoSeleccionado))
                    {
                        MensajesEmergentes.MostrarMensaje("Selecciona un personaje objetivo antes de continuar.", "Advertencia");
                        return;
                    }
                    proxy.AplicarModificadorNegativo(cartaSeleccionada, identificadorUsuario, personajeObjetivoSeleccionado);
                }
            }
            else if (cartaSeleccionada.tipo.Equals("muerte"))
            {
                if (string.IsNullOrEmpty(personajeObjetivoSeleccionado))
                {
                    MensajesEmergentes.MostrarMensaje("Selecciona un personaje objetivo antes de continuar.", "Advertencia");
                    return;
                }

                try
                {
                    proxy.AplicarCartaMuerte(salaNormal.idSala, identificadorUsuario, personajeObjetivoSeleccionado);
                    VerificarFinDePartida();
                }
                catch (FaultException<ManejadorExcepciones> ex)
                {
                    MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
                }
            }
        }


        private void MostrarSeleccionObjetivos(Carta cartaSeleccionada)
        {
            try
            {
                panCargando.Visibility = Visibility.Visible;

                InstanceContext contexto = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contexto);
                var jugadoresEnSala = proxy.ObtenerJugadoresConectados(salaNormal.idSala);

                if (jugadoresEnSala == null || !jugadoresEnSala.Any())
                {
                    MensajesEmergentes.MostrarMensaje("No hay jugadores conectados en la sala.", "Advertencia");
                    return;
                }

                panSeleccionObjetivos.Visibility = Visibility.Visible;
                panUsuariosObjetivo.Children.Clear();
                panPersonajesObjetivo.Children.Clear();

                if (cartaSeleccionada.tipo.Equals("modificador"))
                {
                    if (cartaSeleccionada.valor > 0) 
                    {
                        lblSeleccionUsuario.Content = "Selecciona el usuario objetivo:";
                        lblSeleccionPersonaje.Content = "Selecciona el personaje objetivo del usuario seleccionado:";

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
                        lblSeleccionUsuario.Content = "Selecciona un personaje de tu familia:";
                        lblSeleccionPersonaje.Content = string.Empty;

                        MostrarPersonajesUsuarioSeleccionado(identificadorUsuario);
                    }
                }
                else if (cartaSeleccionada.tipo.Equals("muerte"))
                {
                    lblSeleccionUsuario.Content = "Selecciona un personaje de tu familia para aplicar la carta de muerte:";
                    lblSeleccionPersonaje.Content = string.Empty;
                    MostrarPersonajesUsuarioSeleccionado(identificadorUsuario); 
                }
                
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
            finally
            {
                
                panCargando.Visibility = Visibility.Collapsed;
            }
        }


        private void MostrarPersonajesUsuarioSeleccionado(string usuario)
        {
            try
            {

                panPersonajesObjetivo.Children.Clear();

                InstanceContext contextoTableroJuego = new InstanceContext(this);
                ServicioGloom.CreacionPartidaClient proxyPartida = new ServicioGloom.CreacionPartidaClient(contextoTableroJuego);
                var personajes = proxyPartida.ObtenerFamiliaYPersonajesPorUsuario(salaNormal.idSala);

                if (personajes == null || !personajes.ContainsKey(usuario))
                {
                    MensajesEmergentes.MostrarMensaje($"No se encontraron personajes para el usuario seleccionado.", "Advertencia");
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
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }




        private void BtnConfirmarSeleccion_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(personajeObjetivoSeleccionado))
            {
                MensajesEmergentes.MostrarMensaje("Debes seleccionar un personaje objetivo para continuar.", "Advertencia");
                return;
            }

            if (cartaSeleccionada.tipo.Equals("modificador") && cartaSeleccionada.valor > 0 && string.IsNullOrEmpty(usuarioObjetivoSeleccionado))
            {
                MensajesEmergentes.MostrarMensaje("Debes seleccionar un usuario y un personaje objetivo para continuar.", "Advertencia");
                return;
            }

            try
            {
                ValidarTipoCarta(cartaSeleccionada);

                panSeleccionObjetivos.Visibility = Visibility.Collapsed;

                ActualizarTurnoLlamar();
                DeshabilitaCampos();

                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient(contextoCarta);

                proxy.QuitarCartaDeMazoJugador(identificadorUsuario, cartaSeleccionada);

                PonerImagenCarta(identificadorUsuario);

                cartaSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
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

        


        private void AplicarCartaBonus()
        {
            VerificarFinDePartida();
            InstanceContext contextoTablero = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);
            InstanceContext contextoCarta = new InstanceContext(this);
            ServicioGloom.ServicioCartaClient proxyCarta = new ServicioGloom.ServicioCartaClient(contextoCarta);
            try
            {
                switch (cartaBonusSeleciconada.tipo)
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
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }

        private void ValidarSeleccionJugadorParaCastigo()
        {
            if (jugadorSeleciconadoParaCastigo.Equals("sin jugador"))
            {
                throw new InvalidOperationException("40");
            }
        }

        private void ActualizarTurnoLlamar()
        {
            try
            {
                if (proxyJugador.EsSalaActiva(lblNumeroSala.Content.ToString())) 
                {
                    proxyJugador.CambiarTurno(lblNumeroSala.Content.ToString());
                    VerificarFinDePartida();
                }
                else
                {
                    MessageBox.Show("La sala ya no está activa. Regresando al menú principal.", "Turno no válido", MessageBoxButton.OK, MessageBoxImage.Information);
                    Inicio ventanaInicio = new Inicio(lblJugador1.Content.ToString());
                    ventanaInicio.Show();
                    this.Close();
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }


        private void AsignarCartasPorJugador()
        {

            InstanceContext contextoTablero = new InstanceContext(this);
            ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient(contextoTablero);

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
            catch(FaultException < ManejadorExcepciones > ex)
                {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }


        private void BtnChat_Click(object sender, RoutedEventArgs e)
        {
            
                Chat ventanaChat = new Chat(lblJugador1.Content.ToString(), salaNormal.idSala);
                ventanaChat.Show();
            
        }
    


        private void BtnIniciarVotacion_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

            try
            {
                var jugadores = proxy.ObtenerJugadoresConectados(lblNumeroSala.Content.ToString())
                                     .Where(j => j != lblJugador1.Content.ToString()) // Excluir al solicitante
                                     .ToList();

                if (jugadores.Count == 0)
                {
                    MessageBox.Show("No hay jugadores disponibles para expulsar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                lbxJugadoresParaExpulsion.ItemsSource = jugadores;
                panExpulsion.Visibility = Visibility.Visible; // Mostrar el panel de expulsión
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
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

                proxy.SolicitarExpulsion(lblJugador1.Content.ToString(), jugadorSeleccionado, lblNumeroSala.Content.ToString());

                MessageBox.Show($"Se ha solicitado la expulsión de {jugadorSeleccionado}.", "Expulsión iniciada", MessageBoxButton.OK, MessageBoxImage.Information);

                panExpulsion.Visibility = Visibility.Collapsed;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
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
            try
            {
                InstanceContext contextoTablero = new InstanceContext(this);
                ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);

                if (!proxy.EsSalaActiva(salaNormal.idSala))
                {
                    Inicio ventanaInicio = new Inicio(lblJugador1.Content.ToString());
                    ventanaInicio.Show();
                    this.Close();
                }

                proxy.TerminarPartidaNormal(salaNormal.idSala);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
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




        public void NotificarResultadoExpulsion(string jugadorExpulsado, bool expulsado)
        {
            if (expulsado)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ActualizarInterfazExpulsion(jugadorExpulsado);
                });
            }
        }

        void IServicioCartaCallback.NotificarActualizacion(string mensaje)
        {
            throw new NotImplementedException();
        }

        void ICreacionPartidaCallback.NotificarPartidaCreada(string mensaje)
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.EmpezarJuego()
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarNumeroJugadores()
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarImagenPersonaje(string personaje, string personajeAnterior)
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.SacarDeSalaATodosJugadores()
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarSalasActivas(Sala[] salasActivas)
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ResultadoUnirseASala(string idSala, string codigo, bool esExitoso)
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarSeleccionFamilia(string nombreFamilia, string nombreFamiliaAnterior)
        {
            throw new NotImplementedException();
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
                string mensaje = jugador.Equals("Sin ganador") ? "La partida terminó sin un ganador." : $"El ganador de la partida es {jugador}.";
                MessageBox.Show(mensaje, "Fin de la partida", MessageBoxButton.OK, MessageBoxImage.Information);

                FinPartidaNormal ventanaFin = new FinPartidaNormal(identificadorUsuario, jugador, lblNumeroSala.Content.ToString());
                ventanaFin.Show();
                this.Close();
            });

        }

        public void NotificarVotacionExpulsion(string jugadorPropuesto)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ActualizarInterfazExpulsion(jugadorPropuesto);
                MessageBox.Show($"{jugadorPropuesto} ha sido expulsado de la sala.", "Expulsión", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public void RecibirExpulsion(string jugadorObjetivo)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
 
                MessageBox.Show(jugadorObjetivo, "Has sido expulsado", MessageBoxButton.OK, MessageBoxImage.Warning);

                Inicio ventanaInicio = new Inicio(lblJugador1.Content.ToString()); // Asegúrate de que Inicio sea la ventana del menú principal
                ventanaInicio.Show();
                this.Close();
            });
        }

        public void ActualizarInterfazExpulsion(string jugadorExpulsado)
        {
            var labels = new[] { lblJugador1, lblJugador2, lblJugador3, lblJugador4 };
            var images = new[]
            {
        new[] { imgPersonaje1, imgPersonaje2, imgPersonaje3, imgPersonaje4 },
        new[] { imgPersonaje5, imgPersonaje6, imgPersonaje7, imgPersonaje8 },
        new[] { imgPersonaje9, imgPersonaje10, imgPersonaje11, imgPersonaje12 },
        new[] { imgPersonaje13, imgPersonaje14, imgPersonaje15, imgPersonaje16 }
    };

            for (int i = 0; i < labels.Length; i++)
            {
                if (labels[i].Content.ToString() == jugadorExpulsado)
                {

                    labels[i].Content = string.Empty;

                    foreach (var img in images[i])
                    {
                        img.Source = null;
                        img.Tag = null;
                    }
                    break;
                }
            }

        }

 
    }
}
