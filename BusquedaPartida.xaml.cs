using ClienteGloomApp.ServicioGloom;
using ServicioGlomm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;

namespace ClienteGloomApp
{
    public partial class BusquedaPartida : Window, IServicioBusquedaPartidaCallback
    {

        private ServicioBusquedaPartidaClient servicio;
        private string identificadorUsuario;
        private Sala salaSeleccionada;


        public ObservableCollection<Sala> salasActivas { get; set; }

        public BusquedaPartida(string nombreUsuario)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {

                InitializeComponent();
                identificadorUsuario = nombreUsuario;
                salasActivas = new ObservableCollection<Sala>();
                tblSalas.ItemsSource = salasActivas;

                InstanceContext context = new InstanceContext(this);
                servicio = new ServicioBusquedaPartidaClient(context);

                CargarSalasActivas();
            }
            catch (EndpointNotFoundException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
            }
            catch (TimeoutException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
            }
            catch (Exception ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
            }

        }

        private void CargarSalasActivas()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                var salasDesdeServicio = servicio.ObtenerSalasActivas();
                salasActivas.Clear();

                if (salasDesdeServicio == null || salasDesdeServicio.Length == 0)
                {
                    MessageBox.Show("60", Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                foreach (var sala in salasDesdeServicio)
                {
                    if (!sala.tipoSala.Equals("Mini historia"))
                    {
                        salasActivas.Add(sala);
                    }
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
            }
            catch (EndpointNotFoundException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
            }
            catch (TimeoutException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
            }
            catch (Exception ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
            }
        }

        private void BtnUnirseAPartida_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            salaSeleccionada = (Sala)tblSalas.SelectedItem;
            if (salaSeleccionada == null)
            {
                MessageBox.Show("61", Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string idSala = salaSeleccionada.idSala;
            try
            {

                if (salaSeleccionada.tipoPartida.Trim() == "Pública" && salaSeleccionada.tipoSala == "Normal")
                {
                    servicio.UnirseASalaPublicaNormal(idSala, identificadorUsuario);
                    MessageBox.Show(Properties.Resources.mensajeExp63);
                    AbrirVentanaSalaNormal(idSala, identificadorUsuario, salaSeleccionada);
                }
                else if (salaSeleccionada.tipoPartida.Trim() == "Privada")
                {
                    panelCodigoAcceso.Visibility = Visibility.Visible;
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
            }
            catch (EndpointNotFoundException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
            }
            catch (TimeoutException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
            }
            catch (Exception ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
            }
        }

        private void BtnConfirmarCodigo_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            string codigoAcceso = txtCodigoAcceso.Text;

            if (string.IsNullOrEmpty(codigoAcceso))
            {
                MessageBox.Show(Properties.Resources.mensajeExp64, Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string idSala = salaSeleccionada.idSala;
            try
            {
                if (salaSeleccionada.tipoSala == "Normal")
                {
                    servicio.UnirseASalaPrivadaNormal(identificadorUsuario, idSala, codigoAcceso);
                    MessageBox.Show(Properties.Resources.mensajeExp65);
                    AbrirVentanaSalaNormal(idSala, identificadorUsuario, salaSeleccionada);
                }

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
            }
            catch (EndpointNotFoundException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("58", ex.Message);
            }
            catch (TimeoutException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("59", ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("16", ex.Message);
            }
            catch (Exception ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensaje("60", ex.Message);
            }
        }

        private void AbrirVentanaSalaNormal(string idSala, string nombreUsuario, Sala sala)
        {
            SalaNormal ventanaSala = new SalaNormal(nombreUsuario, sala);
            ventanaSala.Show();
            this.Close();
        }

        private void AbrirVentanaSalaMiniHistoria(string nombreUsuario, Sala sala)
        {
            SalaMiniJuego ventanaSala = new SalaMiniJuego(nombreUsuario, sala);
            ventanaSala.Show();
            this.Close();
        }

        public void ResultadoUnirseASala(string idSala, string codigo, bool esExitoso)
        {
            if (esExitoso)
            {
                MessageBox.Show(Properties.Resources.mensajeExp66);
            }
            else
            {
                MessageBox.Show(Properties.Resources.mensajeExp67);
            }
        }



        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            Inicio nuevaVentana = new Inicio(identificadorUsuario);
            nuevaVentana.Show();
            this.Close();
        }




        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }

        void IServicioBusquedaPartidaCallback.ActualizarSalasActivas(Sala[] salasActivas)
        {
            this.salasActivas.Clear();
            foreach (var sala in salasActivas)
            {
                this.salasActivas.Add(sala);
            }
        }
    }
}