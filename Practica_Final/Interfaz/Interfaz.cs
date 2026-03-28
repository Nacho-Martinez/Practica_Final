using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Interfaz;

public class Interfaz
{
    private uint ventanaAncho = 1200;
    private uint ventanaAlto = 800;


    public void GenerarVentana()
    {
        RenderWindow ventana = new RenderWindow(new VideoMode(new Vector2u(ventanaAncho,ventanaAlto),32), "Exploding Kittens");
        ventana.Closed += (_, _) => ventana.Close();
        ventana.SetFramerateLimit(60);

        while (ventana.IsOpen)
        {
            
        }
        
    }
}