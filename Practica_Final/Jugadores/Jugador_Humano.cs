using Practica_Final.Cartas;

namespace Practica_Final.Jugadores;

public class Jugador_Humano : Jugador
{
    public Jugador_Humano(string nombre) : base(nombre)
    {
    }

    public override void IniciarTurno()
    {
        throw new NotImplementedException();
    }

    public override void RobarCarta()
    {
        throw new NotImplementedException();
    }
}