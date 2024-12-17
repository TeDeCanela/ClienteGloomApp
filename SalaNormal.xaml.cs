using ClienteGloomApp.ServicioGloom;
using ServicioGlomm;
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
    public partial class SalaNormal : Window, IServicioSalaNormalCallback, ISalaCallback
    {
        ServicioGloom.Sala salaNormal = new ServicioGloom.Sala();
        bool familiaSeleccionada = false;
        string numeroSala;
        
        public SalaNormal(String nombreUsuario, Sala sala)
        {

            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            lblnombreUsuario.Content = nombreUsuario;
            salaNormal = sala;
            numeroSala = sala.idSala;
            lblInstruccionCodigo.Content = Properties.Resources.miniHisotriaInstruccionCodigo + " :" + sala.codigo;

            if (sala.tipoPartida.Trim() == "Privada")
            {
                btnInvitarJugadores.Visibility = Visibility.Visible;
            }

            try
            {
                ConectarConSala(); ConectarConSalaNormal();
                PonerFamiliasSeleccionadas();

                if (!ValidarAdministrador())
                {
                    btnEmpezar.Content = Properties.Resources.salaBtnListo;
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

        private bool ValidarAdministrador()
        {
            return salaNormal.idAdministrador.Equals(lblnombreUsuario.Content.ToString());
        }

        private void ConectarConSala()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                proxy.ConectarConSala(salaNormal.idSala, lblnombreUsuario.Content.ToString());
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

        private void ConectarConSalaNormal()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.ServicioSalaNormalClient proxy = new ServicioGloom.ServicioSalaNormalClient(contextoSala);
                proxy.ConectarConSalaNormal(numeroSala, lblnombreUsuario.Content.ToString());
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
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
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
                MessageBox.Show(Properties.Resources.mensajeExp19, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
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
                DirigirJugadorInicioDeSesion();
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


        private void ValidarSeleccionFamilia()
        {
            if (!familiaSeleccionada)
            {
                throw new InvalidOperationException("31");
            }
        }

        private void IngresarJugadorEnTablero()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxySala = new ServicioGloom.SalaClient(contextoSala);

                proxySala.IngresarJugadorAJuego(lblnombreUsuario.Content.ToString(), salaNormal.idSala, salaNormal.noJugadores);
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

        private void BtnInvitarJugadores_Click(object sender, RoutedEventArgs e)
        {
            InvitacionJugador invitacionJugador = new InvitacionJugador(lblnombreUsuario.Content.ToString(), salaNormal.idSala);
            invitacionJugador.Show();
        }

        private void IngresarSeleccionFamilia(string familia)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
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
                administradorLogger.RegistroError(ex);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
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
            btnEmpezar.BorderBrush = Brushes.Red;
            btnEmpezar.BorderThickness = new Thickness(4);
        }






        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
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
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
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
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                InstanceContext contextoSala = new InstanceContext(this);
                ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
                List<string> familias = proxy.ObtenerFamiliaSeleccionada(numeroSala).ToList();

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
                administradorLogger.RegistroError(ex);
                DirigirJugadorInicioDeSesion();
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
      
        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }

        void IServicioSalaNormalCallback.ActualizarSeleccionFamilia(string nombreFamilia, string nombreFamiliaAnterior)
        {
            CambiarFamiliaAnterior(nombreFamiliaAnterior);
            switch (nombreFamilia)
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

        void ISalaCallback.EmpezarJuego()
        {
            PartidaNormal nuevaVentana = new PartidaNormal(lblnombreUsuario.Content.ToString(), salaNormal);
            nuevaVentana.Show();
            this.Close();
        }

        void ISalaCallback.ActualizarNumeroJugadores()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
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

        void ISalaCallback.SacarDeSalaATodosJugadores()
        {
            Inicio nuevaVentana = new Inicio(lblnombreUsuario.Content.ToString());
            nuevaVentana.Show();
            this.Close();
        }
    }

}