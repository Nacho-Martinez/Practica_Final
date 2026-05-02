using System.IO.Compression;
using Practica_Final.Interfaces;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Estados;

public class Estado_FinPartida:IEstado
{
    private RectangleShape fondo = new RectangleShape(new Vector2f(1200, 800))
    {
        FillColor = Color.White,
        
    };
    private CircleShape botonSalir = new CircleShape(130)
    {
        FillColor = Color.Transparent,
        Position = new Vector2f(900,400)
        
    };
    private CircleShape botonJugar = new CircleShape(140)
    {
        FillColor = Color.Transparent,
        Position = new Vector2f(450,450)
        
    };
    
    public void Dibujar()
    {
        if (MotorJuego.Intancia.Victoria)
        {
            fondo.Texture = SpritesManager.Instancia.ConseguirTextura("Sprites/Victoria.jpg");
        }
        else
        {
            fondo.Texture = SpritesManager.Instancia.ConseguirTextura("Sprites/Derrota.jpg");
        }
        Interfaz.Instancia.Ventana.Draw(fondo);
        Interfaz.Instancia.Ventana.Draw(botonJugar);
        Interfaz.Instancia.Ventana.Draw(botonSalir);
        
    }

    public void Inputs()
    {
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            Vector2i posPixel = Mouse.GetPosition(Interfaz.Instancia.Ventana);
            Vector2f posMundo = Interfaz.Instancia.Ventana.MapPixelToCoords(posPixel);

            if (botonJugar.GetGlobalBounds().Contains(posMundo))
            {
                StateManager.Intancia.CambiarEstado(StateManager.Estados.MenuPrincipal);
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