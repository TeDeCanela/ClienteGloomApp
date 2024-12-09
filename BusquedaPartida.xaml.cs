using ClienteGloomApp.ServicioGloom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;

namespace ClienteGloomApp
{
    public partial class BusquedaPartida : Window, ISalaCallback
    {
        
        private SalaClient servicio;
        private string identificadorUsuario;
        private Sala salaSeleccionada;

        public ObservableCollection<Sala> salasActivas { get; set; }

        public BusquedaPartida(string nombreUsuario)
        {
            try
            {
                InitializeComponent();
                identificadorUsuario = nombreUsuario;
                salasActivas = new ObservableCollection<Sala>();
                tblSalas.ItemsSource = salasActivas;

                InstanceContext context = new InstanceContext(this);
                servicio = new SalaClient(context);

                CargarSalasActivas();
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

        private void CargarSalasActivas()
        {
            try
            {
                var salasDesdeServicio = servicio.ObtenerSalasActivas();
                salasActivas.Clear();

                if (salasDesdeServicio == null || salasDesdeServicio.Length == 0)
                {
                    MessageBox.Show("60", Properties.Resources.mensajeTituloInformacion , MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                foreach (var sala in salasDesdeServicio)
                {
                    salasActivas.Add(sala);
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

        private void BtnUnirseAPartida_Click(object sender, RoutedEventArgs e)
        {
            salaSeleccionada = (Sala)tblSalas.SelectedItem;
            if (salaSeleccionada == null)
            {
                MessageBox.Show("61", Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string idSala = salaSeleccionada.idSala;
            try
            {
                if (salaSeleccionada.tipoPartida == "Pública" && salaSeleccionada.tipoSala == "Normal")
                {
                    servicio.UnirseASalaPublicaNormalAsync(idSala, identificadorUsuario);
                    MessageBox.Show("63");
                    AbrirVentanaSalaNormal(idSala, identificadorUsuario, salaSeleccionada);
                }
                else if (salaSeleccionada.tipoPartida == "Privada")
                {
                    panelCodigoAcceso.Visibility = Visibility.Visible;
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

        private void BtnConfirmarCodigo_Click(object sender, RoutedEventArgs e)
        {
            string codigoAcceso = txtCodigoAcceso.Text;

            if (string.IsNullOrEmpty(codigoAcceso))
            {
                MessageBox.Show("64", Properties.Resources.mensajeTituloAdvertencia, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string idSala = salaSeleccionada.idSala;
            try
            {
                if (salaSeleccionada.tipoSala == "Normal")
                {
                    servicio.UnirseASalaPrivadaNormalAsync(identificadorUsuario, idSala, codigoAcceso);
                    MessageBox.Show("65");
                    AbrirVentanaSalaNormal(idSala, identificadorUsuario, salaSeleccionada);
                }
                else if (salaSeleccionada.tipoSala == "Mini historia")
                {
                    servicio.UnirseASalaPrivadaMiniHistoriaAsync(identificadorUsuario, idSala, codigoAcceso);
                    MessageBox.Show("65");
                    AbrirVentanaSalaMiniHistoria(identificadorUsuario, salaSeleccionada);
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Detail.codigo, ex.Detail.mensaje);
            }
            catch (EndpointNotFoundException ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
            }
            catch (TimeoutException ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
                DirigirJugadorInicioDeSesion();
            }
            catch (CommunicationException ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
            }
            catch (Exception ex)
            {
                MensajesEmergentes.MostrarMensaje(ex.Message, ex.Message);
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

        public void ActualizarSalasActivas(List<Sala> salasActualizadas)
        {
            this.salasActivas.Clear();
            foreach (var sala in salasActualizadas)
            {
                this.salasActivas.Add(sala);
            }
        }

        public void ResultadoUnirseASala(string idSala, string codigo, bool esExitoso)
        {
            if (esExitoso)
            {
                MessageBox.Show("66");
            }
            else
            {
                MessageBox.Show("67");
            }
        }

        void ISalaCallback.EmpezarJuego()
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarNumeroJugadores()
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarImagenPersonaje(string personaje, string personajeAnterior)
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarSalasActivas(Sala[] salasActualizadas)
        {
            this.salasActivas.Clear();
            foreach (var sala in salasActualizadas)
            {
                this.salasActivas.Add(sala);
            }
        }

        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            Inicio nuevaVentana = new Inicio(identificadorUsuario);
            nuevaVentana.Show();
            this.Close();
        }

        

        void ISalaCallback.SacarDeSalaATodosJugadores()
        {
            throw new NotImplementedException();
        }


        void ISalaCallback.ActualizarSeleccionFamilia(string nombreUsuario, string nombreFamilia)
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