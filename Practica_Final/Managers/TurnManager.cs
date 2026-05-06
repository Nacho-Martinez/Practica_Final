using Practica_Final.Jugadores;

namespace Practica_Final.Managers;

public class TurnManager
{
    public Jugador VictimaDelFavor { get; set; }
    public List<Jugador> jugadoresVivos { get; private set; } = new List<Jugador>();
    private int indiceActual = 0;
    private int turnosRestantes;
    private bool tieneQueRobar;
    public Jugador JugadorActual { get; private set; } 
    public Jugador JugadorPendienteDeFavor { get; private set; } 

    public static TurnManager Instance { get; private set; } = new();

    public void PasarTurnoSinRobar()
    {
        indiceActual = jugadoresVivos.IndexOf(JugadorActual);
        indiceActual = (indiceActual + 1) % jugadoresVivos.Count;
        JugadorActual = jugadoresVivos[indiceActual];
        EventManager.Instancia.SiguenteTurno();
    }
    public void PasarTurno()
    {
        EventManager.Instancia.SiguienteTurnoParaRobar();
    }

    public void ConfirmarPasoDeTurno()
    {
        string nombreAnterior = JugadorActual.Nombre;
        int indiceAnterior = jugadoresVivos.IndexOf(JugadorActual);
        
        
        if (jugadoresVivos.Contains(JugadorActual))
        {
            int indice = jugadoresVivos.IndexOf(JugadorActual);
            indice = (indice + 1) % jugadoresVivos.Count;
            JugadorActual = jugadoresVivos[indice];
            indiceActual = indice;
        }
        else 
        {
            if (indiceActual >= jugadoresVivos.Count) indiceActual = 0;
            JugadorActual = jugadoresVivos[indiceActual];
        }
        string nombreNuevo = JugadorActual.Nombre;
        int indiceNuevo = jugadoresVivos.IndexOf(JugadorActual);
        int totalVivos = jugadoresVivos.Count;
        
        Console.WriteLine("\n" + new string('=', 40));
        Console.WriteLine($" [CAMBIO DE TURNO]");
        Console.WriteLine($" Sale: {nombreAnterior} (Pos: {indiceAnterior})");
        Console.WriteLine($" Entra: {nombreNuevo} (Pos: {indiceNuevo})");
        Console.WriteLine($" Jugadores restantes: {totalVivos}");
        Console.WriteLine(new string('=', 40) + "\n");
        
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

    public void LimpiarListar()
    {
        JugadorActual = null;
        jugadoresVivos.Clear();
        JugadorPendienteDeFavor = null;
    }
    public void EliminarJugador(Jugador jugador)
    {
        int indiceDelMuerto = jugadoresVivos.IndexOf(jugador);
        jugadoresVivos.Remove(jugador);
    
        if (jugadoresVivos.Count > 0)
        {
            indiceActual = indiceDelMuerto % jugadoresVivos.Count;
            JugadorActual = jugadoresVivos[indiceActual];
        }
    }
}