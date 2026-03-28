using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using SFML.Graphics;

namespace Practica_Final.Cartas;

public class Carta_Favor :Carta,IJugada,IObjetivo
{
    public Carta_Favor(string Nombre, string  Dibujo) : base(Nombre, Dibujo)
    {
    }

    public void JugarCarta()
    {
        throw new NotImplementedException();
    }

    public Jugador ElegirObjetivo()
    {
        throw new NotImplementedException();
    }
}