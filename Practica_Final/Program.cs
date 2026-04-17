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
        
        Jugador_Humano Jugador_1 = new("Nacho");
        Jugador_Humano Jugador_2 = new("Tonto1");
        Jugador_Humano Jugador_3 = new("Tonto2");
        Jugador_Humano Jugador_4 = new("Tonto3");
        
        //Repartimos Cartas a todos
        Jugador_1.Mano = MotorJuego.Intancia.RepatirCartas(Mazo<Carta>.Instancia);
        
        
        CreacionMazo.Instancia.MeterBombas(4);
        Mazo<Carta>.Instancia.Barajar();
        TurnManager.Instance.jugadoresVivos.Add(Jugador_1);
        TurnManager.Instance.jugadoresVivos.Add(Jugador_2);
        TurnManager.Instance.jugadoresVivos.Add(Jugador_3);
        TurnManager.Instance.jugadoresVivos.Add(Jugador_4);
        
        Interfaz.Instancia.GenerarVentana();

    }
}