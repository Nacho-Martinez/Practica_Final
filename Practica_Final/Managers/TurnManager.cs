using Practica_Final.Jugadores;

namespace Practica_Final.Managers;

public class TurnManager
{
    public List<Jugador> jugadoresVivos { get; private set; } = new List<Jugador>();
    private int indiceActual = 0;
    private int turnosRestantes;
    private bool tieneQueRobar;

    public static TurnManager Instance { get; private set; } = new();
    
    public void PasarTurno()
    {
        indiceActual = (indiceActual + 1) % jugadoresVivos.Count;
        EventManager.Instancia.SiguienteTurno();
    }

    public Jugador ObtenerJugadorActual()
    {
        return jugadoresVivos[indiceActual];
    }
}