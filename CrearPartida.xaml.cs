using ClienteGloomApp.ServicioGloom;
using ServicioGlomm;
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
    /// Lógica de interacción para CrearPartida.xaml
    /// </summary>
    public partial class CrearPartida : Window
    {
        private string identificadorUsuario;
        private string tipoSalaSeleccionada; 
        private int numeroJugadoresSeleccionado; 
        private string tipoPartidaSeleccionada;
        ValidacionCampos validar = new ValidacionCampos();
        public CrearPartida(String nombreUsuario)
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            identificadorUsuario = nombreUsuario;
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            InstanceContext contextoPartida = new InstanceContext(this);

            ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient();

            ServicioGloom.Sala sala = new ServicioGloom.Sala();

                try
                {
                sala.nombreSala = validar.VerificarNombrePartida(txtNombreSala.Text);
                sala.tipoSala = tipoSalaSeleccionada;
                sala.tipoPartida = tipoPartidaSeleccionada;
                sala.noJugadores = numeroJugadoresSeleccionado;
                sala.idAdministrador = identificadorUsuario;
                sala.ganador = "Sin ganador";
                sala.fecha = ObtenerFecha();
                if (tipoSalaSeleccionada == "Mini historia" && tipoPartidaSeleccionada == "Pública")
                    {
                        MessageBox.Show(Properties.Resources.mensajeTipoPartidaNoPermitida, Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        int resultadoOperacion = proxy.CrearPartida(sala);
                        if (resultadoOperacion == 1)
                        {
                            MessageBox.Show(Properties.Resources.mensajePartidaCreadaExitosa, Properties.Resources.mensajeTituloInformacion, MessageBoxButton.OK, MessageBoxImage.Information);
                            if (tipoSalaSeleccionada == "Mini historia")
                            {
                                CambiarASalaMiniJuego();
                            }
                            else
                            {
                                CambiarASalaNormal();
                            }
                        }
                    }
                }
            catch (ArgumentException ex)
            {
                administradorLogger.RegistroError(ex);
                MensajesEmergentes.MostrarMensajeAdvertencia(ex.Message, ex.Message);
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


        private string ObtenerFecha()
        {
            System.DateTime datoFecha = DateTime.Now;

            string fechaFormato = datoFecha.ToString("dd/MM/yy");

            return fechaFormato;
        }

        

        void CambiarASalaNormal()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                string codigoSalaNormal = ObtenerCodigoDeSala(identificadorUsuario, txtNombreSala.Text);
                InstanceContext contexto = new InstanceContext(this);
                ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient();

                var salaNormal = proxy.BuscarSalaExistente(txtNombreSala.Text, codigoSalaNormal);


                SalaNormal sala = new SalaNormal(identificadorUsuario, salaNormal);
                sala.Show();
                this.Close();
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

        void CambiarASalaMiniJuego()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                string codigoSalaMini = ObtenerCodigoDeSala(identificadorUsuario, txtNombreSala.Text);

                InstanceContext contexto = new InstanceContext(this);
                ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient();

                var salaMini = proxy.BuscarSalaExistente(txtNombreSala.Text, codigoSalaMini);
                SalaMiniJuego sala = new SalaMiniJuego(identificadorUsuario, salaMini);
                sala.Show();
                this.Close();
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

        private string ObtenerCodigoDeSala(string usuarioAdminsitrador, string nombreSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            string codigo = "";
            try
            {
                InstanceContext contexto = new InstanceContext(this);
                ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient();
                codigo = proxy.ObtenerCodigoSala(usuarioAdminsitrador, nombreSala);
                
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
            return codigo;
        }



        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            Inicio nuevaVentana = new Inicio(identificadorUsuario);
            nuevaVentana.Show();
            this.Close();
        }



        private void BtnSalaNormal_Click(object sender, RoutedEventArgs e)
        {

            tipoSalaSeleccionada = "Normal";

            btnSalaNormal.BorderBrush = Brushes.Purple;
            btnSalaNormal.BorderThickness = new Thickness(3);


            btnSalaMiniHistoria.BorderBrush = Brushes.Transparent;
            btnSalaMiniHistoria.BorderThickness = new Thickness(0);

        }

        private void BtnSalaMiniHistoria_Click(object sender, RoutedEventArgs e)
        {
            tipoSalaSeleccionada = "Mini historia";

            btnSalaMiniHistoria.BorderBrush = Brushes.Aqua;
            btnSalaMiniHistoria.BorderThickness = new Thickness(3);

            btnSalaNormal.BorderBrush = Brushes.Transparent;
            btnSalaNormal.BorderThickness = new Thickness(0);
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            ReestablecerBordeBotones();

            if (sender is Button botonSeleccionado)
            {
                botonSeleccionado.BorderBrush = Brushes.Orange;
                botonSeleccionado.BorderThickness = new Thickness(3);

                if (int.TryParse(botonSeleccionado.Content.ToString(), out int numero))
                {
                    numeroJugadoresSeleccionado = numero;
                }


            }
        }

        private void ReestablecerBordeBotones()
        {
            btnNo2.BorderBrush = Brushes.Transparent;
            btnNo2.BorderThickness = new Thickness(0);

            btnNo3.BorderBrush = Brushes.Transparent;
            btnNo3.BorderThickness = new Thickness(0);

            btnNo4.BorderBrush = Brushes.Transparent;
            btnNo4.BorderThickness = new Thickness(0);
        }

        private void BtnPartidaPublica_Click(object sender, RoutedEventArgs e)
        {
            tipoPartidaSeleccionada = "Pública";

            btnPartidaPublica.BorderBrush = Brushes.GreenYellow;
            btnPartidaPublica.BorderThickness = new Thickness(3);

            btnPartidaPrivada.BorderBrush = Brushes.Transparent;
            btnPartidaPrivada.BorderThickness = new Thickness(0);
        }

        private void BtnPartidaPrivada_Click(object sender, RoutedEventArgs e)
        {
            tipoPartidaSeleccionada = "Privada";

            btnPartidaPrivada.BorderBrush = Brushes.HotPink;
            btnPartidaPrivada.BorderThickness = new Thickness(3);

            btnPartidaPublica.BorderBrush = Brushes.Transparent;
            btnPartidaPublica.BorderThickness = new Thickness(0);
        }
        private void DirigirJugadorInicioDeSesion()
        {
            InicioSesion nuevaVentana = new InicioSesion();
            nuevaVentana.Show();
            this.Close();
        }

    }
    
}


