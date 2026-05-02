using Practica_Final.Interfaces;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Estados;

public class Estado_EleccionNombre : IEstado
{
    private string nombreEscrito = "";
    private bool enterPulsado = false;
    
    private RectangleShape fondo = new RectangleShape(new Vector2f(1200, 800))
    {
        FillColor = Color.White,
        Texture = SpritesManager.Instancia.ConseguirTextura("Sprites/ElegirNombre.jpg")
    };
    private RectangleShape botonSalir = new RectangleShape(new Vector2f(300, 400))
    {
        FillColor = Color.Transparent,
        Position = new Vector2f(900,300)
        
    };
    public Estado_EleccionNombre()
    {
        Interfaz.Instancia.Ventana.TextEntered += AlEscribirTexto;
        Interfaz.Instancia.Ventana.KeyPressed += AlPulsarTecla;
    }

    private void AlPulsarTecla(object? sender, KeyEventArgs e)
    {
        if (e.Code == Keyboard.Key.Backspace && nombreEscrito.Length > 0)
        {
            nombreEscrito = nombreEscrito.Substring(0, nombreEscrito.Length - 1);
        }
        else if (e.Code == Keyboard.Key.Enter && nombreEscrito.Length > 0)
        {
            enterPulsado = true;
        }
    }

    private void AlEscribirTexto(object? sender, TextEventArgs e)
    {
        string texto = e.Unicode;
        if (texto.Length == 0) return;
        char c = texto[0];
        if (c == '\b' || c == '\r' || c == '\n') return;
        if (!char.IsControl(c) && nombreEscrito.Length < 12)
        {
            nombreEscrito += c;
        }
    }

    public void Dibujar()
    {
        Interfaz.Instancia.Ventana.Draw(fondo);
        string cursor = (DateTime.Now.Millisecond < 500) ? "_" : " ";
        Text textoUsuario = new Text(Interfaz.Instancia.Fuente,nombreEscrito + cursor,36);
        textoUsuario.Origin = new Vector2f(textoUsuario.GetLocalBounds().Width / 2f, 15);
        textoUsuario.Position = new Vector2f(550,500);
        textoUsuario.FillColor = Color.Black;
        textoUsuario.OutlineThickness = 1;
        textoUsuario.LetterSpacing = 2;
        Interfaz.Instancia.Ventana.Draw(textoUsuario);
    }

    public void Inputs()
    {
        if (enterPulsado)
        {
            enterPulsado = false;
            FinalizarYEmpezar();
        }
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            Vector2i posPixel = Mouse.GetPosition(Interfaz.Instancia.Ventana);
            Vector2f posMundo = Interfaz.Instancia.Ventana.MapPixelToCoords(posPixel);
            if (botonSalir.GetGlobalBounds().Contains(posMundo))
            {
                Interfaz.Instancia.Ventana.Close();
            }
            
        }
    }

    private void FinalizarYEmpezar()
    {
        Interfaz.Instancia.Ventana.TextEntered -= AlEscribirTexto;
        Interfaz.Instancia.Ventana.KeyPressed -= AlPulsarTecla;
        
        ElJuego.Instancia.AsignarNombreJugador(nombreEscrito);
        StateManager.Intancia.CambiarEstado(StateManager.Estados.EleccionDificultad);
    }

    public void ComportameintoIA()
    {
    }
}