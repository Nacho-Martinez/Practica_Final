using Practica_Final.Jugadores;

namespace Practica_Final.Managers;

public class TurnManager
{
    public List<Jugador> jugadoresVivos { get; private set; } = new List<Jugador>();
    private int indiceActual = 0;
    private int turnosRestantes;
    private bool tieneQueRobar;
    public Jugador JugadorActual { get; private set; } 

    public static TurnManager Instance { get; private set; } = new();

    public void PasarTurnoSinRobar()
    {
        indiceActual = (indiceActual + 1) % jugadoresVivos.Count;
    }
    public void PasarTurno()
    {
        indiceActual = (indiceActual + 1) % jugadoresVivos.Count;
        JugadorActual = jugadoresVivos[indiceActual];
        EventManager.Instancia.SiguienteTurno();
    }

    public Jugador ObtenerJugadorActual()
    {
        JugadorActual = jugadoresVivos[indiceActual];
        return jugadoresVivos[indiceActual];
    }

    public void DarTurno(Jugador jugador)
    {
        JugadorActual = jugador;
    }
    public void InicializarJugadorActual()
    {
        JugadorActual = jugadoresVivos[0];
    }
}