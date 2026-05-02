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
        EsperandoTrasJugada,
        MenuPrincipal,
        EleccionDificultad,
        EleccionNombre,
        FinPartida
    };

    public IEstado EstadoActual { get; private set; }
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
            { Estados.EsperandoTrasJugada , new Estado_EsperandoTrasJugada()},
            {Estados.MenuPrincipal, new Estado_MenuPrincipal()},
            { Estados.EleccionNombre ,new Estado_EleccionNombre()},
            { Estados.EleccionDificultad ,new Estado_EleccionDificultad()},
            { Estados.FinPartida ,new Estado_FinPartida()}
        };
        EstadoActual = diccionarioEstados[Estados.MenuPrincipal]; 
    }

    public void CambiarEstado(Estados estado)
    {
        if (estado == Estados.EleccionNombre)
        {
            diccionarioEstados[estado] = new Estado_EleccionNombre();
        }
        else if (estado == Estados.EleccionDificultad)
        {
            diccionarioEstados[estado] = new Estado_EleccionDificultad();
        }
    
        EstadoActual = diccionarioEstados[estado];
        
    }
}