using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Managers;

public class InputManager
{
    public static InputManager Instance = new InputManager();

    // public void ClicksRaton()
    // {
    //     // Interfaz.Instancia.Ventana.MouseButtonPressed += (sender, e) =>
    //     // {
    //     //
    //     //     // if (StateManager.Intancia.EstadoActual == StateManager.Estados.Normal)
    //     //     // {
    //     //     //  InputsEstadoNomral(e);
    //     //     // }
    //     //     if (StateManager.Intancia.EstadoActual == StateManager.Estados.DefusandoBomba)
    //     //     {
    //     //         InputsEstadoDefusando(e);
    //     //     }
    //     // };
    // }
    //
    //
    //
    // public void FlechasTeclado()
    // {
    //     Interfaz.Instancia.Ventana.KeyPressed += (sender, e) =>
    //     {
    //         // if (StateManager.Intancia.EstadoActual == StateManager.Estados.EsperandoAtaque)
    //         // {
    //         //  if (e.Code == Keyboard.Key.Up)
    //         //  {
    //         //     Interfaz.Instancia.ModificarIndiceEnemigo(1);
    //         //  }
    //         //
    //         //  if (e.Code == Keyboard.Key.Down)
    //         //  {
    //         //     Interfaz.Instancia.ModificarIndiceEnemigo(-1);
    //         //  }
    //         // }
    //
    //         // if (StateManager.Intancia.EstadoActual == StateManager.Estados.InsertandoBomba)
    //         // {
    //         //     if (e.Code == Keyboard.Key.Right)
    //         //     {
    //         //         Interfaz.Instancia.ModificarIndiceInsercion(+1);
    //         //     }
    //         //     if (e.Code == Keyboard.Key.Left)
    //         //     {
    //         //         Interfaz.Instancia.ModificarIndiceInsercion(-1);
    //         //     }
    //         // }
    //
    //     };
    //
    // }
    //
    // public void TeclaEnter()
    // {
    //     Interfaz.Instancia.Ventana.KeyPressed += (sender, e) =>
    //     {
    //         if (StateManager.Intancia.EstadoActual == StateManager.Estados.EsperandoAtaque)
    //         {
    //          // if (e.Code == Keyboard.Key.Enter)
    //          // {
    //          //    EventManager.Instancia.JugadorSeleccionado();
    //          // }
    //         }
    //
    //         if (StateManager.Intancia.EstadoActual == StateManager.Estados.InsertandoBomba)
    //         {
    //             if (e.Code == Keyboard.Key.Enter)
    //             {
    //              EventManager.Instancia.Insercion(Interfaz.Instancia.IndiceInsercion);
    //              Interfaz.Instancia.ResterarIndiceInsercion();
    //              StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
    //              EventManager.Instancia.SiguienteTurno();
    //              TurnManager.Instance.PasarTurno();
    //                 
    //             }
    //         }
    //     };
    // }

    public void ProcesarJugada(Jugador jugador)
        {
        
            foreach (int i in Interfaz.Instancia.IndicesSeleccionados.OrderByDescending(x => x))
            {
                jugador.Mano.RemoveAt(i);
            }
            Interfaz.Instancia.IndicesSeleccionados.Clear();
        }
    
