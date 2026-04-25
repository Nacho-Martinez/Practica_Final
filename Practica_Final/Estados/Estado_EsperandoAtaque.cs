using Practica_Final.Interfaces;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Estados;

public class Estado_EsperandoAtaque: IEstado
{
    
    private bool teclaBloqueada = false;
    
    public void Dibujar()
    {
        RectangleShape panelFondo = new RectangleShape(new Vector2f(1000, 700));
        panelFondo.Position = new Vector2f(100, 50);
        panelFondo.FillColor = new Color(128, 128, 128, 150); //Gris SemiTransparente
        panelFondo.OutlineColor = Color.Black;
        panelFondo.OutlineThickness = 3;
        Interfaz.Instancia.Ventana.Draw(panelFondo);
        Text titulo = new Text(Interfaz.Instancia.Fuente, ">> Elige a un Jugador<<");
        titulo.CharacterSize = 24;
        titulo.Position = new Vector2f(150, 100);
        Interfaz.Instancia.Ventana.Draw(titulo);
        for(int i =0; i<TurnManager.Instance.jugadoresVivos.Count;i++)
        {
            RectangleShape nuevaCasilla = new RectangleShape(new Vector2f(300, 90));
            float posY = 200 + (i * 100);
            float posX = 100;
            nuevaCasilla.Position = new Vector2f(posX, posY);
            nuevaCasilla.FillColor = Color.Transparent;
            if (i == Interfaz.Instancia.IndiceEnemigo)
            {
                nuevaCasilla.OutlineThickness = 3;
                nuevaCasilla.OutlineColor = Color.Yellow;
            }
            else
            {
                nuevaCasilla.OutlineThickness = 0;
                nuevaCasilla.OutlineColor = Color.Transparent;
            }
            Text nombreJugador = new Text(Interfaz.Instancia.Fuente, TurnManager.Instance.jugadoresVivos[i].Nombre);
            nombreJugador.Position = new Vector2f(posX+15, posY);
            Interfaz.Instancia.Ventana.Draw(nuevaCasilla);
            Interfaz.Instancia.Ventana.Draw(nombreJugador);
        }
    }

    public void Inputs()
    {
        bool algunPulsado = Keyboard.IsKeyPressed(Keyboard.Key.Up) || 
                            Keyboard.IsKeyPressed(Keyboard.Key.Down) || 
                            Keyboard.IsKeyPressed(Keyboard.Key.Enter);
                if (algunPulsado)
                {
                    if (!teclaBloqueada)
                    {
                        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                        {
                            Interfaz.Instancia.ModificarIndiceEnemigo(1);
                        }
                        else if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                        {
                            Interfaz.Instancia.ModificarIndiceEnemigo(-1);
                        }
                        else if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
                        {
                            EventManager.Instancia.JugadorSeleccionado();
                            
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
        throw new NotImplementedException();
    }
}