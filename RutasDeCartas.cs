using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteGloomApp
{
    public static class RutasDeCartas
    {
        public static readonly Dictionary<string, string> RutasImagenesPorIdentificador = new Dictionary<string, string>
        {
            { "Accidente1.png", "pack://application:,,,/ImagenCarta/Accidente1.png" },
            { "Accidente2.png", "pack://application:,,,/ImagenCarta/Accidente2.png" },
            { "Accidente3.png", "pack://application:,,,/ImagenCarta/Accidente3.png" },
            { "Accidente4.png", "pack://application:,,,/ImagenCarta/Accidente4.png" },
            { "Accidente5.png", "pack://application:,,,/ImagenCarta/Accidente5.png" },
            { "Accidente6.png", "pack://application:,,,/ImagenCarta/Accidente6.png" },
            { "Accidente7.png", "pack://application:,,,/ImagenCarta/Accidente7.png" },
            { "Desamor1.png", "pack://application:,,,/ImagenCarta/Desamor1.png" },
            { "Desamor2.png", "pack://application:,,,/ImagenCarta/Desamor2.png" },
            { "Desamor3.png", "pack://application:,,,/ImagenCarta/Desamor3.png" },
            { "Desamor4.png", "pack://application:,,,/ImagenCarta/Desamor4.png" },
            { "Desamor5.png", "pack://application:,,,/ImagenCarta/Desamor5.png" },
            { "Desamor6.png", "pack://application:,,,/ImagenCarta/Desamor6.png" },
            { "Desamor7.png", "pack://application:,,,/ImagenCarta/Desamor7.png" },
            { "Desgracia1.png", "pack://application:,,,/ImagenCarta/Desgracia1.png" },
            { "Desgracia2.png", "pack://application:,,,/ImagenCarta/Desgracia2.png" },
            { "Desgracia3.png", "pack://application:,,,/ImagenCarta/Desgracia3.png" },
            { "Desgracia4.png", "pack://application:,,,/ImagenCarta/Desgracia4.png" },
            { "Desgracia5.png", "pack://application:,,,/ImagenCarta/Desgracia5.png" },
            { "Desgracia6.png", "pack://application:,,,/ImagenCarta/Desgracia6.png" },
            { "Desgracia7.png", "pack://application:,,,/ImagenCarta/Desgracia7.png" },
            { "Enfermedad1.png", "pack://application:,,,/ImagenCarta/Enfermedad1.png" },
            { "Enfermedad2.png", "pack://application:,,,/ImagenCarta/Enfermedad2.png" },
            { "Enfermedad3.png", "pack://application:,,,/ImagenCarta/Enfermedad3.png" },
            { "Enfermedad4.png", "pack://application:,,,/ImagenCarta/Enfermedad4.png" },
            { "Enfermedad5.png", "pack://application:,,,/ImagenCarta/Enfermedad5.png" },
            { "Enfermedad6.png", "pack://application:,,,/ImagenCarta/Enfermedad6.png" },
            { "Enfermedad7.png", "pack://application:,,,/ImagenCarta/Enfermedad7.png" },
            { "Escándalo1.png", "pack://application:,,,/ImagenCarta/Escándalo1.png" },
            { "Escándalo2.png", "pack://application:,,,/ImagenCarta/Escándalo2.png" },
            { "Escándalo3.png", "pack://application:,,,/ImagenCarta/Escándalo3.png" },
            { "Escándalo4.png", "pack://application:,,,/ImagenCarta/Escándalo4.png" },
            { "Escándalo5.png", "pack://application:,,,/ImagenCarta/Escándalo5.png" },
            { "Escándalo6.png", "pack://application:,,,/ImagenCarta/Escándalo6.png" },
            { "Escándalo7.png", "pack://application:,,,/ImagenCarta/Escándalo7.png" },
            { "Fraude1.png", "pack://application:,,,/ImagenCarta/Fraude1.png" },
            { "Fraude2.png", "pack://application:,,,/ImagenCarta/Fraude2.png" },
            { "Fraude3.png", "pack://application:,,,/ImagenCarta/Fraude3.png" },
            { "Fraude4.png", "pack://application:,,,/ImagenCarta/Fraude4.png" },
            { "Fraude5.png", "pack://application:,,,/ImagenCarta/Fraude5.png" },
            { "Fraude6.png", "pack://application:,,,/ImagenCarta/Fraude6.png" },
            { "Fraude7.png", "pack://application:,,,/ImagenCarta/Fraude7.png" },
            { "Pérdida1.png", "pack://application:,,,/ImagenCarta/Pérdida1.png" },
            { "Pérdida2.png", "pack://application:,,,/ImagenCarta/Pérdida2.png" },
            { "Pérdida3.png", "pack://application:,,,/ImagenCarta/Pérdida3.png" },
            { "Pérdida4.png", "pack://application:,,,/ImagenCarta/Pérdida4.png" },
            { "Pérdida5.png", "pack://application:,,,/ImagenCarta/Pérdida5.png" },
            { "Pérdida6.png", "pack://application:,,,/ImagenCarta/Pérdida6.png" },
            { "Pérdida7.png", "pack://application:,,,/ImagenCarta/Pérdida7.png" },
            { "Ruina1.png", "pack://application:,,,/ImagenCarta/Ruina1.png" },
            { "Ruina2.png", "pack://application:,,,/ImagenCarta/Ruina2.png" },
            { "Ruina3.png", "pack://application:,,,/ImagenCarta/Ruina3.png" },
            { "Ruina4.png", "pack://application:,,,/ImagenCarta/Ruina4.png" },
            { "Ruina5.png", "pack://application:,,,/ImagenCarta/Ruina5.png" },
            { "Ruina6.png", "pack://application:,,,/ImagenCarta/Ruina6.png" },
            { "Ruina7.png", "pack://application:,,,/ImagenCarta/Ruina7.png" },
            { "MuerteInoportuna.png", "pack://application:,,,/ImagenCarta/MuerteInoportuna.png" },
            { "BebeCenteno.png", "pack://application:,,,/ImagenCarta/BebeCenteno.png" },
            { "HorneadoEnTarta.png", "pack://application:,,,/ImagenCarta/HorneadoEnTarta.png" },
            { "AtragantarHueso.png", "pack://application:,,,/ImagenCarta/AtragantarHueso.png" },
            { "MuerteViejo.png", "pack://application:,,,/ImagenCarta/MuerteViejo.png" },
            { "MuerteSarampio.png", "pack://application:,,,/ImagenCarta/MuerteSarampio.png" },
            { "DevoradoPorComdrejas.png", "pack://application:,,,/ImagenCarta/DevoradoPorComdrejas.png" },
            { "ConsumidoPorFuego.png", "pack://application:,,,/ImagenCarta/ConsumidoPorFuego.png" },
            { "Desmembrado.png", "pack://application:,,,/ImagenCarta/Desmembrado.png" },
            { "ComidoPorOsos.png", "pack://application:,,,/ImagenCarta/ComidoPorOsos.png" },
            { "MuerteSinPreocupacion.png", "pack://application:,,,/ImagenCarta/MuerteSinPreocupacion.png" },
            { "EmpujadoPorLasEscaleras.png", "pack://application:,,,/ImagenCarta/EmpujadoPorLasEscaleras.png" },
            { "AsesionadoPorHeredero.png", "pack://application:,,,/ImagenCarta/AsesionadoPorHeredero.png" },
            { "AhogadoEnPantano.png", "pack://application:,,,/ImagenCarta/AhogadoEnPantano.png" },
            { "QurmadoPorTurbia.png", "pack://application:,,,/ImagenCarta/QurmadoPorTurbia.png" },
            { "NoRegreso.png", "pack://application:,,,/ImagenCarta/NoRegreso.png" },
            { "SeveramenteQuemado.png", "pack://application:,,,/ImagenCarta/SeveramenteQuemado.png" },
            { "MuertePorDesesperacion.png", "pack://application:,,,/ImagenCarta/MuertePorDesesperacion.png" },
            { "SinAire.png", "pack://application:,,,/ImagenCarta/SinAire.png" },
            { "DesaparecioEnNiebla.png", "pack://application:,,,/ImagenCarta/DesaparecioEnNiebla.png" },
            { "CayoDesdeAlto.png", "pack://application:,,,/ImagenCarta/CayoDesdeAlto.png" }
        };

        public static readonly Dictionary<string, string> RutasImagenesPerfiles= new Dictionary<string, string>
        {
            { "/Imagenes/PerfilDiamante.png", "pack://application:,,,/Imagenes/PerfilDiamante.png" },
            { "/Imagenes/PerfilCalavera.png", "pack://application:,,,/Imagenes/PerfilCalavera.png" },
            { "/Imagenes/PerfilCorazon.png", "pack://application:,,,/Imagenes/PerfilCorazon.png" },
            { "/Imagenes/PerfilCastillo.png", "pack://application:,,,/Imagenes/PerfilCastillo.png" },
            { "/Imagenes/PerfilCorona.png", "pack://application:,,,/Imagenes/PerfilCorona.png" },
            { "/Imagenes/PerfilCastillo2.png", "pack://application:,,,/Imagenes/PerfilCastillo2.png" },
            { "/Imagenes/PerfilUnicornio.png", "pack://application:,,,/Imagenes/PerfilUnicornio.png" },
            { "/Imagenes/PerfilVela.png", "pack://application:,,,/Imagenes/PerfilVela.png" },
            { "/Imagenes/PerfilEspada.png", "pack://application:,,,/Imagenes/PerfilEspada.png" },
            { "/Imagenes/PerfilEscudo.png", "pack://application:,,,/Imagenes/PerfilEscudo.png" }
        };

        public static readonly Dictionary<string, string> RutasImagenesCartaBonus = new Dictionary<string, string>
        {
            { "SaltarJugador.png", "pack://application:,,,/ImagenCarta/SaltarJugador.png" },
            { "RobarCarta2.png", "pack://application:,,,/ImagenCarta/RobarCarta2.png" },
            { "RobarCarta1.png", "pack://application:,,,/ImagenCarta/RobarCarta1.png" },
            { "QuitarCarta.png", "pack://application:,,,/ImagenCarta/QuitarCarta.png" },
            { "PerderTurno.png", "pack://application:,,,/ImagenCarta/PerderTurno.png" } 
        };
    }
}
