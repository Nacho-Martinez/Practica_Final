using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;

namespace Practica_Final.Estados;

public class Estado_ViendoFuturo : IEstado
{
    private  Clock cronometroFuturo = new Clock();
    private bool relojIniciado = false;
    
    public void Dibujar()
    {
        if (!relojIniciado)
        {
            cronometroFuturo.Restart();
            relojIniciado = true;
            Console.WriteLine("[DEBUG] Entrando en ViendoFuturo - Cronómetro iniciado");
        }
        if (TurnManager.Instance.JugadorActual is Jugador_Robot)
        {
            
            RectangleShape panelFondoIa = new RectangleShape(new Vector2f(1200, 800));
            panelFondoIa.FillColor = Color.White;
            panelFondoIa.Texture = SpritesManager.Instancia.ConseguirTextura(
                 "C:\\Users\\nache\\OneDrive\\Desktop\\POO\\Practica_Final\\Practica_Final\\Sprites\\VerFuturoRobot.png");
            Interfaz.Instancia.Ventana.Draw(panelFondoIa);
        }
        else
        {
            
            List<Carta> listaTemp = new();
            listaTemp = Mazo<Carta>.Instancia.DevolverMazoTemporal();
            RectangleShape panelFondo = new RectangleShape(new Vector2f(900, 700));
            panelFondo.Position = new Vector2f(150, 50);
            panelFondo.FillColor = new Color(128, 0, 128, 150); //Morado Semitransparente
            panelFondo.OutlineColor = Color.Black;
            panelFondo.OutlineThickness = 3;
            Interfaz.Instancia.Ventana.Draw(panelFondo);

            for (int i = 0; i < 3; i++)
            {
                if (i >= listaTemp.Count) break;
                RectangleShape carta = new RectangleShape(new Vector2f(293.33f, 300));
                carta.Position = new Vector2f(i * 293.33f + 150 + (10), 200);
                carta.Texture = SpritesManager.Instancia.ConseguirTextura(listaTemp[i].Dibujo);
                Interfaz.Instancia.Ventana.Draw(carta);

            }
        }
        if (cronometroFuturo.ElapsedTime.AsSeconds() > 5f)
        {
            relojIniciado = false;
            StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
        }
    }

    public void Inputs()
    {
        //No Puedes hacer inputs de momento en este estado
    }

    public void ComportameintoIA()
    {
    }
}