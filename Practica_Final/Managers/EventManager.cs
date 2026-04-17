using Practica_Final.Jugadores;

namespace Practica_Final.Managers;

public class EventManager
{
    public static EventManager Instancia { get; private set; } = new();
    
    public event Action EnJugadorSeleccionado;
    public event Action<Jugador> EnJugadorCOnfirmado;


    
    public void JugadorSeleccionado()
    {
        EnJugadorSeleccionado?.Invoke();
    }

    public void JugadorConfirmado(Jugador jugador)
    {
        EnJugadorCOnfirmado?.Invoke(jugador);
    }
}