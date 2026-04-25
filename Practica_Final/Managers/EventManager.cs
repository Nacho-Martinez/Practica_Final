using Practica_Final.Jugadores;

namespace Practica_Final.Managers;

public class EventManager
{
    public static EventManager Instancia { get; private set; } = new();
    
    public event Action EnJugadorSeleccionado;
    public event Action<Jugador> EnJugadorCOnfirmado;
    public event Action<int> EnInsercionRealizada;
    public event Action EnSigueinteTurnoParaRobar;
    public event Action EnGatoBoom;
    public event Action EnCartaDada;
    public event Action EnSiguienteTurno;
    

    
    public void JugadorSeleccionado()
    {
        EnJugadorSeleccionado?.Invoke();
    }

    public void JugadorConfirmado(Jugador jugador)
    {
        EnJugadorCOnfirmado?.Invoke(jugador);
    }

    public void SiguienteTurnoParaRobar()
    {
        EnSigueinteTurnoParaRobar?.Invoke();
    }

    public void GatoBoom()
    {
        EnGatoBoom?.Invoke();
    }

    public void Insercion(int indice)
    {
        EnInsercionRealizada?.Invoke(indice);
    }

    public void CartaDada()
    {
        Console.WriteLine("Vento de carta dada se ha lanzado");
        EnCartaDada?.Invoke();
    }

    public void SiguenteTurno()
    {
        EnSiguienteTurno?.Invoke();
    }
    
}