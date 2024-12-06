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
    /// Lógica de interacción para CrearPartida.xaml
    /// </summary>
    public partial class CrearPartida : Window, ISalaCallback, ICreacionPartidaCallback
    {
        private string identificadorUsuario;
        private string tipoSalaSeleccionada; 
        private int numeroJugadoresSeleccionado; 
        private string tipoPartidaSeleccionada;
        public CrearPartida(String nombreUsuario)
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            identificadorUsuario = nombreUsuario;
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext contextoPartida = new InstanceContext(this);

            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoPartida);

            ServicioGloom.Sala sala = new ServicioGloom.Sala();

            string nombrePartida = txtNombreSala.Text;

            if (ContieneCaracteresEspeciales(nombrePartida))
            {
                MessageBox.Show("El campo de texto contiene caracteres especiales no permitidos.");
                txtNombreSala.Clear();
            }
            else
            {
                sala.nombreSala = txtNombreSala.Text;
                sala.tipoSala = tipoSalaSeleccionada;
                sala.tipoPartida = tipoPartidaSeleccionada;
                sala.noJugadores = numeroJugadoresSeleccionado;
                sala.idAdministrador = identificadorUsuario;
                sala.ganador = "Sin ganador";
                sala.fecha = ObtenerFecha();

                try
                {
                    if (tipoSalaSeleccionada == "Mini historia" && tipoPartidaSeleccionada == "Pública")
                    {
                        MessageBox.Show(Properties.Resources.mensajeTipoPartidaNoPermitida, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        int resultadoOperacion = proxy.CrearPartida(sala);
                        if (resultadoOperacion == 1)
                        {
                            MessageBox.Show(Properties.Resources.mensajePartidaCreadaExitosa, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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
                catch (FaultException<ManejadorExcepciones> ex)
                {
                    MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
                }
            }
        }


        private bool ContieneCaracteresEspeciales(string texto)
        {
            string patron = @"[@?#{\[\]""'¿-]";  

            return Regex.IsMatch(texto, patron);
        }

        private string ObtenerFecha()
        {
            System.DateTime datoFecha = DateTime.Now;

            string fechaFormato = datoFecha.ToString("dd/MM/yy");

            return fechaFormato;
        }

        

        void CambiarASalaNormal()
        {
            string codigoSalaNormal = ObtenerCodigoDeSala(identificadorUsuario, txtNombreSala.Text);

            InstanceContext contexto = new InstanceContext(this);
            ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient(contexto);

            var salaNormal = proxy.BuscarSalaExistente(codigoSalaNormal, codigoSalaNormal);

            
            SalaNormal sala = new SalaNormal(identificadorUsuario, salaNormal);
            sala.Show();
            this.Close();
        }

        void CambiarASalaMiniJuego()
        {
            string codigoSalaMini = ObtenerCodigoDeSala(identificadorUsuario, txtNombreSala.Text);

            InstanceContext contexto = new InstanceContext(this);
            ServicioGloom.CreacionPartidaClient proxy = new ServicioGloom.CreacionPartidaClient(contexto);

            var salaMini = proxy.BuscarSalaExistente(codigoSalaMini, codigoSalaMini);
            SalaMiniJuego sala = new SalaMiniJuego(identificadorUsuario, salaMini);
            sala.Show();
            this.Close();
        }

        private string ObtenerCodigoDeSala(string usuarioAdminsitrador, string nombreSala)
        {
            InstanceContext contexto = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contexto);
            string codigo = proxy.ObtenerCodigoSala(usuarioAdminsitrador, nombreSala);
            return codigo;
        }


        void ISalaCallback.EmpezarJuego()
        {
            // Implementación del método
        }

        void ISalaCallback.ActualizarNumeroJugadores()
        {
            // Implementación del método
        }

        void ISalaCallback.ActualizarImagenPersonaje(string personaje, string imagen)
        {
            // Implementación del método
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
        }

        private void BtnSalaMiniHistoria_Click(object sender, RoutedEventArgs e)
        {
            tipoSalaSeleccionada = "Mini historia";
        }

        private void BtnNo2_Click(object sender, RoutedEventArgs e)
        {
            numeroJugadoresSeleccionado = 2;
        }

        private void BtnNo3_Click(object sender, RoutedEventArgs e)
        {
            numeroJugadoresSeleccionado = 3;
        }

        private void BtnNo4_Click(object sender, RoutedEventArgs e)
        {
            numeroJugadoresSeleccionado = 4;
        }

        private void BtnPartidaPublica_Click(object sender, RoutedEventArgs e)
        {
            tipoPartidaSeleccionada = "Pública";
        }

        private void BtnPartidaPrivada_Click(object sender, RoutedEventArgs e)
        {
            tipoPartidaSeleccionada = "Privada";
        }

        void ISalaCallback.SacarDeSalaATodosJugadores()
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarSalasActivas(Sala[] salasActivas)
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ResultadoUnirseASala(string idSala, string codigo, bool esExitoso)
        {
            throw new NotImplementedException();
        }

        void ISalaCallback.ActualizarSeleccionFamilia(string nombreUsuario, string nombreFamilia)
        {
            throw new NotImplementedException();
        }

        void ICreacionPartidaCallback.NotificarPartidaCreada(string mensaje)
        {
            throw new NotImplementedException();
        }
    }
    
}


