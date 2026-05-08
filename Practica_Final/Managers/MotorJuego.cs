using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.Estados;
using Practica_Final.Jugadores;

namespace Practica_Final.Managers;

public class MotorJuego
{
    public static MotorJuego Intancia { get; private set; }= new();
    private Random rand = new();
    public bool Victoria { get; private set; } = false;

    public MotorJuego()
    {
        EventManager.Instancia.EnGatoBoom += ProcesarGatoEpxlosivo;
    }

    public void ProcesarGatoEpxlosivo()
    {
        bool tieneDefuser = false;
        foreach (var carta in TurnManager.Instance.JugadorActual.Mano)
        {
            if (carta is Carta_Defuser)
            {
                carta.Resaltada = true;
                tieneDefuser = true;
            }
        }

        if (!tieneDefuser)
        {
            Console.WriteLine($"[EXPLOSIÓN] {TurnManager.Instance.JugadorActual.Nombre} ha muerto.");
            TurnManager.Instance.EliminarJugador(TurnManager.Instance.JugadorActual);
            RevisarFInPartida();
            if (ElJuego.Instancia.haEmpezadoElJuego)
            {
                StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
                EventManager.Instancia.SiguenteTurno();
            }
            return;
        }
        StateManager.Intancia.CambiarEstado(StateManager.Estados.DefusandoBomba);
    }

    public List<Carta> RepatirCartas(Mazo<Carta> Mazo)
    {
        List<Carta> listaTemporal = new();
        string rutaAleatoria = CreacionMazo.Instancia.dibujosDefuser[rand.Next(CreacionMazo.Instancia.dibujosDefuser.Length)];
        listaTemporal.Add(new Carta_Defuser("Carta_Defuser",rutaAleatoria));
        for (int i = 0; i < 8; i++)
        {
            Carta cartaRobada = Mazo.Baraja.Pop();
            listaTemporal.Add(cartaRobada);
        }

        return listaTemporal;
    }

    public void RevisarFInPartida()
    {
        int playerVivos = 0;
        int robotsVivos = 0;
        foreach (var jugador in TurnManager.Instance.jugadoresVivos)
        {
            if (jugador is Jugador_Humano)
                playerVivos++;
            else
            {
                robotsVivos++;
            }
        }

        if (playerVivos == 0)
        {
            Victoria = false;
            StateManager.Intancia.CambiarEstado(StateManager.Estados.FinPartida);
            ElJuego.Instancia.haEmpezadoElJuego = false;
        }
        else if(robotsVivos == 0)
        {
            Victoria = true;
            StateManager.Intancia.CambiarEstado(StateManager.Estados.FinPartida);
            ElJuego.Instancia.haEmpezadoElJuego = false;
        }
    }
}