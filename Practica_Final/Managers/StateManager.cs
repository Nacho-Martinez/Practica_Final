using Practica_Final.Estados;
using Practica_Final.Interfaces;

namespace Practica_Final.Managers;

public class StateManager
{
    public static StateManager Intancia = new();

    public enum Estados
    {
        Normal,
        EsperandoAtaque,
        ViendoFuturo,
        DefusandoBomba,
        InsertandoBomba,
        DandoFavor,
        EsperandoTrasJugada
    };

    public IEstado EstadoActual { get; private set; } = new Estado_Normal();
    private Dictionary<Estados, IEstado> diccionarioEstados;
    
    private StateManager()
    {
        diccionarioEstados = new Dictionary<Estados, IEstado>()
        {
            { Estados.Normal, new Estado_Normal() },
            { Estados.EsperandoAtaque, new Estado_EsperandoAtaque() },
            { Estados.ViendoFuturo, new Estado_ViendoFuturo() },
            { Estados.DefusandoBomba, new Estado_DefusandoBomba() },
            { Estados.InsertandoBomba, new Estado_InsertandoBomba() },
            { Estados.DandoFavor , new Estado_DandoFavor() },
            { Estados.EsperandoTrasJugada , new Estado_EsperandoTrasJugada()}
        };
        EstadoActual = diccionarioEstados[Estados.Normal]; 
    }

    public void CambiarEstado(Estados estado)
    {
        EstadoActual = diccionarioEstados[estado];
        
    }
}