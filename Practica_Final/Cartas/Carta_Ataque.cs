using Practica_Final.BarajaCartas;
using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using SFML.Graphics;

namespace Practica_Final.Cartas;

public class Carta_Ataque : Carta ,IJugada,IObjetivo
{
    public Carta_Ataque(string Nombre, string  Dibujo) : base(Nombre, Dibujo)
    {
    }

    public void JugarCarta()
    {
        if (TurnManager.Instance.ObtenerJugadorActual() is not Jugador_Humano)
        {
            //Elije objetivo aleatorio que no sea el

            return;
        }
        Interfaz.Instancia.IndiceEnemigo = 0;
        StateManager.Intancia.CambiarEstado(StateManager.Estados.EsperandoAtaque);
        EventManager.Instancia.EnJugadorSeleccionado += ElegirObjetivo;
        EventManager.Instancia.EnJugadorCOnfirmado += Logica;
    }

    private void Logica(Jugador objetivo)
    {
        objetivo.Mano.Add(Mazo<Carta>.Instancia.CogerPrimeraCarta());
        objetivo.Mano.Add(Mazo<Carta>.Instancia.CogerPrimeraCarta());
        EventManager.Instancia.EnJugadorSeleccionado -= ElegirObjetivo;
        EventManager.Instancia.EnJugadorCOnfirmado -= Logica;
        StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
        
        TurnManager.Instance.PasarTurno();
    }

    public void ElegirObjetivo()
    {
        EventManager.Instancia.JugadorConfirmado(TurnManager.Instance.jugadoresVivos[Interfaz.Instancia.IndiceEnemigo]);
    }
    
}