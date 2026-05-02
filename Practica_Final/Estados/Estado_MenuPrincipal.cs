using Practica_Final.Interfaces;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Estados;

public class Estado_MenuPrincipal : IEstado
{
    private bool clickBloqueado = true;
    
    private RectangleShape fondo = new RectangleShape(new Vector2f(1200, 800))
    {
        FillColor = Color.White,
        Texture = SpritesManager.Instancia.ConseguirTextura("Sprites/Menu_Gatos.jpg")
    };
    private RectangleShape botonInicio = new RectangleShape(new Vector2f(500, 400))
    {
        FillColor = Color.Transparent,
        Position = new Vector2f(350,300)
        
    };
    private RectangleShape botonSalir = new RectangleShape(new Vector2f(300, 400))
    {
        FillColor = Color.Transparent,
        Position = new Vector2f(900,300)
        
    };
    

    public void Dibujar()
    {
        Interfaz.Instancia.Ventana.Draw(fondo);
        Interfaz.Instancia.Ventana.Draw(botonInicio);
        Interfaz.Instancia.Ventana.Draw(botonSalir);
    }

    public void Inputs()
    {
        if (!Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            clickBloqueado = false;
        }
        if (clickBloqueado) return;
        
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            Vector2i posPixel = Mouse.GetPosition(Interfaz.Instancia.Ventana);
            Vector2f posMundo = Interfaz.Instancia.Ventana.MapPixelToCoords(posPixel);

            if (botonInicio.GetGlobalBounds().Contains(posMundo))
            {
                StateManager.Intancia.CambiarEstado(StateManager.Estados.EleccionNombre);
            }
            if (botonSalir.GetGlobalBounds().Contains(posMundo))
            {
                Interfaz.Instancia.Ventana.Close();
            }
            
        }
        
    }

    public void ComportameintoIA()
    {
        
    }
}