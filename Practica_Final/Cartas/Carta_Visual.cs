using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;

namespace Practica_Final.Cartas;

public class Carta_Visual
{
     public Carta Logica { get; private set; }
     public Sprite Dibujo { get; private set; }
     private Vector2f offset;

     public Carta_Visual(Carta logica)
     {
          Logica = logica;
          Dibujo = new Sprite(SpritesManager.Instancia.ConseguirTextura(logica.Dibujo));
          Dibujo.Origin = new Vector2f(Dibujo.TextureRect.Width / 2, Dibujo.TextureRect.Height / 2);
     }

     public bool ContienePunto(Vector2f punto)
     {
          return Dibujo.GetGlobalBounds().Contains(punto);
     }

     public void SeguirRaton(Vector2f posRaton)
     {
          Dibujo.Position = posRaton;
     }

     public void Dibujar(RenderWindow ventana)
     {
          ventana.Draw(Dibujo);
     }
}