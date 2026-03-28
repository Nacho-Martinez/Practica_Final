using Practica_Final.Interfaces;

namespace Practica_Final.Cartas;

public class Carta_Saltar : Carta,IJugada
{
    public Carta_Saltar(string Nombre) : base("Carta Saltar Turno")
    {
    }

    public void JugarCarta()
    {
        throw new NotImplementedException();
    }
}