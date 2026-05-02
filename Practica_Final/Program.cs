using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.InteligenciaArtificial;
using Practica_Final.Jugadores;
using Practica_Final.Managers;

namespace Practica_Final;

class Program
{
    static void Main(string[] args)
    {
        Interfaz.Instancia.CrearVentana();
        ElJuego.Instancia.BucleMenu();
    }
}