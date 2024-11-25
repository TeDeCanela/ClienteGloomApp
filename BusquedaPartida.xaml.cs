using ClienteGloomApp.ServicioGloom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ClienteGloomApp
{
    /// <summary>
    /// Lógica de interacción para BusquedaPartida.xaml
    /// </summary>
    public partial class BusquedaPartida : Window
    {
        /*
        private SalaClient servicio;
        private String identificadorUsuario;

        public ObservableCollection<Sala> salasActivas { get; set; }
        public BusquedaPartida(String nombreUsuario)
        {
            InitializeComponent();

            identificadorUsuario = nombreUsuario;

            // Inicializa la colección observable y enlaza el DataGrid
            salasActivas = new ObservableCollection<Sala>();
            tblSalas.ItemsSource = salasActivas;

            // Inicializa el cliente del servicio con InstanceContext para habilitar callbacks
            InstanceContext context = new InstanceContext(this);
            servicio = new SalaClient(context);

            // Cargar las salas activas al iniciar
            CargarSalasActivas();
        }

        private async void CargarSalasActivas()
        {
            try
            {
                var salas = await ObtenerSalasDesdeServicio();
                ActualizarSalasActivas(salas);
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar las salas activas", ex);
            }
        }

        // Método para obtener las salas activas desde el servicio
        private async Task<List<Sala>> ObtenerSalasDesdeServicio()
        {
            return await servicio.ObtenerSalasActivasAsync();
        }

        // Método para actualizar la colección de salas activas en la UI
        private void ActualizarSalasActivas(List<Sala> salas)
        {
            salasActivas.Clear();

            foreach (var sala in salas)
            {
                salasActivas.Add(sala);
            }
        }


        private void btnUnirseAPartida_Click(object sender, RoutedEventArgs e)
        {
            var salaSeleccionada = (Sala)tblSalas.SelectedItem;
            if (salaSeleccionada == null)
            {
                MessageBox.Show("Por favor, selecciona una sala.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string idSala = salaSeleccionada.idSala;

                if (salaSeleccionada.tipoPartida == "Publica")
                {
                    Task servicio.UnirseASalaPublicaAsync(idSala, identificadorUsuario);
                    MessageBox.Show("Te has unido a la sala pública correctamente.");
                }
                else if (salaSeleccionada.tipoPartida == "Privada")
                {
                    string codigoAcceso = PromptCodigoAcceso(); //ventana para ingresar el numero de sala y codigo
                    if (!string.IsNullOrEmpty(codigoAcceso))
                    {
                        Task _ervicio.UnirseASalaPrivadaAsync(identificadorUsuario, idSala, codigoAcceso);
                        MessageBox.Show("Te has unido a la sala privada correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("Código de acceso no proporcionado.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al unirse a la sala: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }

  */      
    }
}
