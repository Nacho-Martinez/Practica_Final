using Practica_Final.Interfaces;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Estados;

public class Estado_EleccionDificultad : IEstado
{
    private int indiceDificultad = 0;
    private RectangleShape fondo = new RectangleShape(new Vector2f(1200, 800))
    {
        FillColor = Color.White,
        Texture = SpritesManager.Instancia.ConseguirTextura("Sprites/EleccionDificultad.jpg")
    };
    private bool teclaBloqueada = true;
    private RectangleShape botonSalir = new RectangleShape(new Vector2f(300, 400))
    {
        FillColor = Color.Transparent,
        Position = new Vector2f(900,300)
        
    };

    
    public void Dibujar()
    {
        Interfaz.Instancia.Ventana.Draw(fondo);
        for (int i = 0; i < 3; i++)
        {
            RectangleShape nuevaCasilla = new RectangleShape(new Vector2f(330, 120));
            float posY = 230 + (i * 130);
            float posX = 435;
            nuevaCasilla.Position = new Vector2f(posX, posY);
            nuevaCasilla.FillColor = Color.Transparent;
            if (i == indiceDificultad)
            {
                nuevaCasilla.OutlineThickness = 3;
                nuevaCasilla.OutlineColor = Color.Red;
            }
            else
            {
                nuevaCasilla.OutlineThickness = 0;
                nuevaCasilla.OutlineColor = Color.Transparent;
            }
            Interfaz.Instancia.Ventana.Draw(nuevaCasilla);
        }
    }

    public void Inputs()
    {
        bool algunPulsado = Keyboard.IsKeyPressed(Keyboard.Key.Up) || 
                            Keyboard.IsKeyPressed(Keyboard.Key.Down) || 
                            Keyboard.IsKeyPressed(Keyboard.Key.Enter);
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            Vector2i posPixel = Mouse.GetPosition(Interfaz.Instancia.Ventana);
            Vector2f posMundo = Interfaz.Instancia.Ventana.MapPixelToCoords(posPixel);
            if (botonSalir.GetGlobalBounds().Contains(posMundo))
            {
                Interfaz.Instancia.Ventana.Close();
            }
            
        }
        if (!algunPulsado)
        {
            teclaBloqueada = false;
            return;
        }
        if (teclaBloqueada) return;
        
            
            
                if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    ModificarIndiceDificultad(1);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    ModificarIndiceDificultad(-1);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
                {
                    StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
                    ElJuego.Instancia.haEmpezadoElJuego = true;
                    switch (indiceDificultad)
                    {
                        case 0:
                            ElJuego.Instancia.Inicializar(ElJuego.dificultades.Facil);
                            break;
                        case 1:
                            ElJuego.Instancia.Inicializar(ElJuego.dificultades.Medio);
                            break;
                        case 2:
                            ElJuego.Instancia.Inicializar(ElJuego.dificultades.Dificil);
                            break;
                    }
                }
                teclaBloqueada = true; 
    }

    public void ComportameintoIA()
    {
    }
    
    public void ModificarIndiceDificultad(int clave)
    {

        indiceDificultad = clave > 0
            ? (indiceDificultad - 1 + 3 ) % 3
            : (indiceDificultad + 1) % 3;
    }
}