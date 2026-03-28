using Practica_Final.Interfaces;
using SFML.Graphics;

namespace Practica_Final.Cartas;

public class Carta_Gato : Carta,IJugada
{
    public Carta_Gato(string Nombre, string  Dibujo) : base(Nombre, Dibujo)
    {
    }

    public void JugarCarta()
    {
        throw new NotImplementedException();
    }
}