using SFML.Graphics;

namespace Practica_Final.Cartas;

public abstract class Carta
{
    //Sprite
   public string Nombre { get; protected set; }
   public string Dibujo { get; set; }

   public Carta(string Nombre,string Dibujo ) 
   {
       this.Nombre = Nombre;
       this.Dibujo = Dibujo;
   }
}