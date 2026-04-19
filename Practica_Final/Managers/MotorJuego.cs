using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;

namespace Practica_Final.Managers;

public class MotorJuego
{
    public static MotorJuego Intancia { get; private set; }= new();
    private Random rand = new();

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
            TurnManager.Instance.jugadoresVivos.Remove(TurnManager.Instance.JugadorActual);
            TurnManager.Instance.PasarTurno();
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
}