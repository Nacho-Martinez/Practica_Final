using Practica_Final.BarajaCartas;
using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;

namespace Practica_Final.Cartas;

public class Carta_Ataque : Carta ,IJugada,IObjetivo
{
    private Random rand = new Random();
    public Carta_Ataque(string Nombre, string  Dibujo) : base(Nombre, Dibujo)
    {
    }

    public void JugarCarta()
    {
        if (TurnManager.Instance.JugadorActual is not Jugador_Humano)
        {
            int indiceEnemigo;
            do
            {
             indiceEnemigo = rand.Next(0, TurnManager.Instance.jugadoresVivos.Count);
                
            } while (TurnManager.Instance.jugadoresVivos[indiceEnemigo] == TurnManager.Instance.JugadorActual);
            Console.WriteLine($"Jugador atacado:{TurnManager.Instance.jugadoresVivos[indiceEnemigo].Nombre}");
            Logica(TurnManager.Instance.jugadoresVivos[indiceEnemigo]);
            return;
        }
        Interfaz.Instancia.IndiceEnemigo = 0;
        StateManager.Intancia.CambiarEstado(StateManager.Estados.EsperandoAtaque);
        EventManager.Instancia.EnJugadorSeleccionado += ElegirObjetivo;
        EventManager.Instancia.EnJugadorCOnfirmado += Logica;
    }

    private void Logica(Jugador objetivo)
    {
        EventManager.Instancia.EnJugadorSeleccionado -= ElegirObjetivo;
        EventManager.Instancia.EnJugadorCOnfirmado -= Logica;
        StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
        RobarCartaAnimada(objetivo,2);
        
    }

    public void ElegirObjetivo()
    {
        EventManager.Instancia.JugadorConfirmado(TurnManager.Instance.jugadoresVivos[Interfaz.Instancia.IndiceEnemigo]);
    }

    private void RobarCartaAnimada(Jugador objetivo, int cartasParaRobar)
    {
        if (cartasParaRobar <= 0)
        {
            StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
            TurnManager.Instance.PasarTurnoSinRobar();
            return;
        }
        Carta cartaRobada = Mazo<Carta>.Instancia.Baraja.Pop();

        if (cartaRobada is Carta_Explosion)
        {
            Interfaz.Instancia.cartaBomba = cartaRobada;
                
            StateManager.Intancia.CambiarEstado(StateManager.Estados.DefusandoBomba);
            TurnManager.Instance.DarTurno(objetivo);
            EventManager.Instancia.GatoBoom();

            return;
                
        }
        else
        {
            Vector2f inicio = new Vector2f(600, 400);
            Vector2f final = Interfaz.Instancia.ObtenerPosicionRobot(objetivo);
            Texture text;
            text = SpritesManager.Instancia.ConseguirTextura(TurnManager.Instance.JugadorActual is Jugador_Humano ? cartaRobada.Dibujo : "C:\\Users\\nache\\OneDrive\\Desktop\\POO\\Practica_Final\\Practica_Final\\Sprites\\ReversoCarta.png");
            Interfaz.Instancia.LanzarAnimacion(text,inicio,final,0.4f, () =>
            {
                objetivo.Mano.Add(cartaRobada);
                RobarCartaAnimada(objetivo, cartasParaRobar -1 );
            });
        }
    }
    
}