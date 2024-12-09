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
        bool familiaSeleccionada = false;
        public SalaNormal(String nombreUsuario, Sala sala)
        {
            

            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            lblnombreUsuario.Content = nombreUsuario;
            salaNormal = sala;

            if (sala.tipoPartida == "Privada")
            {
                btnInvitarJugadores.Visibility = Visibility.Visible;
            }

            try
            {
                ConectarConSala();
                ActualizarNumeroJugadores();
                PonerFamiliasSeleccionadas();

                if (!ValidarAdministrador())
                {
                    btnEmpezar.Content = Properties.Resources.salaBtnListo;
                }

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
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

        private bool ValidarAdministrador()
        {
            return salaNormal.idAdministrador.Equals(lblnombreUsuario.Content.ToString());
        }

        private void ConectarConSala()
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                proxy.ConectarConSala(salaNormal.idSala, lblnombreUsuario.Content.ToString());
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

        public void ActualizarNumeroJugadores()
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                var listaJugadores = proxy.ObtenerJugadoresConectados(salaNormal.idSala);
                txtJugador1.Text = listaJugadores.Count() > 0 ? listaJugadores[0] : string.Empty;
                txtJugador2.Text = listaJugadores.Count() > 1 ? listaJugadores[1] : string.Empty;
                txtJugador3.Text = listaJugadores.Count() > 2 ? listaJugadores[2] : string.Empty;
                txtJugador4.Text = listaJugadores.Count() > 3 ? listaJugadores[3] : string.Empty;
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
                ValidarSeleccionFamilia();
                IngresarJugadorEnTablero();

                if (ValidarAdministrador())
                {
                    InstanceContext contexto = new InstanceContext(this);
                    ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contexto);

                    proxy.ValidarFamiliaSeleccionada(salaNormal.noJugadores, salaNormal.idSala);
                    proxy.CambiarEstadoParaPartida(salaNormal.idSala, "En partida");
                    proxy.EmpezarPartida(salaNormal.idSala);

                }
                btnEmpezar.BorderBrush = Brushes.Green;
                btnEmpezar.BorderThickness = new Thickness(4);

            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(Properties.Resources.mensajeExp19, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);//cambiarlo
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
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


        private void ValidarSeleccionFamilia()
        {
            if (!familiaSeleccionada)
            {
                throw new InvalidOperationException("31");
            }
        }

        private void IngresarJugadorEnTablero()
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxySala = new ServicioGloom.SalaClient(contextoSala);

                proxySala.IngresarJugadorAJuego(lblnombreUsuario.Content.ToString(), salaNormal.idSala, salaNormal.noJugadores);
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

        private void BtnInvitarJugadores_Click(object sender, RoutedEventArgs e)
        {
            InvitacionJugador invitacionJugador = new InvitacionJugador(lblnombreUsuario.Content.ToString(), salaNormal.idSala);
            invitacionJugador.Show();
        }

        private void IngresarSeleccionFamilia(string familia)
        {
            try
            {

                familiaSeleccionada = true;
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

                proxy.SeleccionarFamilia(lblnombreUsuario.Content.ToString(), familia, salaNormal.idSala);

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
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
            btnEmpezar.BorderBrush = Brushes.Red;
            btnEmpezar.BorderThickness = new Thickness(4);
        }






        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                if (ValidarAdministrador())
                {
                    proxy.SacarATodosLosJugadoresDeSala(salaNormal.idSala);
                    MessageBox.Show(Properties.Resources.mensajeHaSalidoDeSala, Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    proxy.SacarDeSala(salaNormal.idSala, lblnombreUsuario.Content.ToString());
                    Inicio regresoInicio = new Inicio(lblnombreUsuario.Content.ToString());
                    regresoInicio.Show();
                    this.Close();
                }
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


        public void EmpezarJuego()
        {
            PartidaNormal nuevaVentana = new PartidaNormal(lblnombreUsuario.Content.ToString(), salaNormal);
            nuevaVentana.Show();
            this.Close();
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

        void ISalaCallback.ActualizarSeleccionFamilia(string familia, string familiaAnterior)
        {
            CambiarFamiliaAnterior(familiaAnterior);
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

        public void CambiarFamiliaAnterior(string familiaAnterior)
        {
            if (!familiaAnterior.Equals("sin familia"))
            {
                switch (familiaAnterior)
                {
                    case "Ores":
                        btnOres.BorderBrush = null;
                        btnOres.BorderThickness = new Thickness(0);
                        break;
                    case "Corbat":
                        btnCorbat.BorderBrush = null;
                        btnCorbat.BorderThickness = new Thickness(0);
                        break;
                    case "Garlo":
                        btnGarlo.BorderBrush = null;
                        btnGarlo.BorderThickness = new Thickness(0);
                        break;
                    case "Ramfez":
                        btnRamfez.BorderBrush = null;
                        btnRamfez.BorderThickness = new Thickness(0);
                        break;
                }
            }
        }

        private void PonerFamiliasSeleccionadas()
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                List<string> familias = proxy.ObtenerFamiliaSeleccionada(salaNormal.idSala).ToList();

                foreach (string familia in familias)
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

        void ISalaCallback.ActualizarSalasActivas(Sala[] salasActivas)
        {
            throw new NotImplementedException();
        }

        public void SacarDeSalaATodosJugadores()
        {
            Inicio nuevaVentana = new Inicio(lblnombreUsuario.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }

        void IServicioJuegoTableroCallback.RecibirExpulsion(string jugadorObjetivo)
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.ActualizarInterfazExpulsion(string jugadorExpulsado)
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.ActualizarJugadorMuerto(string jugadorMuerto)
        {
            throw new NotImplementedException();
        }

        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }
    }

}