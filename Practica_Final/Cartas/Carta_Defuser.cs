using Practica_Final.Interfaces;
using SFML.Graphics;

namespace Practica_Final.Cartas;

public class Carta_Defuser :Carta,IJugada
{
    public Carta_Defuser(string Nombre, string  Dibujo) : base(Nombre, Dibujo)
    {
    }

    public void JugarCarta()
    {
    }
}