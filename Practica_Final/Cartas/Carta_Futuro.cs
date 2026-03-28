using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using SFML.Graphics;

namespace Practica_Final.Cartas;

public class Carta_Futuro : Carta ,IJugada
{
    public Carta_Futuro(string Nombre, string  Dibujo) : base(Nombre, Dibujo)
    {
    }

    public void JugarCarta()
    {
        
    }
}