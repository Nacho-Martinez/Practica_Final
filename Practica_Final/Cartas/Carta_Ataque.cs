using Practica_Final.Interfaces;
using Practica_Final.Jugadores;

namespace Practica_Final.Cartas;

public class Carta_Ataque : Carta ,IJugada,IObjetivo
{
    
    public Carta_Ataque(string Nombre) : base("Carta Ataque")
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