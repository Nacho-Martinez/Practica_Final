using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.Jugadores;
using Practica_Final.Managers;

namespace Practica_Final;

class Program
{
    static void Main(string[] args)
    {
        Interfaz.Instancia.CrearVentana();
        Mazo<Carta>.Instancia.Baraja = CreacionMazo.Instancia.PrepararMazo(4);
        Mazo<Carta>.Instancia.Barajar();
        
        Jugador_Humano jugador1 = new("Nacho");
        Jugador_Humano jugador2 = new("Tonto1");
        Jugador_Humano jugador3 = new("Tonto2");
        Jugador_Humano jugador4 = new("Tonto3");
        
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
        
        //Esto Lo tengo que quitar
        Carta_Explosion bombaDePrueba = new Carta_Explosion("Bomba de Prueba", "C:\\Users\\nache\\OneDrive\\Desktop\\POO\\Practica_Final\\Practica_Final\\Sprites\\Explosion.png");
        Mazo<Carta>.Instancia.Baraja.Push(bombaDePrueba);
        //Esto tbm
        string rutaAtaque = "C:\\Users\\nache\\OneDrive\\Desktop\\POO\\Practica_Final\\Practica_Final\\Sprites\\Ataque1.png";
        Carta_Ataque cartaTrucada = new Carta_Ataque("Ataque Trucado", rutaAtaque);
        jugador1.Mano.Add(cartaTrucada);
        
         Interfaz.Instancia.ResterarIndiceInsercion();
         
         
        Interfaz.Instancia.GenerarVentana();

    }
}