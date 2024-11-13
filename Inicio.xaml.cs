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
    public partial class Inicio : Window, ISalaCallback
    {
        private String identificadorUsuario;
        private String tipoJugador;

        public Inicio(String nombreDelUsuario)
        {
            InitializeComponent();

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
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                ServicioGloom.Sala sala = new ServicioGloom.Sala();
                var resultado = proxy.BuscarSalaExistente(txtIdSala.Text, txtCodigo.Text);
                if (resultado!=null)
                {
                    SalaMiniJuego nuevaVentana = new SalaMiniJuego(lblNombreUsuario.Content.ToString(), resultado);
                    nuevaVentana.Show();
                    this.Close();
                }

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
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

        public void EmpezarJuego()
        {
            throw new NotImplementedException();
        }

        public void ActualizarNumeroJugadores()
        {
            throw new NotImplementedException();
        }

        public void ActualizarImagenPersonaje(string personaje, string personajeAnterior)
        {
            throw new NotImplementedException();
        }

        public void EnviarGanador(string jugador)
        {
            throw new NotImplementedException();
        }
    }
}
