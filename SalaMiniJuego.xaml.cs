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
    public partial class SalaMiniJuego : Window, ISalaCallback
    {
        private DispatcherTimer timer;
        ServicioGloom.Sala salaRegistrada = new ServicioGloom.Sala();
        public SalaMiniJuego(String nombreUsuario, Sala sala)
        {
            InitializeComponent();
            lblNombreUsuarioRegistrado.Content = nombreUsuario;
            salaRegistrada = sala;
            ActualizarJugadores();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            timer.Start();

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            //ActualizarJugadores();
        }
        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            /*Inicio nuevaVentana = new Inicio(lblNombreUsuarioRegistrado.Content.ToString());
            nuevaVentana.Show();
            this.Close();*/
        }

        public void EmpezarJuego(List<string> jugadores)
        {
            // Aquí puedes actualizar los TextBox con la lista de jugadores recibida del callback
            txtJugador1.Text = jugadores.Count > 0 ? jugadores[0] : string.Empty;
            txtJugador2.Text = jugadores.Count > 1 ? jugadores[1] : string.Empty;
            txtJugador3.Text = jugadores.Count > 2 ? jugadores[2] : string.Empty;
            txtJugador4.Text = jugadores.Count > 3 ? jugadores[3] : string.Empty;
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
            throw new NotImplementedException();
        }

        private void btnTucani_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

                proxy.SeleccionarPersonaje(lblNombreUsuarioRegistrado.Content.ToString(), "Tucani", 0);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
            
        }

        private void btnLusiel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

                proxy.SeleccionarPersonaje(lblNombreUsuarioRegistrado.Content.ToString(), "Lusiel", 0);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
            
        }

        private void btnAngelus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

                proxy.SeleccionarPersonaje(lblNombreUsuarioRegistrado.Content.ToString(), "Angelus", 0);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
            
        }

        private void btnLuan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

                proxy.SeleccionarPersonaje(lblNombreUsuarioRegistrado.Content.ToString(), "Luan", 0);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
            
        }

        private void btnEmpezar_Click(object sender, RoutedEventArgs e)
        {  
            try
            {
                validarAdministrador();
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                proxy.validarPersonajesSeleccionados(salaRegistrada.noJugadores);
                PartidaMiniJuego nuevaVentana = new PartidaMiniJuego(lblNombreUsuarioRegistrado.Content.ToString(), salaRegistrada.noJugadores, salaRegistrada.idSala);
                nuevaVentana.Show();
                this.Close();
            }
            catch(InvalidOperationException ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
            
        }

        private void validarAdministrador()
        {
            if (!salaRegistrada.idAdministrador.Equals(lblNombreUsuarioRegistrado.Content.ToString()))
            {
                throw new InvalidOperationException("16");
            }
        }
    }

   
}
