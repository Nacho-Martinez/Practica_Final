using Practica_Final.Cartas;
using Practica_Final.InteligenciaArtificial;
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
        Interfaz.Instancia.DibujarRivales();
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
                            cartasElegidas.Add( jugadorActual.Mano[indice]);
                        }

                        int cantidad = cartasElegidas.Count;
                        if (cantidad == 1)
                        {
                            Carta cartaJugada = cartasElegidas[0];
                            Console.WriteLine($"[LOG] Jugando carta simple: {cartasElegidas[0].Nombre}");
                            
                            if (cartasElegidas[0] is not IJugada cartaParaJugar || cartasElegidas[0] is Carta_Gato ||
                                cartasElegidas[0] is Carta_Defuser || cartasElegidas[0] is Carta_Nope) return;
                            
                            int indiceEnMano = Interfaz.Instancia.IndicesSeleccionados[0];
                            float posXInicio = 100f + (indiceEnMano * Interfaz.Instancia.Separacion);
                            float posYInicio = 570f; 
                            Vector2f posInicio = new Vector2f(posXInicio, posYInicio);
                            Vector2f posDestino = new Vector2f(720f, 300f); 
                            Texture tex = SpritesManager.Instancia.ConseguirTextura(cartaJugada.Dibujo);
                            
                            Interfaz.Instancia.LanzarAnimacion(tex, posInicio, posDestino, 0.4f, () => 
                            {
                                
                                ReactManager.Instance.MeterJugadaEnCola(cartaJugada);
                                ReactManager.Instance.ProcesarJugada(TurnManager.Instance.JugadorActual);
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
                                
                                int indice1 = Interfaz.Instancia.IndicesSeleccionados[0];
                                int indice2 = Interfaz.Instancia.IndicesSeleccionados[1];
                                
                                Vector2f posInicio1 = new Vector2f(100f + (indice1 * Interfaz.Instancia.Separacion), 570f);
                                Vector2f posInicio2 = new Vector2f(100f + (indice2 * Interfaz.Instancia.Separacion), 570f);
                                Vector2f posDestino1 = new Vector2f(720f, 300f);
                                Vector2f posDestino2 = new Vector2f(820f, 300f);
                                Texture tex1 = SpritesManager.Instancia.ConseguirTextura(cartasElegidas[0].Dibujo);
                                Texture tex2 = SpritesManager.Instancia.ConseguirTextura(cartasElegidas[1].Dibujo);
                                
                                Interfaz.Instancia.LanzarAnimacion(tex2, posInicio2, posDestino2, 0.4f);
                                Interfaz.Instancia.LanzarAnimacion(tex1, posInicio1, posDestino1, 0.4f, () => 
                                {
                                    ReactManager.Instance.MeterJugadaEnCola(cartasElegidas[0]);
                                    ReactManager.Instance.ProcesarJugada(TurnManager.Instance.JugadorActual);
                                    StateManager.Intancia.CambiarEstado(StateManager.Estados.EsperandoTrasJugada);
                                });
                            }
                        }

                    }
                    else if (Interfaz.Instancia.BotonSaltarTurno.GetGlobalBounds().Contains(posMundo))
                    {
                        TurnManager.Instance.PasarTurno();
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
        Console.WriteLine($"[DEBUG ComportamientoIA] JugadorActual: {TurnManager.Instance.JugadorActual.Nombre}");
        if (ElJuego.Instancia.HayTurnoSinRobarPendiente) return;
        if (ElJuego.Instancia.TurnoConfirmadoPendiente) return;
        if (TurnManager.Instance.JugadorActual is Jugador_Robot iaActual)
        {
            iaActual.JugarCarta();
        }
    }
    
}