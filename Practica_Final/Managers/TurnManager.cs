using Practica_Final.Jugadores;

namespace Practica_Final.Managers;

public class TurnManager
{
    public List<Jugador> jugadoresVivos { get; private set; } = new List<Jugador>();
    private int indiceActual = 0;
    private int turnosRestantes;
    private bool tieneQueRobar;
    public Jugador JugadorActual { get; private set; } 
    public Jugador JugadorPendienteDeFavor { get; private set; } 

    public static TurnManager Instance { get; private set; } = new();

    public void PasarTurnoSinRobar()
    {
        indiceActual = (indiceActual + 1) % jugadoresVivos.Count;
        JugadorActual = jugadoresVivos[indiceActual];
        EventManager.Instancia.SiguenteTurno();
    }
    public void PasarTurno()
    {
        indiceActual = (indiceActual + 1) % jugadoresVivos.Count;
        JugadorActual = jugadoresVivos[indiceActual];
        EventManager.Instancia.SiguienteTurnoParaRobar();
        EventManager.Instancia.SiguenteTurno();
    }

    
    public void DarTurno(Jugador jugador)
    {
        JugadorActual = jugador;
    }
    public void InicializarJugadorActual()
    {
        JugadorActual = jugadoresVivos[0];
    }

    public void AsignarJugadorPendienteDeFavor(Jugador jugador)
    {
        JugadorPendienteDeFavor = jugador;
    }

    public Jugador DevolverPrimerJugadorHumano()
    {
        foreach (var jugador in jugadoresVivos)
        {
            if (jugador is Jugador_Humano)
            {
                return jugador;
            }
        }

        return null;
    }
}