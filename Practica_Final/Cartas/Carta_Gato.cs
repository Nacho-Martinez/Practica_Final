using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using SFML.Graphics;

namespace Practica_Final.Cartas;

public class Carta_Gato : Carta,IJugada,IObjetivo
{
    public enum TiposGato
    {
      GatoTaco,
      GatoSandia,
      GatoBarba,
      GatoPatata,
      Ninguno
    }

    public TiposGato tipoGato { get; private set; }
    private Random rand = new Random();
    public Carta_Gato(string Nombre, string  Dibujo,TiposGato tipo) : base(Nombre, Dibujo)
    {
        tipoGato = tipo;
    }

    public void JugarCarta()
    {
        EventManager.Instancia.EnCartaDada += VolverTurno;
        if (TurnManager.Instance.JugadorActual is not Jugador_Humano)
        {
            int indiceEnemigo;
            Jugador objetivo;
            do
            {
                objetivo = TurnManager.Instance.jugadoresVivos[rand.Next(0, TurnManager.Instance.jugadoresVivos.Count)];
                
            } while (objetivo == TurnManager.Instance.JugadorActual);
            Logica(objetivo);
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
        Jugador jugadorOriginal = TurnManager.Instance.JugadorPendienteDeFavor;
        TurnManager.Instance.DarTurno(TurnManager.Instance.JugadorPendienteDeFavor);
        StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
        if (jugadorOriginal is Jugador_Robot robot)
        {
            robot.ReducirJugadas();
        }
    }

    private void Logica(Jugador objetivo)
    {
        TurnManager.Instance.AsignarJugadorPendienteDeFavor(TurnManager.Instance.JugadorActual); 
        TurnManager.Instance.DarTurno(objetivo);
        EventManager.Instancia.EnJugadorSeleccionado -= ElegirObjetivo;
        EventManager.Instancia.EnJugadorCOnfirmado -= Logica;
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