    private void InputsEstadoNomral(MouseButtonEventArgs e)
    {
        // if (e.Button == Mouse.Button.Left)
        // {
        //     List<Carta> cartasElegidas = new();
        //     Vector2i posPixel = Mouse.GetPosition(Interfaz.Instancia.Ventana);
        //     Vector2f posMundo = Interfaz.Instancia.Ventana.MapPixelToCoords(posPixel);
        //     if (Interfaz.Instancia.BotonJugar.GetGlobalBounds().Contains(posMundo))
        //     {
        //         if (Interfaz.Instancia.IndicesSeleccionados.Count == 0)
        //             return;
        //         foreach (var indice in Interfaz.Instancia.IndicesSeleccionados)
        //         {
        //             cartasElegidas.Add(Interfaz.Instancia.JugadorActual.Mano[indice]);
        //         }
        //
        //         int cantidad = cartasElegidas.Count;
        //         if (cantidad == 1)
        //         {
        //             Console.WriteLine($"[LOG] Jugando carta simple: {cartasElegidas[0].Nombre}");
        //             if (cartasElegidas[0] is IJugada cartaParaJugar)
        //             {
        //                 cartaParaJugar.JugarCarta();
        //             }
        //             ProcesarJugada(Interfaz.Instancia.JugadorActual);
        //         }
        //         //AQUI VAN LOS FILTROS PARA LAS CARTAS DE GATOS ETC
        //         else
        //         {
        //             Console.WriteLine($"[LOG] Logica aun no hecha");
        //         }
        //
        //     }
        //     else if (Interfaz.Instancia.BotonSaltarTurno.GetGlobalBounds().Contains(posMundo))
        //     {
        //         TurnManager.Instance.PasarTurno();
        //     }
        //     else if (posMundo.Y > 600 && posMundo.Y < 750)
        //     {
        //         for (int i = Interfaz.Instancia.JugadorActual.Mano.Count - 1; i >= 0; i--)
        //         {
        //             float posX = 100 + (i * Interfaz.Instancia.Separacion);
        //             float posY = Interfaz.Instancia.IndicesSeleccionados.Contains(i) ? 570f : 600f;
        //             FloatRect limiteCartas = new FloatRect(new Vector2f(posX, posY), new Vector2f(100, 150));
        //             if (limiteCartas.Contains(posMundo))
        //             {
        //                 if (Interfaz.Instancia.IndicesSeleccionados.Contains(i))
        //                 {
        //                     Interfaz.Instancia.IndicesSeleccionados.Remove(i);
        //                 }
        //                 else
        //                 {
        //                     Interfaz.Instancia.IndicesSeleccionados.Add(i);
        //                 }
        //
        //                 break;
        //             }
        //         }
        //     }
        //
        // }
    }
    
    private void InputsEstadoDefusando(MouseButtonEventArgs e)
    {
    //     if (e.Button == Mouse.Button.Left)
    //     {
    //         List<Carta> cartasElegidas = new();
    //         Vector2i posPixel = Mouse.GetPosition(Interfaz.Instancia.Ventana);
    //         Vector2f posMundo = Interfaz.Instancia.Ventana.MapPixelToCoords(posPixel);
    //         if (Interfaz.Instancia.BotonJugar.GetGlobalBounds().Contains(posMundo))
    //         {
    //             if (Interfaz.Instancia.IndicesSeleccionados.Count == 0)
    //                 return;
    //             foreach (var indice in Interfaz.Instancia.IndicesSeleccionados)
    //             {
    //                 cartasElegidas.Add(Interfaz.Instancia.JugadorActual.Mano[indice]);
    //             }
    //
    //             int cantidad = cartasElegidas.Count;
    //             if (cantidad == 1)
    //             {
    //                 Console.WriteLine($"[LOG] Jugando carta simple: {cartasElegidas[0].Nombre}");
    //                 if (cartasElegidas[0] is Carta_Defuser && cartasElegidas[0] is IJugada cartaParaJugar)
    //                 { cartaParaJugar.JugarCarta();
    //                   ProcesarJugada(Interfaz.Instancia.JugadorActual);
    //                 }
    //             }
    //             else
    //             {
    //                 return;
    //             }
    //
    //         }
    //         else if (posMundo.Y > 600 && posMundo.Y < 750)
    //         {
    //             for (int i = Interfaz.Instancia.JugadorActual.Mano.Count - 1; i >= 0; i--)
    //             {
    //                 float posX = 100 + (i * Interfaz.Instancia.Separacion);
    //                 float posY = Interfaz.Instancia.IndicesSeleccionados.Contains(i) ? 570f : 600f;
    //                 FloatRect limiteCartas = new FloatRect(new Vector2f(posX, posY), new Vector2f(100, 150));
    //                 if (limiteCartas.Contains(posMundo))
    //                 {
    //                     if (Interfaz.Instancia.IndicesSeleccionados.Contains(i))
    //                     {
    //                         Interfaz.Instancia.IndicesSeleccionados.Remove(i);
    //                     }
    //                     else
    //                     {
    //                         Interfaz.Instancia.IndicesSeleccionados.Add(i);
    //                     }
    //
    //                     break;
    //                 }
    //             }
    //         }
    //
    //     }
     }
}