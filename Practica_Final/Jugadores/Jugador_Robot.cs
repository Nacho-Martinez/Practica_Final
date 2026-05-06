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

    public Jugador_Robot(string nombre,Comportamiento comportamiento) : base(nombre)
    {
        EventManager.Instancia.EnSiguienteTurno += IniciarTurno;   
        MiComportamiento = comportamiento;
    }
    public override void IniciarTurno()
    {
        if (TurnManager.Instance.JugadorActual != this) return; 
        MiComportamiento.RellenarListas(this);
        numeroJugadas = MiComportamiento.NumeroDeJugadas();
    }

    public void JugarCarta()
    {
        bool puedeJugar = false;
        if (numeroJugadas < 0)
        {
            if (TurnManager.Instance.JugadorActual == this)
                TurnManager.Instance.PasarTurno();
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
        if (cartasElegidas == null)
        {
            Console.WriteLine($"[IA] {Nombre} no tiene jugadas y pasa el turno.");
            TurnManager.Instance.PasarTurno();
            return;
        }


        int cantidad = cartasElegidas.Length;
        if (cantidad == 1)
        {
            Carta cartaJugada = cartasElegidas[0];
            if (cartasElegidas[0] is not IJugada cartaParaJugar || cartasElegidas[0] is Carta_Gato ||
                cartasElegidas[0] is Carta_Defuser || cartasElegidas[0] is Carta_Nope) return;
            
           
            float posXInicio = 100f ;
            float posYInicio = 570f; 
            Vector2f posInicio = Interfaz.Instancia.ObtenerPosicionRobot(this);
            Vector2f posDestino = new Vector2f(720f, 300f); 
            Texture tex = SpritesManager.Instancia.ConseguirTextura(cartaJugada.Dibujo);
                            
            Interfaz.Instancia.LanzarAnimacion(tex, posInicio, posDestino, 0.4f, () => 
            {
                                
                ReactManager.Instance.MeterJugadaEnCola(cartaJugada);
                ReactManager.Instance.ProcesarJugada(this,cartasElegidas);
                if (cartaJugada is IForzarFinTurno) // <- añade esto
                {
                    numeroJugadas = 0;
                }
                StateManager.Intancia.CambiarEstado(StateManager.Estados.EsperandoTrasJugada);
            });
           
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
                    cartaComprobarTipo.tipoGato != tipoRequerido) return;
                
                
                                
                Vector2f posInicio1 = Interfaz.Instancia.ObtenerPosicionRobot(this);
                Vector2f posInicio2 = Interfaz.Instancia.ObtenerPosicionRobot(this);
                Vector2f posDestino1 = new Vector2f(720f, 300f);
                Vector2f posDestino2 = new Vector2f(820f, 300f);
                Texture tex1 = SpritesManager.Instancia.ConseguirTextura(cartasElegidas[0].Dibujo);
                Texture tex2 = SpritesManager.Instancia.ConseguirTextura(cartasElegidas[1].Dibujo);
                                
                Interfaz.Instancia.LanzarAnimacion(tex2, posInicio2, posDestino2, 0.4f);
                Interfaz.Instancia.LanzarAnimacion(tex1, posInicio1, posDestino1, 0.4f, () => 
                {
                    ReactManager.Instance.MeterJugadaEnCola(cartasElegidas[0]);
                    ReactManager.Instance.ProcesarJugada(this,cartasElegidas);
                    StateManager.Intancia.CambiarEstado(StateManager.Estados.EsperandoTrasJugada);
                });
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
        if (numeroJugadas <= 0)
        {
            TurnManager.Instance.PasarTurno();
        }
        
    }
}