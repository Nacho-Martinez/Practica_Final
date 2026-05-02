using Practica_Final.Cartas;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Interfaces;

public class Estado_DefusandoBomba : IEstado
{
    private bool mousePulsado = false;
    private Jugador jugadorActual;
    private Clock relojIA = new Clock();
    private bool relojIniciado = false;
    public void Dibujar()
    {
        
        Interfaz.Instancia.DibujarRivales();
        RectangleShape carta = new RectangleShape(new Vector2f(200,350));
        carta.Position = new Vector2f(400,50);
        carta.Texture = SpritesManager.Instancia.ConseguirTextura(Interfaz.Instancia.cartaBomba.Dibujo);
        Interfaz.Instancia.Ventana.Draw(carta);
        
        jugadorActual = TurnManager.Instance.JugadorActual;
           
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
    }

    public void Inputs()
    {
        if (TurnManager.Instance.JugadorActual is not Jugador_Humano) return;
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
                            cartasElegidas.Add(TurnManager.Instance.JugadorActual.Mano[indice]);
                        }

                        int cantidad = cartasElegidas.Count;
                        if (cantidad == 1)
                        {
                            Console.WriteLine($"[LOG] Jugando carta simple: {cartasElegidas[0].Nombre}");
                            if (cartasElegidas[0] is Carta_Defuser && cartasElegidas[0] is IJugada cartaParaJugar)
                            {
                                cartaParaJugar.JugarCarta();
                                foreach (var car in TurnManager.Instance.JugadorActual.Mano)
                                {
                                    car.Resaltada = false;
                                }
                                ReactManager.Instance.ProcesarJugada(TurnManager.Instance.JugadorActual);
                            }
                        }
                        else
                        {
                            return;
                        }

                    }
                    else if (posMundo.Y > 600 && posMundo.Y < 750)
                    {
                        for (int i = TurnManager.Instance.JugadorActual.Mano.Count - 1; i >= 0; i--)
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
        if (!relojIniciado)
        {
            relojIA.Restart();
            relojIniciado = true;
        }
        if(relojIA.ElapsedTime.AsSeconds() <1.5f)return;
        if (TurnManager.Instance.JugadorActual is Jugador_Robot jugadorRobot)
        {
            foreach (var carta in jugadorRobot.Mano)
            {
                if (carta is Carta_Defuser)
                {
                    relojIA.Restart();
                    relojIniciado = false;
                    jugadorRobot.JugarCarta(carta);
                    return;
                }
            }
        }
        
        Console.WriteLine($"[IA] La ia {TurnManager.Instance.JugadorActual.Nombre} no ha encontrado un defuser en su mano");
        TurnManager.Instance.jugadoresVivos.Remove(TurnManager.Instance.JugadorActual);
    }
}