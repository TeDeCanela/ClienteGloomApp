﻿using ClienteGloomApp.ServicioGloom;
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

        List<Carta> mazoDeJugador = new List<Carta>();
        Carta cartaSeleccionada = new Carta();
        String jugadorPropietario;
        Carta cartaBonusSeleciconada = new Carta();
        string jugadorSeleciconadoParaCastigo="sin jugador";

        private InstanceContext contextoJugador;
        private ServicioGloom.ServicioJuegoTableroClient proxyJugador;


        public PartidaMiniJuego(string nombreUsuario, int cantidadJugadores, string numeroSala)
        {
            InitializeComponent();
            lblJugador1.Content = nombreUsuario;
            jugadorPropietario = nombreUsuario;
            lblNumeroSala.Content = numeroSala;
            contextoJugador = new InstanceContext(this);
            proxyJugador = new ServicioGloom.ServicioJuegoTableroClient(contextoJugador);
            InstanceContext contextoTableroJuego = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTableroJuego);

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
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);

            }


        }
        private void ObtenerPirmerTurno()
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
        public void EnviarTurno(string nombreDelUsuarioEnTurno)
        {
            btnDescartar.IsEnabled = nombreDelUsuarioEnTurno.Equals(jugadorPropietario);
            btnUsar.IsEnabled = nombreDelUsuarioEnTurno.Equals(jugadorPropietario);
            btnMazoCartas.IsEnabled = nombreDelUsuarioEnTurno.Equals(jugadorPropietario);
        }

        private void AsignarJugadores()
        {
            InstanceContext contextoTablero = new InstanceContext(this);
            ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient(contextoTablero);
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
                proxy.AgregarCartaAMazoJugador(jugadorPropietario);
                PonerImagenCarta(jugadorPropietario);
                DeshabilitaCampos();
                ActualizarTurnoLlamar();

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
                panCarta.Visibility = Visibility.Collapsed;
                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxy = new ServicioGloom.ServicioCartaClient(contextoCarta);
                proxy.QuitarCartaDeMazoJugador(jugadorPropietario, cartaSeleccionada);
                PonerImagenCarta(jugadorPropietario);
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
            panCarta.Visibility = Visibility.Collapsed;
            ValidarTipoCarta(cartaSeleccionada.tipo);
            PonerImagenCarta(jugadorPropietario);
            cartaSeleccionada = new Carta { identificador = string.Empty, valor = 0, tipo = "vacío" };
            ActualizarTurnoLlamar();
            DeshabilitaCampos();
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

        private void ActualizarTurnoLlamar()
        {
            proxyJugador.CambiarTurno(lblNumeroSala.Content.ToString());
        }

        private void ValidarTipoCarta(string tipo)
        {
            InstanceContext contextoTablero = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTablero);
            if (tipo.Equals("modificador"))
            {
                proxy.SumarVidaPersonaje(lblNumeroSala.Content.ToString() ,jugadorPropietario, cartaSeleccionada.valor);

                InstanceContext contextoCarta = new InstanceContext(this);
                ServicioGloom.ServicioCartaClient proxyCarta = new ServicioGloom.ServicioCartaClient(contextoCarta);
                proxyCarta.QuitarCartaDeMazoJugador(jugadorPropietario, cartaSeleccionada);
            }
            else if (tipo.Equals("muerte"))
            {
                try
                {
                    proxy.TerminarPartidaMiniJuego(lblNumeroSala.Content.ToString());
                }
                catch (FaultException<ManejadorExcepciones> ex)
                {
                    MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
                }

            }
        }

        public void EnviarGanador(string jugador)
        {
            FinPartidaMini nuevaVentana = new FinPartidaMini(jugadorPropietario, jugador, lblNumeroSala.Content.ToString());
            nuevaVentana.Show();
            this.Close();
            Console.WriteLine(jugador);
        }

        private void BtnChat_Click(object sender, RoutedEventArgs e)
        {
            //var chatWindow = new Chat(lblJugador1.Content.ToString(), lblNumeroSala.Content.ToString());
            //chatWindow.Show();
        }

        private void btnInvitarCorreo_Click(object sender, RoutedEventArgs e)
        {
            //var chatWindow = new Chat(lblJugador1.Content.ToString(), lblNumeroSala.Content.ToString());
            //chatWindow.Show();
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
        }

        public void ActualizarImagenMazoCartaBonus()
        {
            btnCartaBonus.Visibility = Visibility.Collapsed;
        }

        private void PonerInformacionCartaBonus(string tipo)
        {
            InstanceContext contextoTablero = new InstanceContext(this);
            ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient(contextoTablero);
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

        private void CerrarCartaBonus_Click(object sender, RoutedEventArgs e)
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
        }

        private void Cambiarjugador_Click(object sender, RoutedEventArgs e)
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
                        proxyCarta.QuitarCartaDeMazoJugadorExterno(jugadorSeleciconadoParaCastigo);
                        jugadorSeleciconadoParaCastigo = "sin jugador";
                        break;
                    case "PerderTurno":
                        proxy.AgregarCastigo(jugadorPropietario);

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

        public void ActualizarMazoJugador()
        {
            PonerImagenCarta(jugadorPropietario);
            jugadorSeleciconadoParaCastigo = "sin jugador";
        }

        private void ValidarSeleccionJugadorParaCastigo()
        {
            if (jugadorSeleciconadoParaCastigo.Equals("sin jugador"))
            {
                throw new InvalidOperationException("40");
            }
        }

        void IServicioJuegoTableroCallback.NotificarVotacionExpulsion(string jugadorPropuesto)
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.RecibirExpulsion(string jugadorObjetivo)
        {
            throw new NotImplementedException();
        }

        public void ActualizarInterfazExpulsion(string jugadorExpulsado)
        {
            throw new NotImplementedException();
        }


    }
}
