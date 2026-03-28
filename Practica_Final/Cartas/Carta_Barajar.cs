using Practica_Final.Interfaces;
using SFML.Graphics;

namespace Practica_Final.Cartas;

public class Carta_Barajar :Carta,IJugada
{
    public Carta_Barajar(string Nombre, string  Dibujo) : base(Nombre, Dibujo)
    {
    }

    public void JugarCarta()
    {
        throw new NotImplementedException();
    }
}