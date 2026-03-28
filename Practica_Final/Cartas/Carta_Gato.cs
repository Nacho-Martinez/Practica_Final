using Practica_Final.Interfaces;

namespace Practica_Final.Cartas;

public class Carta_Gato : Carta,IJugada
{
    public Carta_Gato(string Nombre) : base("Gato Nomal")
    {
    }

    public void JugarCarta()
    {
        throw new NotImplementedException();
    }
}