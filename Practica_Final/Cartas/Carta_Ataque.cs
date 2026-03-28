using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using SFML.Graphics;

namespace Practica_Final.Cartas;

public class Carta_Ataque : Carta ,IJugada,IObjetivo
{
    public Carta_Ataque(string Nombre, string  Dibujo) : base(Nombre, Dibujo)
    {
    }

    public void JugarCarta()
    {
        
    }

    public Jugador ElegirObjetivo()
    {
        return null;
    }
}