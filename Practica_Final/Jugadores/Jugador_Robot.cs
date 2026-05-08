using System.Reflection.Metadata.Ecma335;
using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.InteligenciaArtificial;
using Practica_Final.Interfaces;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;

namespace Practica_Final.Jugadores;

public class Jugador_Robot:Jugador
{
    
    private Random rand = new Random();
    public Comportamiento MiComportamiento { get; }
    private int numeroJugadas = -1;
    private bool jugandoCarta = false;
    private bool esperandoRobo = false;

    public Jugador_Robot(string nombre,Comportamiento comportamiento) : base(nombre)
    {
        EventManager.Instancia.EnSiguienteTurno += IniciarTurno;   
        MiComportamiento = comportamiento;
    }
    public override void IniciarTurno()
    {
        if (TurnManager.Instance.JugadorActual != this) return; 
        Console.WriteLine($"[DEBUG IniciarTurno] {Nombre} inicia turno, animaciones: {Interfaz.Instancia.AnimacionesActivas.Count}");
        MiComportamiento.RellenarListas(this);
        numeroJugadas = MiComportamiento.NumeroDeJugadas();
        esperandoRobo = false;
    }

    public void JugarCarta()
    {
        Console.WriteLine($"[DEBUG JugarCarta] {Nombre} intenta jugar, numeroJugadas: {numeroJugadas}, animaciones: {Interfaz.Instancia.AnimacionesActivas.Count}");
        if (ElJuego.Instancia.HayTurnoSinRobarPendiente) return;
        if (ElJuego.Instancia.TurnoConfirmadoPendiente) return;
        if (jugandoCarta) return;
        bool puedeJugar = false;
        if (numeroJugadas <= 0)
        {
            if (TurnManager.Instance.JugadorActual == this && !esperandoRobo && !ElJuego.Instancia.TurnoConfirmadoPendiente &&
                !ElJuego.Instancia.HayTurnoSinRobarPendiente)
            {
                esperandoRobo = true;
                TurnManager.Instance.PasarTurno();
            }

            return;
        }
        if(TurnManager.Instance.JugadorActual != this)return;
        foreach (var carta in Mano)
        {
            if (carta is not Carta_Nope || carta is not Carta_Defuser)
            {
                puedeJugar = true;
            }
        }

        if (!puedeJugar)
        {
            TurnManager.Instance.PasarTurno();
            return;
        }
        Carta[] cartasElegidas = MiComportamiento.CartasParaJugar();
        Console.WriteLine($"[DEBUG CartasElegidas] {Nombre}: {(cartasElegidas == null ? "null" : string.Join(", ", cartasElegidas.Select(c => c.Nombre)))}");
        if (cartasElegidas == null)
        {
            Console.WriteLine($"[IA] {Nombre} no tiene jugadas y pasa el turno.");
            TurnManager.Instance.PasarTurno();
            return;
        }

        jugandoCarta = true;   
        int cantidad = cartasElegidas.Length;
        if (cantidad == 1)
        {
            Carta cartaJugada = cartasElegidas[0];
            if (cartasElegidas[0] is not IJugada cartaParaJugar || cartasElegidas[0] is Carta_Gato ||
                cartasElegidas[0] is Carta_Defuser || cartasElegidas[0] is Carta_Nope)
            {
                jugandoCarta = false;
                numeroJugadas = 0;
                return;
            }
            
           
            float posXInicio = 100f ;
            float posYInicio = 570f; 
            Vector2f posInicio = Interfaz.Instancia.ObtenerPosicionRobot(this);
            Vector2f posDestino = new Vector2f(720f, 300f); 
            Texture tex = SpritesManager.Instancia.ConseguirTextura(cartaJugada.Dibujo);
            jugandoCarta = true;            
            Interfaz.Instancia.LanzarAnimacion(tex, posInicio, posDestino, 0.4f, () => 
            {
                jugandoCarta = false;            
                ReactManager.Instance.MeterJugadaEnCola(cartaJugada);
                ReactManager.Instance.ProcesarJugada(this,cartasElegidas);
                if (cartaJugada is IForzarFinTurno) // <- añade esto
                {
                    numeroJugadas = 0;
                }
                StateManager.Intancia.CambiarEstado(StateManager.Estados.EsperandoTrasJugada);
            });
            jugandoCarta = true;
           
        }
        else if (cantidad == 2)
        {
            Carta_Gato.TiposGato tipoRequerido = Carta_Gato.TiposGato.Ninguno;
            if (cartasElegidas[0] is Carta_Gato cartaGato)
            {
                tipoRequerido = cartaGato.tipoGato;
            }

            foreach (var carta in cartasElegidas)
            {
                if (carta is not Carta_Gato)
                {
                    return;
                }
            }

            if (cartasElegidas[0] is IJugada cartaParaJugar && cartasElegidas[1]is Carta_Gato cartaComprobarTipo)
            {
                if (tipoRequerido == Carta_Gato.TiposGato.Ninguno ||
                    cartaComprobarTipo.tipoGato != tipoRequerido)
                {
                    jugandoCarta = false;
                    numeroJugadas = 0;
                    return;
                }
                
                
                                
                Vector2f posInicio1 = Interfaz.Instancia.ObtenerPosicionRobot(this);
                Vector2f posInicio2 = Interfaz.Instancia.ObtenerPosicionRobot(this);
                Vector2f posDestino1 = new Vector2f(720f, 300f);
                Vector2f posDestino2 = new Vector2f(820f, 300f);
                Texture tex1 = SpritesManager.Instancia.ConseguirTextura(cartasElegidas[0].Dibujo);
                Texture tex2 = SpritesManager.Instancia.ConseguirTextura(cartasElegidas[1].Dibujo);
                                
                Interfaz.Instancia.LanzarAnimacion(tex2, posInicio2, posDestino2, 0.4f);
                Interfaz.Instancia.LanzarAnimacion(tex1, posInicio1, posDestino1, 0.4f, () => 
                {
                    jugandoCarta = false;
                    ReactManager.Instance.MeterJugadaEnCola(cartasElegidas[0]);
                    ReactManager.Instance.ProcesarJugada(this,cartasElegidas);
                    StateManager.Intancia.CambiarEstado(StateManager.Estados.EsperandoTrasJugada);
                });
                jugandoCarta = true;
            }
        }
        if(TurnManager.Instance.JugadorActual != this)return;
    }

    public void JugarCarta(Carta carta)
    {
        if (carta is IJugada cartaJugar)
        {
            cartaJugar.JugarCarta();
            ReactManager.Instance.ProcesarJugada(this, carta);
            foreach (var Var in Mano)
            {
                Console.WriteLine($"Carta: {Var.Nombre}");
            }
        }
    }
    public override void RobarCarta()
    {
    }

    public void ReducirJugadas()
    {
        numeroJugadas--;
        if (numeroJugadas <= 0 && !ElJuego.Instancia.HayTurnoSinRobarPendiente)
        {
            TurnManager.Instance.PasarTurno();
        }
        
    }
}