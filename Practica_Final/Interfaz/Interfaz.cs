using Practica_Final.Cartas;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Interfaz;

public class Interfaz
{
    private RenderWindow ventana;
    private Jugador jugadorActual;
    private RectangleShape botonJugar;
    private uint ventanaAncho = 1200;
    private uint ventanaAlto = 800;
    private List<int> indicesSeleccionados = new List<int>();
    private float separacion = 120f;
    

    public void GenerarVentana()
    {
         ventana = new RenderWindow(new VideoMode(new Vector2u(ventanaAncho,ventanaAlto),32), "Exploding Kittens");
        ventana.SetFramerateLimit(60);
        jugadorActual = TurnManager.Instance.ObtenerJugadorActual();
        botonJugar = new RectangleShape(new Vector2f(200, 80));
        botonJugar.FillColor = Color.Green;
        botonJugar.Position = new Vector2f(80, 360);
        
        ventana.Closed += (_, _) => ventana.Close();

        ventana.MouseButtonPressed += (sender, e) =>
        {
            if (e.Button == Mouse.Button.Left)
            {
                List<Carta> cartasElegidas = new();
                Vector2i posPixel = Mouse.GetPosition(ventana);
                Vector2f posMundo = ventana.MapPixelToCoords(posPixel);
                if (botonJugar.GetGlobalBounds().Contains(posMundo))
                {
                    if (indicesSeleccionados.Count == 0)
                        return;
                    foreach (var indice in indicesSeleccionados)
                    {
                        cartasElegidas.Add(jugadorActual.Mano[indice]);
                    }
                    int cantidad = cartasElegidas.Count;
                    if (cantidad == 1)
                    {
                        Console.WriteLine($"[LOG] Jugando carta simple: {cartasElegidas[0].Nombre}");
                        ProcesarJugada(jugadorActual);
                    }
                    //AQUI VAN LOS FILTROS PARA LAS CARTAS DE GATOS ETC
                    else
                    {
                        Console.WriteLine($"[LOG] Logica aun no hecha");
                    }
                    
                }
                else if (posMundo.Y > 600 && posMundo.Y < 750)
                {
                    for (int i = jugadorActual.Mano.Count - 1; i >= 0; i--)
                    {
                        float posX = 100 + (i * separacion);
                        FloatRect limiteCartas = new FloatRect(new Vector2f(posX, 600), new Vector2f(100, 150));
                        if (limiteCartas.Contains(posMundo))
                        {
                            if (indicesSeleccionados.Contains(i))
                            {
                                indicesSeleccionados.Remove(i);
                            }
                            else
                            {
                                indicesSeleccionados.Add(i);
                            }
                            break; 
                        }
                    }
                }
                    
            }
        };
        while (ventana.IsOpen)
        {
           ventana.DispatchEvents();
           ventana.Clear(new Color(30,30,30));
           ventana.Draw(botonJugar);

           float margenIzquierdo = 100f;
           float anchoDisponible = ventanaAncho - (margenIzquierdo * 2);
           float anchoCarta = 100f;
           if (jugadorActual.Mano.Count > 1)
           {
               float separacionNecesaria = anchoDisponible / (jugadorActual.Mano.Count - 1);
               separacion = Math.Min(120f, separacionNecesaria);
           }

           for (int i = 0; i < jugadorActual.Mano.Count; i++)
           {
               RectangleShape rect = new RectangleShape(new Vector2f(anchoCarta, 150));
               float posX = margenIzquierdo + (i * separacion);
               rect.Position = new Vector2f(posX, 600);

               rect.FillColor = (indicesSeleccionados.Contains(i)) ? Color.Yellow : Color.Red;
               ventana.Draw(rect);
           }

           ventana.Display();
        }
        
    }
    private void ProcesarJugada(Jugador jugador)
    {
        
        foreach (int i in indicesSeleccionados.OrderByDescending(x => x))
        {
            jugador.Mano.RemoveAt(i);
        }
        indicesSeleccionados.Clear();
    }
   
}