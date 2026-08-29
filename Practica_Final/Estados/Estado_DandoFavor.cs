using Practica_Final.Cartas;
using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Estados;

public class Estado_DandoFavor : IEstado
{
    private bool mousePulsado = false;
    private Random rand = new Random();
    private bool iaHaDado = false;
    public void Dibujar()
    {
        Jugador jugadorActual = TurnManager.Instance.JugadorActual;
        if (TurnManager.Instance.JugadorActual is not Jugador_Robot)
        {

            RectangleShape panelTexto = new RectangleShape(new Vector2f(600, 200));
            panelTexto.Position = new Vector2f(400, 300);
            panelTexto.FillColor = new Color(128, 128, 128, 150); //Gris SemiTransparente
            panelTexto.OutlineColor = Color.Black;
            panelTexto.OutlineThickness = 3;
            Text texto = new Text(Interfaz.Instancia.Fuente, ">>ELIGE CARTA PARA DAR<<");
            texto.Position = new Vector2f(410, 350);
            Interfaz.Instancia.Ventana.Draw(panelTexto);
            Interfaz.Instancia.Ventana.Draw(texto);
        }

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
                    cartasElegidas.Add(TurnManager.Instance.JugadorActual.Mano[indice]);
                }

                int cantidad = cartasElegidas.Count;
                if (cantidad == 1)
                {
                    Carta cartaElegida = cartasElegidas[0];     
                    Vector2f posInicio = Interfaz.Instancia.ObtenerPosicionRobot(TurnManager.Instance.JugadorActual);
                    Vector2f posDestino = Interfaz.Instancia.ObtenerPosicionRobot(TurnManager.Instance.JugadorPendienteDeFavor);
                    Texture tex = SpritesManager.Instancia.ConseguirTextura(cartaElegida.Dibujo);
                    StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
                    Interfaz.Instancia.LanzarAnimacion(tex, posInicio, posDestino, 0.4f, () => 
                    {
                     TurnManager.Instance.JugadorPendienteDeFavor.Mano.Add(cartaElegida);
                     TurnManager.Instance.JugadorActual.Mano.Remove(cartaElegida);
                     EventManager.Instancia.CartaDada();
                    });
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
        if (iaHaDado)
        {
            //Console.WriteLine($"iaHaDado en estado: {iaHaDado} volviendo");
            return;
        }
        if (TurnManager.Instance.JugadorActual.Mano.Count == 0)
        {
            EventManager.Instancia.CartaDada();
            return;
        }

        iaHaDado = true;
        int numeroAleatorio = rand.Next(0, TurnManager.Instance.JugadorActual.Mano.Count);
        Carta cartaParaDar = TurnManager.Instance.JugadorActual.Mano[numeroAleatorio];
        Vector2f posInicio = Interfaz.Instancia.ObtenerPosicionRobot(TurnManager.Instance.JugadorActual);
        Vector2f posDestino = Interfaz.Instancia.ObtenerPosicionRobot(TurnManager.Instance.JugadorPendienteDeFavor);
        Texture tex = SpritesManager.Instancia.ConseguirTextura(cartaParaDar.Dibujo);
        TurnManager.Instance.JugadorActual.Mano.Remove(cartaParaDar);
        Interfaz.Instancia.LanzarAnimacion(tex, posInicio, posDestino, 0.4f, () => 
        {
         TurnManager.Instance.JugadorPendienteDeFavor.Mano.Add(cartaParaDar);
         EventManager.Instancia.CartaDada();
         Resetear();
        });
        
        

    }
    public void Resetear()
    {
        //Console.WriteLine("Reseteando");
        iaHaDado = false;
    }
}