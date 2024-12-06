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
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
           


        }
        private void btnFlecha_Click(object sender, RoutedEventArgs e)
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

        private void ConectarConSala()
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            proxy.ConectarConSala(numeroDeSala, lblNombreUsuarioRegistrado.Content.ToString());
        }
        private void ActualizarJugadores()
        {
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            proxy.ConectarConSala(numeroDeSala,lblNombreUsuarioRegistrado.Content.ToString());
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
                ValidarSeleccionPersonaje();
                IngresarJugadorEnTablero();
                
                if (ValidarAdministrador())
                {
                    InstanceContext contextoSala = new InstanceContext(this);
                    ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);

                    proxy.ValidarPersonajesSeleccionados(numeroDeSala, salaRegistrada.noJugadores);
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
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
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
            
            InstanceContext contextoSala = new InstanceContext(this);
            ServicioGloom.SalaClient proxy = new ServicioGloom.SalaClient(contextoSala);
            var listaJugadores = proxy.ObtenerJugadoresConectados(numeroDeSala);
            txtJugador1.Text = listaJugadores.Count() > 0 ? listaJugadores[0] : string.Empty;
            txtJugador2.Text = listaJugadores.Count() > 1 ? listaJugadores[1] : string.Empty;
            txtJugador3.Text = listaJugadores.Count() > 2 ? listaJugadores[2] : string.Empty;
            txtJugador4.Text = listaJugadores.Count() > 3 ? listaJugadores[3] : string.Empty;
            
        }
       
        private void IngresarJugadorEnTablero()
        {
            InstanceContext contextoSala= new InstanceContext(this);
            ServicioGloom.SalaClient proxySala = new ServicioGloom.SalaClient(contextoSala);

            proxySala.IngresarJugadorAJuego(lblNombreUsuarioRegistrado.Content.ToString(), salaRegistrada.idSala, salaRegistrada.noJugadores);
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
                MensajesEmergentes.MostrarMensaje(ex.Detail.mensaje, ex.Detail.mensaje);
            }
            btnEmpezar.BorderBrush = Brushes.Red;
            btnEmpezar.BorderThickness = new Thickness(4);
        }

        public void ActualizarImagenPersonaje(string personaje, string personajeAnterior)
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
        //ToDO
        public void ActualizarSalasActivas(Sala[] salasActivas)
        {
            throw new NotImplementedException();
        }

        public void ResultadoUnirseASala(string idSala, string codigo, bool esExitoso)
        {
            throw new NotImplementedException();
        }
    }
}
