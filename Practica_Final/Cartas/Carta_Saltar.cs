using Practica_Final.Interfaces;
using Practica_Final.Managers;
using SFML.Graphics;

namespace Practica_Final.Cartas;

public class Carta_Saltar : Carta,IJugada,IForzarFinTurno
{
    public Carta_Saltar(string Nombre, string  Dibujo) : base(Nombre, Dibujo)
    {
    }

    public void JugarCarta()
    {
        TurnManager.Instance.PasarTurnoSinRobar();
    }
}