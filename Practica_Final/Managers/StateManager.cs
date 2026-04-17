namespace Practica_Final.Managers;

public class StateManager
{
    public static StateManager Intancia = new();

    public enum Estados
    {
        Normal,
        EsperandoAtaque
    };

    public Estados EstadoActual { get; private set; } = Estados.Normal;

    public void CambiarEstado(Estados estado)
    {
        EstadoActual = estado;
    }
}