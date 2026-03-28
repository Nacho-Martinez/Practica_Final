namespace Practica_Final.Cartas;

public abstract class Carta
{
    //Sprite
   public string Nombre { get; protected set; }

   public Carta(string Nombre ) //Falta el sprite
   {
       this.Nombre = Nombre;
   }
}