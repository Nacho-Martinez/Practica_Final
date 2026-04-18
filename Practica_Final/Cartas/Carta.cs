using SFML.Graphics;

namespace Practica_Final.Cartas;

public abstract class Carta
{
   public string Nombre { get; protected set; }
   public string Dibujo { get; set; }

   public bool Resaltada { get; set; } = false;

   public Carta(string Nombre,string Dibujo ) 
   {
       this.Nombre = Nombre;
       this.Dibujo = Dibujo;
   }
}