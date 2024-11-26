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
            InitializeComponent();
            identificadorUsuario = nombreUsuario;
            salasActivas = new ObservableCollection<Sala>();
            tblSalas.ItemsSource = salasActivas;

            // Asegúrate de que la clase implemente ISalaCallback y pasa InstanceContext con "this"
            InstanceContext context = new InstanceContext(this);
            servicio = new SalaClient(context);

            CargarSalasActivas();
        }

        private void CargarSalasActivas()
        {
            try
            {
                var salasDesdeServicio = servicio.ObtenerSalasActivas();
                salasActivas.Clear();

                if (salasDesdeServicio == null || salasDesdeServicio.Length == 0)
                {
                    MessageBox.Show("No se encontraron salas activas.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                foreach (var sala in salasDesdeServicio)
                {
                    salasActivas.Add(sala);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las salas activas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUnirseAPartida_Click(object sender, RoutedEventArgs e)
        {
            salaSeleccionada = (Sala)tblSalas.SelectedItem;
            if (salaSeleccionada == null)
            {
                MessageBox.Show("Por favor, selecciona una sala.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string idSala = salaSeleccionada.idSala;
            try
            {
                if (salaSeleccionada.tipoPartida == "Pública" && salaSeleccionada.tipoSala == "Normal")
                {
                    servicio.UnirseASalaPublicaNormalAsync(idSala, identificadorUsuario);
                    MessageBox.Show("Te has unido a la sala pública correctamente.");
                    AbrirVentanaSalaNormal(idSala, identificadorUsuario, salaSeleccionada);
                }
                else if (salaSeleccionada.tipoPartida == "Privada")
                {
                    panelCodigoAcceso.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al unirse a la sala: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnConfirmarCodigo_Click(object sender, RoutedEventArgs e)
        {
            string codigoAcceso = txtCodigoAcceso.Text;

            if (string.IsNullOrEmpty(codigoAcceso))
            {
                MessageBox.Show("Código de acceso no proporcionado.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string idSala = salaSeleccionada.idSala;
            try
            {
                if (salaSeleccionada.tipoSala == "Normal")
                {
                    servicio.UnirseASalaPrivadaNormalAsync(identificadorUsuario, idSala, codigoAcceso);
                    MessageBox.Show("Te has unido a la sala privada correctamente.");
                    AbrirVentanaSalaNormal(idSala, identificadorUsuario, salaSeleccionada);
                }
                else if (salaSeleccionada.tipoSala == "Mini historia")
                {
                    servicio.UnirseASalaPrivadaMiniHistoriaAsync(identificadorUsuario, idSala, codigoAcceso);
                    MessageBox.Show("Te has unido a la sala privada correctamente.");
                    AbrirVentanaSalaMiniHistoria(identificadorUsuario, salaSeleccionada);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al unirse a la sala: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        // Implementación de los métodos de ISalaCallback
        public void ActualizarSalasActivas(List<Sala> salasActualizadas)
        {
            // Actualiza las salas activas desde el callback
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
                MessageBox.Show("Te has unido a la sala correctamente.");
            }
            else
            {
                MessageBox.Show("No se pudo unir a la sala. Verifica el código de acceso.");
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

        void ISalaCallback.ActualizarSeleccionFamilia(string nombreUsuario, string nombreFamilia)
        {
            throw new NotImplementedException();
        }
    }
}