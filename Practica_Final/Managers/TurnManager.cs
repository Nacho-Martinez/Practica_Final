using Practica_Final.Jugadores;

namespace Practica_Final.Managers;

public class TurnManager
{
    public List<Jugador> jugadoresVivos { get; private set; } = new List<Jugador>();
    private int indiceActual = 0;
    private int TurnosRestantes;

    public static TurnManager Instance { get; private set; } = new();
    public void PasarTurnoSinRobar()
    {
        
    }

    public void PasarTurno()
    {
        
    }

    public Jugador ObtenerJugadorActual()
    {
        return jugadoresVivos[indiceActual];
    }
}