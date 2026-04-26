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
        Mazo<Carta>.Instancia.Baraja = CreacionMazo.Instancia.PrepararMazo(4);
        Console.WriteLine($"Tamaño del Mazo : {Mazo<Carta>.Instancia.Baraja.Count}");
        Mazo<Carta>.Instancia.Barajar();
        Console.WriteLine($"Tamaño del Mazo : {Mazo<Carta>.Instancia.Baraja.Count}");
        Jugador_Humano jugador1 = new("Nacho");
        Jugador_Robot jugador2 = new("Tonto1Robot",new ComportamientoFacil());
        Jugador_Robot jugador3 = new("Tonto2Robot",new ComportamientoMedio());
        Jugador_Robot jugador4 = new("Tonto3Robot",new ComportamientoDificil());
        
        //Repartimos Cartas a todos
        jugador1.Mano = MotorJuego.Intancia.RepatirCartas(Mazo<Carta>.Instancia);
        jugador2.Mano = MotorJuego.Intancia.RepatirCartas(Mazo<Carta>.Instancia);
        jugador3.Mano = MotorJuego.Intancia.RepatirCartas(Mazo<Carta>.Instancia);
        jugador4.Mano = MotorJuego.Intancia.RepatirCartas(Mazo<Carta>.Instancia);
        
        
        CreacionMazo.Instancia.MeterBombas(4);
        Mazo<Carta>.Instancia.Barajar();
        TurnManager.Instance.jugadoresVivos.Add(jugador1);
        TurnManager.Instance.jugadoresVivos.Add(jugador2);
        TurnManager.Instance.jugadoresVivos.Add(jugador3);
        TurnManager.Instance.jugadoresVivos.Add(jugador4);
        TurnManager.Instance.InicializarJugadorActual();
        
        // //Esto Lo tengo que quitar
        // Carta_Explosion bombaDePrueba = new Carta_Explosion("Bomba de Prueba", "C:\\Users\\nache\\OneDrive\\Desktop\\POO\\Practica_Final\\Practica_Final\\Sprites\\Explosion.png");
        // Mazo<Carta>.Instancia.Baraja.Push(bombaDePrueba);
        // //Esto tbm
        // string rutaAtaque = "C:\\Users\\nache\\OneDrive\\Desktop\\POO\\Practica_Final\\Practica_Final\\Sprites\\Ataque1.png";
        // Carta_Ataque cartaTrucada = new Carta_Ataque("Ataque Trucado", rutaAtaque);
        // jugador1.Mano.Add(cartaTrucada);
        
         Interfaz.Instancia.ResterarIndiceInsercion();
         
         Console.WriteLine("======= CONTENIDO DEL MAZO =======");
         // Recorremos de arriba (0) hacia abajo (Count - 1)
         foreach (var VARIABLE in Mazo<Carta>.Instancia.Baraja)
         {
             Console.WriteLine($"Carta: {VARIABLE.Nombre}");
         }
         Console.WriteLine("==================================");
         Console.WriteLine($"Mano de la Ia");
         Console.WriteLine($"=================================00");
         foreach (var VARIABLE in jugador2.Mano)
         {
             Console.WriteLine($"Carta: {VARIABLE.Nombre}");
         }

        
         
        Interfaz.Instancia.GenerarVentana();

    }
}