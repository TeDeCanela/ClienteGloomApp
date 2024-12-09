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
        
        ServicioGloom.Sala salaRegistrada = new ServicioGloom.Sala();
        bool persoanjeSeleciconado = false;
        string numeroDeSala;

        public SalaMiniJuego(String nombreUsuario, Sala sala)
        {
            InitializeComponent();
            lblNombreUsuarioRegistrado.Content = nombreUsuario;
            salaRegistrada = sala;
            lblInstruccion.Content = Properties.Resources.miniHistoriaNumeroSala + " :" + sala.idSala;
            lblInstruccionCodigo.Content = Properties.Resources.miniHisotriaInstruccionCodigo + " :" + sala.codigo;
            numeroDeSala = sala.idSala;
            btnEmpezar.BorderBrush = Brushes.Red;
            btnEmpezar.BorderThickness = new Thickness(4);
            try
            {
                ConectarConSala();
                ActualizarNumeroJugadores();
                PonerPersonajesUsados();
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
        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                if (ValidarAdministrador())
                {
                    proxy.SacarATodosLosJugadoresDeSala(numeroDeSala);
                }
                else
                {
                    proxy.SacarDeSala(numeroDeSala, lblNombreUsuarioRegistrado.Content.ToString());
                    Inicio nuevaVentana = new Inicio(lblNombreUsuarioRegistrado.Content.ToString());
                    nuevaVentana.Show();
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

        private void ConectarConSala()
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                proxy.ConectarConSala(numeroDeSala, lblNombreUsuarioRegistrado.Content.ToString());
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
        private void ActualizarJugadores()
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                proxy.ConectarConSala(numeroDeSala, lblNombreUsuarioRegistrado.Content.ToString());
                var listaJugadores = proxy.ObtenerJugadoresConectados(lblNombreUsuarioRegistrado.Content.ToString());
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
                ValidarSeleccionPersonaje();
                IngresarJugadorEnTablero();
                
                if (ValidarAdministrador())
                {
                    InstanceContext contextoSala = new InstanceContext(this);
                    ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

                    proxy.ValidarPersonajesSeleccionados(numeroDeSala, salaRegistrada.noJugadores);
                    proxy.CambiarEstadoParaPartida(numeroDeSala, "En partida");
                    proxy.EmpezarPartida(salaRegistrada.idSala);

                }
                btnEmpezar.BorderBrush = Brushes.Green;
                btnEmpezar.BorderThickness = new Thickness(4);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(Properties.Resources.mensajeExp19, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
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
            return salaRegistrada.idAdministrador.Equals(lblNombreUsuarioRegistrado.Content.ToString());
        }

        private void ValidarSeleccionPersonaje()
        {
            if (!persoanjeSeleciconado)
            {
                throw new InvalidOperationException("19");
            }
        }

        public void ActualizarNumeroJugadores()
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                var listaJugadores = proxy.ObtenerJugadoresConectados(numeroDeSala);
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
       
        private void IngresarJugadorEnTablero()
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxySala = new ServicioGloom.SalaClient(contextoSala);

                proxySala.IngresarJugadorAJuego(lblNombreUsuarioRegistrado.Content.ToString(), salaRegistrada.idSala, salaRegistrada.noJugadores);
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

        private void IngresarSeleccionPersonaje(String personaje)
        {
            try
            {
                persoanjeSeleciconado = true;
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

                proxy.SeleccionarPersonaje(lblNombreUsuarioRegistrado.Content.ToString(), personaje, numeroDeSala);
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

        void ISalaCallback.ActualizarImagenPersonaje(string personaje, string personajeAnterior)
        {
            CambiarPersonajeAnterior(personajeAnterior);
            switch (personaje)
            {
                case "Tucani":
                    btnTucani.BorderBrush = Brushes.Yellow;
                    btnTucani.BorderThickness = new Thickness(2);
                    break;
                case "Lusiel":
                    btnLusiel.BorderBrush = Brushes.Fuchsia;
                    btnLusiel.BorderThickness = new Thickness(2);
                    break;
                case "Angelus":
                    btnAngelus.BorderBrush = Brushes.Purple;
                    btnAngelus.BorderThickness = new Thickness(2);
                    break;
                case "Luan":
                    btnLuan.BorderBrush = Brushes.Blue;
                    btnLuan.BorderThickness = new Thickness(2);
                    break;
            }
        }

        private void PonerPersonajesUsados()
        {
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                List<string> personajes = proxy.ObtenerPersonajesUsados(numeroDeSala).ToList();

                foreach (var personaje in personajes)
                {
                    switch (personaje)
                    {
                        case "Tucani":
                            btnTucani.BorderBrush = Brushes.Red;
                            btnTucani.BorderThickness = new Thickness(2);
                            break;
                        case "Lusiel":
                            btnLusiel.BorderBrush = Brushes.Fuchsia;
                            btnLusiel.BorderThickness = new Thickness(2);
                            break;
                        case "Angelus":
                            btnAngelus.BorderBrush = Brushes.Green;
                            btnAngelus.BorderThickness = new Thickness(2);
                            break;
                        case "Luan":
                            btnLuan.BorderBrush = Brushes.Blue;
                            btnLuan.BorderThickness = new Thickness(2);
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

        public void CambiarPersonajeAnterior(string personajeAnterior)
        {
            if(!personajeAnterior.Equals("sin personaje"))
            {
                switch (personajeAnterior)
                {
                    case "Tucani":
                        btnTucani.BorderBrush = null;
                        btnTucani.BorderThickness = new Thickness(0);
                        break;
                    case "Lusiel":
                        btnLusiel.BorderBrush = null;
                        btnLusiel.BorderThickness = new Thickness(0);
                        break;
                    case "Angelus":
                        btnAngelus.BorderBrush = null;
                        btnAngelus.BorderThickness = new Thickness(0);
                        break;
                    case "Luan":
                        btnLuan.BorderBrush = null;
                        btnLuan.BorderThickness = new Thickness(0);
                        break;
                }
            }
        }

        public void SacarDeSalaATodosJugadores()
        {
            Inicio nuevaVentana = new Inicio(lblNombreUsuarioRegistrado.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }

        public void ActualizarSalasActivas(Sala[] salasActivas)
        {
            throw new NotImplementedException();
        }

        public void ResultadoUnirseASala(string idSala, string codigo, bool esExitoso)
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarSeleccionFamilia(string nombreFamilia, string nombreFamiliaAnterior)
        {
            throw new NotImplementedException();
        }

        private void BtnInvitarJugadores_Click(object sender, RoutedEventArgs e)
        {
            InvitacionJugador invitacionJugador = new InvitacionJugador(lblNombreUsuarioRegistrado.Content.ToString(), salaRegistrada.idSala);
            invitacionJugador.Show();
        }

        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }
    }
}
