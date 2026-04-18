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
        
        Interfaz.Instancia.GenerarVentana();

    }
}