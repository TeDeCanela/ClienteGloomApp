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

namespace ClienteGloomApp
{
    /// <summary>
    /// Lógica de interacción para FinPartidaNormal.xaml
    /// </summary>
    public partial class FinPartidaNormal : Window, ICreacionPartidaCallback, IServicioJuegoTableroCallback
    {
        private string jugadorPropietario;
        private string identificadorSala;

        public FinPartidaNormal(string nombreUsuario, string ganador, string idSala)
        {
            InitializeComponent();
            jugadorPropietario = nombreUsuario;
            identificadorSala = idSala;

            lblJugador1.Content = $"Ganador: {ganador}";
            AsignarJugadores();
        }


        private void AsignarJugadores()
        {
            InstanceContext contextoTableroJuego = new InstanceContext(this);
            ServicioGloom.ServicioJuegoTableroClient proxy = new ServicioGloom.ServicioJuegoTableroClient(contextoTableroJuego);

            var resumenFamilias = proxy.ObtenerResumenFamiliasPorSala(identificadorSala);
            var rutaImagenesPorFamilia = new Dictionary<string, string>
    {
        { "Ores", "Imagenes/EscudoOres.jpg" },
        { "Corbat", "Imagenes/EscudoCorbat.jpg" },
        { "Garlo", "Imagenes/EscudoGarlo.jpg" },
        { "Ramfez", "Imagenes/EscudoRamfez.jpg" }
    };

            var labels = new[] { lblJugador1, lblJugador2, lblJugador3, lblJugador4 };
            var images = new[] { imgFamilia1, imgFamilia2, imgFamilia3, imgFamilia4 };

            int index = 0;
            foreach (var jugador in resumenFamilias)
            {
                if (index >= labels.Length) break;

                string nombreJugador = jugador.Key;
                string familia = jugador.Value.Item1; 
                int vidaTotal = jugador.Value.Item2; 

                labels[index].Content = $"Jugador: {nombreJugador}, Familia: {familia}, Vida Total: {vidaTotal}";
                if (rutaImagenesPorFamilia.TryGetValue(familia, out var rutaImagen))
                {
                    images[index].Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                }

                index++;
            }
        }

        void IServicioJuegoTableroCallback.EnviarGanador(string jugador)
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.EnviarTurno(string nombreDelUsuarioEnTurno)
        {
            throw new NotImplementedException();
        }

        void ICreacionPartidaCallback.NotificarPartidaCreada(string mensaje)
        {
            throw new NotImplementedException();
        }


        void IServicioJuegoTableroCallback.NotificarVotacionExpulsion(string jugadorPropuesto)
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.RecibirExpulsion(string jugadorObjetivo)
        {
            throw new NotImplementedException();
        }


        void IServicioJuegoTableroCallback.ActualizarImagenMazoCartaBonus()
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.ActualizarImagenMazoCartaSobrante()
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.ActualizarInterfazExpulsion(string jugadorExpulsado)
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.ActualizarMazoJugador()
        {
            throw new NotImplementedException();
        }

        void IServicioJuegoTableroCallback.ActualizarTurno(string nombreDelUsuarioEnTurno)
        {
            throw new NotImplementedException();
        }

        private void BtnFlecha_Click(object sender, RoutedEventArgs e)
        {
            
                Inicio nuevaVentana = new Inicio(jugadorPropietario);
                nuevaVentana.Show();
                this.Close();
            
        }
    }
}
    
