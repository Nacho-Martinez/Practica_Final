using Practica_Final.Interfaces;
using Practica_Final.Managers;
using SFML.Graphics;

namespace Practica_Final.Cartas;

public class Carta_Defuser :Carta,IJugada
{
    public Carta_Defuser(string Nombre, string  Dibujo) : base(Nombre, Dibujo)
    {
    }

    public void JugarCarta()
    {
        StateManager.Intancia.CambiarEstado(StateManager.Estados.InsertandoBomba);
    }
}