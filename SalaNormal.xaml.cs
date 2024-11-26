using ClienteGloomApp.ServicioGloom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Lógica de interacción para Sala.xaml
    /// </summary>
    public partial class SalaNormal : Window, ISalaCallback, IServicioJuegoTableroCallback
    {
        ServicioGloom.Sala salaNormal = new ServicioGloom.Sala();
        //private String identificadorUsuario;
        //private String idSalaNormal;
        bool familiaSeleccionada = false;
        public SalaNormal(String nombreUsuario, Sala sala)
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            lblnombreUsuario.Content = nombreUsuario;
            salaNormal = sala;

            

            try
            {
                ConectarConSala();
                ActualizarNumeroJugadores();
                FamiliaSeleccionadas();

                if (!ValidarAdministrador())
                {
                    btnEmpezar.Content = Properties.Resources.salaBtnListo;
                }

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }

        private bool ValidarAdministrador()
        {
            return salaNormal.idAdministrador.Equals(lblnombreUsuario.Content.ToString());
        }

        private void ConectarConSala()
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            proxy.ConectarConSala(lblnombreUsuario.Content.ToString());
        }

        public void ActualizarNumeroJugadores()
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                proxy.ConectarConSala(lblnombreUsuario.Content.ToString());
                var listaJugadores = proxy.ObtenerJugadoresConectados(lblnombreUsuario.Content.ToString());
                txtJugador1.Text = listaJugadores.Count() > 0 ? listaJugadores[0] : string.Empty;
                txtJugador2.Text = listaJugadores.Count() > 1 ? listaJugadores[1] : string.Empty;
                txtJugador3.Text = listaJugadores.Count() > 2 ? listaJugadores[2] : string.Empty;
                txtJugador4.Text = listaJugadores.Count() > 3 ? listaJugadores[3] : string.Empty;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }


        }

        private void FamiliaSeleccionadas()
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            List<string> familias = proxy.ObtenerFamiliaSeleccionada(salaNormal.idSala).ToList();

            foreach (var familia in familias)
            {
                switch (familia)
                {
                    case "Ores":
                        btnOres.BorderBrush = Brushes.Red;
                        btnOres.BorderThickness = new Thickness(2);
                        break;
                    case "Corbat":
                        btnCorbat.BorderBrush = Brushes.Fuchsia;
                        btnCorbat.BorderThickness = new Thickness(2);
                        break;
                    case "Garlo":
                        btnGarlo.BorderBrush = Brushes.Green;
                        btnGarlo.BorderThickness = new Thickness(2);
                        break;
                    case "Ramfez":
                        btnRamfez.BorderBrush = Brushes.Blue;
                        btnRamfez.BorderThickness = new Thickness(2);
                        break;
                }
            }
        }

        private void BtnOres_Click(object sender, RoutedEventArgs e)
        {
            txtDescripcionFamilia.Text = Properties.Resources.salaDescripcionOres;
            IngresarSeleccionFamilia("Ores");
        }

        private void BtnCorbat_Click(object sender, RoutedEventArgs e)
        {
            txtDescripcionFamilia.Text = Properties.Resources.salaDescripcionCorbat;
            IngresarSeleccionFamilia("Corbat");
        }

        private void BtnGarlo_Click(object sender, RoutedEventArgs e)
        {
            txtDescripcionFamilia.Text = Properties.Resources.salaDescripcionGarlo;
            IngresarSeleccionFamilia("Garlo");
        }

        private void BtnRamfez_Click(object sender, RoutedEventArgs e)
        {
            txtDescripcionFamilia.Text = Properties.Resources.salaDescripcionRamfez;
            IngresarSeleccionFamilia("Ramfez");
        }

        private void BtnEmpezar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine($"Estado de 'familiaSeleccionada' antes de la validación: {familiaSeleccionada}");
                ValidarSeleccionFamilia();

                Console.WriteLine("Selección de familia validada correctamente.");

                IngresarJugadorEnTablero();

                if (ValidarAdministrador())
                {
                    Console.WriteLine("El usuario es el administrador, procediendo con la validación de familias...");

                    // Instanciar el proxy del servicio
                    InstanceContext contexto = new InstanceContext(this);
                    ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contexto);

                    // Validar si todos los jugadores han seleccionado una familia
                    proxy.ValidarFamiliaSeleccionada(salaNormal.noJugadores, salaNormal.idSala);

                    proxy.EmpezarPartida(salaNormal.idSala);

                    // Si la validación es exitosa, proceder a iniciar la partida
                    //InstanceContext contextoTablero = new InstanceContext(this);
                    /*ServicioGloom.ServicioJuegoTableroClient proxyTablero = new ServicioGloom.ServicioJuegoTableroClient(contexto);
                    proxyTablero.IniciarPartidaPorAdministrador(lblnombreUsuario.Content.ToString(), salaNormal.idSala, salaNormal.noJugadores);
                    List<Carta> cartasSobrantes = proxyTablero.ObtenerCartasSobrantes().ToList();*/


                    // Cambiar a la ventana PartidaNormal
                    PartidaNormal ventanaPartida = new PartidaNormal(lblnombreUsuario.Content.ToString(), salaNormal);
                    ventanaPartida.Show();
                    this.Close();
                }

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Validación fallida: {ex.Message}");
                MessageBox.Show("No puede dar clic al botón 'Listo' si no ha seleccionado una familia.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                Console.WriteLine($"Excepción de servicio: {ex.Detail.mensaje}");
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }


        private void ValidarSeleccionFamilia()
        {
            if (!familiaSeleccionada)
            {
                Console.WriteLine("Validación fallida: 'familiaSeleccionada' es false.");
                throw new InvalidOperationException("31");
            }
        }

        private void IngresarJugadorEnTablero()
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxyTablero = new ServicioGloom.ServicioJuegoTableroClient(contextoSala);

            proxyTablero.IngresarJugadorAJuego(lblnombreUsuario.Content.ToString(), salaNormal.idSala, salaNormal.noJugadores);
        }

        private void BtnInvitarJugadores_Click(object sender, RoutedEventArgs e)
        {
            InvitacionJugador invitacionJugador = new InvitacionJugador(lblnombreUsuario.Content.ToString(), salaNormal.idSala);
            invitacionJugador.Show();
        }

        private void IngresarSeleccionFamilia(string familia)
        {
            try
            {
                //DeshabilitarBotonFamilia(familia);
                // Notificar al servidor la nueva selección
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

                proxy.SeleccionarFamilia(lblnombreUsuario.Content.ToString(), familia, salaNormal.idSala);

                familiaSeleccionada = true;


                // Actualizar visualmente la selección en el cliente

                Console.WriteLine($"Familia seleccionada por {lblnombreUsuario.Content} es {familia}, estado de familiaSeleccionada: {familiaSeleccionada}");
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
        }


        private void ActivarTodosLosBotones()
        {
            btnOres.IsEnabled = true;
            btnCorbat.IsEnabled = true;
            btnGarlo.IsEnabled = true;
            btnRamfez.IsEnabled = true;
        }


        private void ActivarBotonFamilia(string familia)
        {
            switch (familia)
            {
                case "Ores":
                    btnOres.IsEnabled = true;
                    break;
                case "Corbat":
                    btnCorbat.IsEnabled = true;
                    break;
                case "Garlo":
                    btnGarlo.IsEnabled = true;
                    break;
                case "Ramfez":
                    btnRamfez.IsEnabled = true;
                    break;
            }
        }

        private void DeshabilitarBotonFamilia(string familia)
        {
            switch (familia)
            {
                case "Ores":
                    btnOres.IsEnabled = false;
                    break;
                case "Corbat":
                    btnCorbat.IsEnabled = false;
                    break;
                case "Garlo":
                    btnGarlo.IsEnabled = false;
                    break;
                case "Ramfez":
                    btnRamfez.IsEnabled = false;
                    break;
            }
        }



        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            if (ValidarAdministrador())
            {
                // Llama al método del servicio para que el administrador salga de la sala
                proxy.SalirDeSala(salaNormal.idSala, lblnombreUsuario.Content.ToString());
                MessageBox.Show("Has salido de la sala y esta ha sido eliminada.", "Sala eliminada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                proxy.SacarDeSala(lblnombreUsuario.Content.ToString());
            }
            Inicio nuevaVentana = new Inicio(lblnombreUsuario.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }


        void ISalaCallback.EmpezarJuego()
        {

        }

        void ISalaCallback.ActualizarNumeroJugadores()
        {
            Dispatcher.Invoke(() =>
            {
                ActualizarNumeroJugadores();
            });
        }

        void ISalaCallback.ActualizarImagenPersonaje(string personaje, string personajeAnterior)
        {
            
        }


        void ISalaCallback.ResultadoUnirseASala(string idSala, string codigo, bool esExitoso)
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarSeleccionFamilia(string nombreUsuario, string nombreFamilia)
        {
            Dispatcher.Invoke(() =>
            {
                Console.WriteLine($"ActualizarSeleccionFamilia callback recibido: {nombreUsuario} ha seleccionado {nombreFamilia}");
                DeshabilitarBotonFamilia(nombreFamilia);
            });
        }

        public void CambiarFamiliaAnterior(string familiaAnterior)
        {
            if (!familiaAnterior.Equals("Sin familia"))
            {
                switch (familiaAnterior)
                {
                    case "Ores":
                        btnOres.BorderBrush = Brushes.Red;
                        btnOres.BorderThickness = new Thickness(2);
                        break;
                    case "Corbat":
                        btnCorbat.BorderBrush = Brushes.Fuchsia;
                        btnCorbat.BorderThickness = new Thickness(2);
                        break;
                    case "Garlo":
                        btnGarlo.BorderBrush = Brushes.Green;
                        btnGarlo.BorderThickness = new Thickness(2);
                        break;
                    case "Ramfez":
                        btnRamfez.BorderBrush = Brushes.Blue;
                        btnRamfez.BorderThickness = new Thickness(2);
                        break;
                }
            }
        }

        void IServicioJuegoTableroCallback.EnviarTurno(string nombreDelUsusarioEnTurno)
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

        void IServicioJuegoTableroCallback.EnviarGanador(string jugador)
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

        void ISalaCallback.ActualizarSalasActivas(Sala[] salasActivas)
        {
            throw new NotImplementedException();
        }
    }

}