using Practica_Final.Cartas;
using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Estados;

public class Estado_Normal : IEstado
{
    private bool mousePulsado = false;
    private Jugador jugadorActual;
    public void Dibujar()
    {
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
                            cartasElegidas.Add(Interfaz.Instancia.JugadorActual.Mano[indice]);
                        }

                        int cantidad = cartasElegidas.Count;
                        if (cantidad == 1)
                        {
                            Console.WriteLine($"[LOG] Jugando carta simple: {cartasElegidas[0].Nombre}");
                            if (cartasElegidas[0] is IJugada cartaParaJugar)
                            {
                                cartaParaJugar.JugarCarta();
                            }

                            InputManager.Instance.ProcesarJugada(Interfaz.Instancia.JugadorActual);
                        }
                        //AQUI VAN LOS FILTROS PARA LAS CARTAS DE GATOS ETC
                        else
                        {
                            Console.WriteLine($"[LOG] Logica aun no hecha");
                        }

                    }
                    else if (Interfaz.Instancia.BotonSaltarTurno.GetGlobalBounds().Contains(posMundo))
                    {
                        TurnManager.Instance.PasarTurno();
                    }
                    else if (posMundo.Y > 600 && posMundo.Y < 750)
                    {
                        for (int i = Interfaz.Instancia.JugadorActual.Mano.Count - 1; i >= 0; i--)
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
}