using Practica_Final.BarajaCartas;
using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using SFML.Graphics;

namespace Practica_Final.Cartas;

public class Carta_Favor :Carta,IJugada,IObjetivo
{
    private Random rand = new Random();
    public Carta_Favor(string Nombre, string  Dibujo) : base(Nombre, Dibujo)
    {
    }

    public void JugarCarta()
    { 
        EventManager.Instancia.EnCartaDada += VolverTurno;
        if (TurnManager.Instance.JugadorActual is not Jugador_Humano)
        {
            int indiceEnemigo;
            do
            {
                indiceEnemigo = rand.Next(0, TurnManager.Instance.jugadoresVivos.Count);
                
            } while (TurnManager.Instance.jugadoresVivos[indiceEnemigo] == TurnManager.Instance.JugadorActual);
            Logica(TurnManager.Instance.jugadoresVivos[indiceEnemigo]);
            return;
        }
        Interfaz.Instancia.IndiceEnemigo = 0;
        StateManager.Intancia.CambiarEstado(StateManager.Estados.EsperandoAtaque);
        EventManager.Instancia.EnJugadorSeleccionado += ElegirObjetivo;
        EventManager.Instancia.EnJugadorCOnfirmado += Logica;
    }

    private void VolverTurno()
    {
        LimpiarEventos();
        TurnManager.Instance.DarTurno(TurnManager.Instance.JugadorPendienteDeFavor);
        StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
    }

    private void Logica(Jugador objetivo)
    {
        TurnManager.Instance.AsignarJugadorPendienteDeFavor(TurnManager.Instance.JugadorActual); 
        TurnManager.Instance.DarTurno(objetivo);
        StateManager.Intancia.CambiarEstado(StateManager.Estados.DandoFavor);
        
    }

    public void ElegirObjetivo()
    {
        EventManager.Instancia.JugadorConfirmado(TurnManager.Instance.jugadoresVivos[Interfaz.Instancia.IndiceEnemigo]);
        StateManager.Intancia.CambiarEstado(StateManager.Estados.DandoFavor);
    }
    
    
    private void LimpiarEventos()
    {
        EventManager.Instancia.EnJugadorSeleccionado -= ElegirObjetivo;
        EventManager.Instancia.EnJugadorCOnfirmado -= Logica;
        EventManager.Instancia.EnCartaDada -= VolverTurno;
    }
}