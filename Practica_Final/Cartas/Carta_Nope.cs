using Practica_Final.Interfaces;

namespace Practica_Final.Cartas;

public class Carta_Nope : Carta,IJugada
{
    public Carta_Nope(string Nombre) : base("Nope")
    {
    }

    public void JugarCarta()
    {
        throw new NotImplementedException();
    }
}