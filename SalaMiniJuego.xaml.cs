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
using System.Windows.Threading;

namespace ClienteGloomApp
{
    /// <summary>
    /// Lógica de interacción para SalaMiniJuego.xaml
    /// </summary>
    public partial class SalaMiniJuego : Window, ISalaCallback, IServicioJuegoTableroCallback
    {
        private DispatcherTimer timer;
        ServicioGloom.Sala salaRegistrada = new ServicioGloom.Sala();
        public SalaMiniJuego(String nombreUsuario, Sala sala)
        {
            InitializeComponent();
            lblNombreUsuarioRegistrado.Content = nombreUsuario;
            salaRegistrada = sala;
            ConectarConSala();
            ActualizarNumeroJugadores();
           
        }
        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            /*Inicio nuevaVentana = new Inicio(lblNombreUsuarioRegistrado.Content.ToString());
            nuevaVentana.Show();
            this.Close();*/
        }

        private void ConectarConSala()
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            proxy.ConectarConSala(lblNombreUsuarioRegistrado.Content.ToString());
        }
        private void ActualizarJugadores()
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            proxy.ConectarConSala(lblNombreUsuarioRegistrado.Content.ToString());
            var listaJugadores = proxy.ObtenerJugadoresConectados(lblNombreUsuarioRegistrado.Content.ToString());
            txtJugador1.Text = listaJugadores.Count() > 0 ? listaJugadores[0] : string.Empty;
            txtJugador2.Text = listaJugadores.Count() > 1 ? listaJugadores[1] : string.Empty;
            txtJugador3.Text = listaJugadores.Count() > 2 ? listaJugadores[2] : string.Empty;
            txtJugador4.Text = listaJugadores.Count() > 3 ? listaJugadores[3] : string.Empty;
        }

        public void EmpezarJuego()
        {
            PartidaMiniJuego nuevaVentana = new PartidaMiniJuego(lblNombreUsuarioRegistrado.Content.ToString(), salaRegistrada.noJugadores, salaRegistrada.idSala);
            nuevaVentana.Show();
            this.Close();
        }

        private void btnTucani_Click(object sender, RoutedEventArgs e)
        {
            IngresarSeleccionPersonaje("Tucani");
        }

        private void btnLusiel_Click(object sender, RoutedEventArgs e)
        {
            IngresarSeleccionPersonaje("Lusiel");
        }

        private void btnAngelus_Click(object sender, RoutedEventArgs e)
        {
            IngresarSeleccionPersonaje("Angelus");
        }

        private void btnLuan_Click(object sender, RoutedEventArgs e)
        {
            IngresarSeleccionPersonaje("Luan");
        }

        private void btnEmpezar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IngresarJugadorEnTablero();

                if (ValidarAdministrador())
                {
                    InstanceContext contextoSala = new InstanceContext(this);
                    ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

                    proxy.validarPersonajesSeleccionados(salaRegistrada.noJugadores);
                    proxy.EmpezarPartida(salaRegistrada.idSala);
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

        private bool ValidarAdministrador()
        {
            return salaRegistrada.idAdministrador.Equals(lblNombreUsuarioRegistrado.Content.ToString());
        }

        public void ActualizarNumeroJugadores()
        {
            
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            var listaJugadores = proxy.ObtenerJugadoresConectados(lblNombreUsuarioRegistrado.Content.ToString());
            txtJugador1.Text = listaJugadores.Count() > 0 ? listaJugadores[0] : string.Empty;
            txtJugador2.Text = listaJugadores.Count() > 1 ? listaJugadores[1] : string.Empty;
            txtJugador3.Text = listaJugadores.Count() > 2 ? listaJugadores[2] : string.Empty;
            txtJugador4.Text = listaJugadores.Count() > 3 ? listaJugadores[3] : string.Empty;
            
        }
       
        private void IngresarJugadorEnTablero()
        {
            InstanceContext contextoSala= new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxySala= new ServicioGloom.ServicioJuegoTableroClient(contextoSala);

            proxySala.IngresarJugadorAJuego(lblNombreUsuarioRegistrado.Content.ToString(), salaRegistrada.idSala, salaRegistrada.noJugadores);
        }

        private void IngresarSeleccionPersonaje(String personaje)
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

                proxy.SeleccionarPersonaje(lblNombreUsuarioRegistrado.Content.ToString(), personaje, 0);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }

        public void EnviarTurno(string nombreDelUsusarioEnTurno)
        {
            throw new NotImplementedException();
        }
    }


}
