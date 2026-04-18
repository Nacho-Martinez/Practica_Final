using Practica_Final.Jugadores;

namespace Practica_Final.Managers;

public class EventManager
{
    public static EventManager Instancia { get; private set; } = new();
    
    public event Action EnJugadorSeleccionado;
    public event Action<Jugador> EnJugadorCOnfirmado;
    public event Action<int> EnInsercionRealizada;
    public event Action EnSigueinteTurno;
    public event Action EnGatoBoom;
    
    

    
    public void JugadorSeleccionado()
    {
        EnJugadorSeleccionado?.Invoke();
    }

    public void JugadorConfirmado(Jugador jugador)
    {
        EnJugadorCOnfirmado?.Invoke(jugador);
    }

    public void SiguienteTurno()
    {
        EnSigueinteTurno?.Invoke();
    }

    public void GatoBoom()
    {
        EnGatoBoom?.Invoke();
    }

    public void Insercion(int indice)
    {
        EnInsercionRealizada?.Invoke(indice);
    }

    
}