using Practica_Final.Interfaces;

namespace Practica_Final.Cartas;

public class Carta_Barajar :Carta,IJugada
{
    public Carta_Barajar(string Nombre) : base("Carta Barajar")
    {
    }

    public void JugarCarta()
    {
        throw new NotImplementedException();
    }
}