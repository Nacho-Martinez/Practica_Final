using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.Interfaces;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Estados;

public class Estado_InsertandoBomba : IEstado
{
    private Random rand = new Random();
    private bool teclaBloqueada = false;
    public void Dibujar()
    {
        
        RectangleShape panelFondo = new RectangleShape(new Vector2f(400, 300));
        panelFondo.Position = new Vector2f(400, 250);
        panelFondo.FillColor = Color.Black; 
        panelFondo.OutlineColor = Color.Black;
        panelFondo.OutlineThickness = 3;
        Interfaz.Instancia.Ventana.Draw(panelFondo);
        
        ConvexShape flechaDerecha = new ConvexShape(3);
        flechaDerecha.SetPoint(0, new Vector2f(20, 0)); 
        flechaDerecha.SetPoint(1, new Vector2f(-20, -20)); 
        flechaDerecha.SetPoint(2, new Vector2f(-20, 20)); 
        flechaDerecha.FillColor = Color.White;
        flechaDerecha.Position = new Vector2f(625, 300);
        Interfaz.Instancia. Ventana.Draw(flechaDerecha);
        ConvexShape flechaIzquierda = new ConvexShape(3);
        flechaIzquierda.SetPoint(0, new Vector2f(-20, 0)); 
        flechaIzquierda.SetPoint(1, new Vector2f(20, -20)); 
        flechaIzquierda.SetPoint(2, new Vector2f(20, 20)); 
        flechaIzquierda.FillColor = Color.White;
        flechaIzquierda.Position = new Vector2f(425, 300);
        Interfaz.Instancia.Ventana.Draw(flechaIzquierda);
        int indiceVisual = Mazo<Carta>.Instancia.Baraja.Count -  Interfaz.Instancia.IndiceInsercion;
        Text numero = new Text( Interfaz.Instancia.Fuente, $"{indiceVisual}");
        numero.Position = new Vector2f(525, 275);
        Interfaz.Instancia.Ventana.Draw(numero);
    }

    public void Inputs()
    {
        bool teclaPulsada = Keyboard.IsKeyPressed(Keyboard.Key.Right) || 
                            Keyboard.IsKeyPressed(Keyboard.Key.Left) || 
                            Keyboard.IsKeyPressed(Keyboard.Key.Enter);
        
        if (teclaPulsada)
        {
            if (!teclaBloqueada)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                {
                    Interfaz.Instancia.ModificarIndiceInsercion(1);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                {
                    Interfaz.Instancia.ModificarIndiceInsercion(-1);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
                {
                    EventManager.Instancia.Insercion(Interfaz.Instancia.IndiceInsercion);
                    Interfaz.Instancia.ResterarIndiceInsercion();
                    Interfaz.Instancia.IndicesSeleccionados.Clear();
                    StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
                    EventManager.Instancia.SiguienteTurnoParaRobar();
                    TurnManager.Instance.PasarTurno();
                }

                teclaBloqueada = true; 
            }
        }
        else
        {
            teclaBloqueada = false; 
        }
    }

    public void ComportameintoIA()
    {
        int randNum = rand.Next(0, Mazo<Carta>.Instancia.Baraja.Count);
        EventManager.Instancia.Insercion(randNum);
        StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
        EventManager.Instancia.SiguienteTurnoParaRobar();
        TurnManager.Instance.PasarTurno();
    }
}