using Practica_Final.Cartas;

namespace Practica_Final.Jugadores;

public class Jugador_Humano : Jugador
{
    public override void IniciarTurno()
    {
        throw new NotImplementedException();
    }

    public override void RobarCarta(Carta carta)
    {
        Mano.Add(carta);
    }
}