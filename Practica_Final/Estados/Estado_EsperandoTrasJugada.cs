using Practica_Final.Cartas;
using Practica_Final.InteligenciaArtificial;
using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Estados;

public class Estado_EsperandoTrasJugada : IEstado
{
    private bool mousePulsado = false;
    private Jugador jugadorActual;
    private  Clock cronometro= new Clock();
    private bool relojIniciado = false;
    private Random rand = new Random();
    private bool iaHaJugadoNope = false;
    private bool seHaIntentadoJugarNope = false;

    public Estado_EsperandoTrasJugada()
    {
        EventManager.Instancia.EnSiguienteTurno += () => iaHaJugadoNope = false;
    }
    
    public void Dibujar()
    {
        if (!relojIniciado)
        {
            cronometro.Restart();
            relojIniciado = true;
        }
        jugadorActual = TurnManager.Instance.DevolverPrimerJugadorHumano();
        float margenIzquierdo = 100f;
        float anchoDisponible = Interfaz.Instancia.VentanaAncho - (margenIzquierdo * 2);
        float anchoCarta = 100f;
        if (jugadorActual.Mano.Count > 1)
        {
            float separacionNecesaria = anchoDisponible / (jugadorActual.Mano.Count - 1);
            Interfaz.Instancia.Separacion = Math.Min(120f, separacionNecesaria);
        }

        for (int i = 0; i < jugadorActual.Mano.Count; i++)
        {
            Carta cartaActual = jugadorActual.Mano[i];
               
            RectangleShape rect = new RectangleShape(new Vector2f(anchoCarta, 150));
            float posX = margenIzquierdo + (i * Interfaz.Instancia.Separacion);
            float posY = Interfaz.Instancia.IndicesSeleccionados.Contains(i) ? 570f : 600f;
            rect.Position = new Vector2f(posX, posY);
            rect.Texture = SpritesManager.Instancia.ConseguirTextura(cartaActual.Dibujo);
            rect.FillColor = Color.White;
            if (cartaActual.Resaltada)
            {
                rect.OutlineColor = Color.Yellow;
                rect.OutlineThickness = 2;
            }

               
            Interfaz.Instancia.Ventana.Draw(rect);
        }
        
        if (cronometro.ElapsedTime.AsSeconds() > 3f)
        {
            relojIniciado = false;
            if (!ReactManager.Instance.EfectoCancelado)
            {

                if (ReactManager.Instance.JugadaPendiente is IJugada jugada)
                {
                    jugada.JugarCarta();
                }

                if (StateManager.Intancia.EstadoActual is Estado_EsperandoTrasJugada)
                {
                    StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
                }
            }
            else
            {
                Console.WriteLine("[LOG] La carta fue NOPED. No hace nada.");
                StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
            }
            ReactManager.Instance.ResetearEfecto();
            iaHaJugadoNope = false;
            seHaIntentadoJugarNope = false;
        }
    }

    public void Inputs()
    {
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (mousePulsado) return;
                mousePulsado = true;
                    List<Carta> cartasElegidas = new();
                    Vector2i posPixel = Mouse.GetPosition(Interfaz.Instancia.Ventana);
                    Vector2f posMundo = Interfaz.Instancia.Ventana.MapPixelToCoords(posPixel);
                    if (Interfaz.Instancia.BotonJugar.GetGlobalBounds().Contains(posMundo))
                    {
                        if (Interfaz.Instancia.IndicesSeleccionados.Count == 0)
                            return;
                        foreach (var indice in Interfaz.Instancia.IndicesSeleccionados)
                        {
                            cartasElegidas.Add(jugadorActual.Mano[indice]);
                        }

                        int cantidad = cartasElegidas.Count;
                        if (cantidad == 1)
                        {
                            Console.WriteLine($"[LOG] Jugando carta simple: {cartasElegidas[0].Nombre}");
                            if (cartasElegidas[0] is IJugada cartaParaJugar && cartasElegidas[0] is Carta_Nope)
                            {
                                cartaParaJugar.JugarCarta();
                                ReactManager.Instance.ProcesarJugada(jugadorActual);
                                cronometro.Restart();
                            }
                        }
                    }
                    else if (posMundo.Y > 600 && posMundo.Y < 750)
                    {
                        for (int i =  jugadorActual.Mano.Count - 1; i >= 0; i--)
                        {
                            float posX = 100 + (i * Interfaz.Instancia.Separacion);
                            float posY = Interfaz.Instancia.IndicesSeleccionados.Contains(i) ? 570f : 600f;
                            FloatRect limiteCartas = new FloatRect(new Vector2f(posX, posY), new Vector2f(100, 150));
                            if (limiteCartas.Contains(posMundo))
                            {
                                if (Interfaz.Instancia.IndicesSeleccionados.Contains(i))
                                {
                                    Interfaz.Instancia.IndicesSeleccionados.Remove(i);
                                }
                                else
                                {
                                    Interfaz.Instancia.IndicesSeleccionados.Add(i);
                                }
                                break;
                            }
                        }
                    }
            }
            else
            {
                mousePulsado = false;
            }
    }

    public void ComportameintoIA()
    {
        if (iaHaJugadoNope || seHaIntentadoJugarNope) return;
        foreach (var jugador in TurnManager.Instance.jugadoresVivos )
        {
            if (jugador  is Jugador_Robot jugadorRobot)
            {
                if (jugadorRobot.MiComportamiento is ComportamientoFacil)
                {
                    JugarNope(2,jugadorRobot);   
                }
                if (jugadorRobot.MiComportamiento is ComportamientoMedio)
                {
                    JugarNope(3,jugadorRobot);   
                }
                if (jugadorRobot.MiComportamiento is ComportamientoDificil)
                {
                    JugarNope(4,jugadorRobot);   
                }
            }
        }

        seHaIntentadoJugarNope = true;

    }



    public void JugarNope(int porcentaje,Jugador_Robot robot)
    {
        int numero = rand.Next(0, 11);
        if (numero <= porcentaje)
        {
            foreach (var carta in robot.Mano)
            {
                if (carta is Carta_Nope nope)
                {
                    nope.JugarCarta();
                    ReactManager.Instance.ProcesarJugada(robot,carta);
                    iaHaJugadoNope = true;
                    cronometro.Restart();
                    return;
                }
            }
        }
    }
}