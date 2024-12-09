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
    /// Lógica de interacción para Inicio.xaml
    /// </summary>
    public partial class Inicio : Window
    {
        private String tipoJugador;

        public Inicio(String nombreDelUsuario)
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            lblNombreUsuario.Content = nombreDelUsuario;
            ValidarTipoJugador(nombreDelUsuario);
            RestringirTipoJugador();


        }

        public void Response(int result)
        {
            throw new NotImplementedException();
        }

        private void btnPerfil_Click(object sender, RoutedEventArgs e)
        {
            PerfilJugador nuevaVentana = new PerfilJugador(lblNombreUsuario.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }

        private void btnVerPersonajes_Click(object sender, RoutedEventArgs e)
        {
            HistoriaPersonajes nuevaVentana = new HistoriaPersonajes(lblNombreUsuario.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }

        private void btnListaDeAmigos_Click(object sender, RoutedEventArgs e)
        {
            ListaAmigos nuevaVentana = new ListaAmigos(lblNombreUsuario.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }

        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }

        private void btnHistorialDePartidas_Click(object sender, RoutedEventArgs e)
        {
            HistorialPartidas nuevavenatana = new HistorialPartidas(lblNombreUsuario.Content.ToString());
            nuevavenatana.Show();
            this.Close();
        }

        private void btnMiniHistoria_Click(object sender, RoutedEventArgs e)
        {
            panelMiniEntrada .Visibility = Visibility.Visible;
        }

        private void btnFlechaPanelAmigos_Click(object sender, RoutedEventArgs e)
        {
            panelMiniEntrada.Visibility = Visibility.Collapsed;
        }

        private void btnEntrarPartidaMiniHistoria_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext contextoCrearPartida= new InstanceContext(this);
                ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient(contextoCrearPartida);
                ServicioGloom.Sala sala = new ServicioGloom.Sala();
                var resultadoSala = proxy.BuscarSalaExistente(txtIdSala.Text, txtCodigo.Text);
                ValidarSalaActiva(resultadoSala.ganador);
                if (resultadoSala != null)
                {
                    proxy.ValidarCantidadJugadoresEnSala(resultadoSala.idSala, resultadoSala.noJugadores);
                    SalaMiniJuego nuevaVentana = new SalaMiniJuego(lblNombreUsuario.Content.ToString(), resultadoSala);
                    nuevaVentana.Show();
                    this.Close();
                }

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);

            }catch (InvalidOperationException ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
            }
        }

        private void ValidarSalaActiva(string ganador)
        {
            if (!ganador.Equals("Sin ganador"))
            {
                throw new InvalidOperationException("43");
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

        private void RestringirTipoJugador()
        {
            if(tipoJugador == "Invitado")
            {
                btnCrearPartida.IsEnabled = false;
                btnPerfil.IsEnabled = false;
                btnListaDeAmigos.IsEnabled = false;
                btnHistorialDePartidas.IsEnabled = false;
                lblInstruccionInvitado.Visibility = Visibility.Visible;
            }
        }

        private void btnCrearPartida_Click(object sender, RoutedEventArgs e)
        {
            CrearPartida ventanaCrearPartida = new CrearPartida(lblNombreUsuario.Content.ToString());
            ventanaCrearPartida.Show();
            this.Close();
        }

        private void btnBuscarPartida_Click(object sender, RoutedEventArgs e)
        {
            BusquedaPartida ventanBusqueda = new BusquedaPartida(lblNombreUsuario.Content.ToString());
            ventanBusqueda.Show();
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
