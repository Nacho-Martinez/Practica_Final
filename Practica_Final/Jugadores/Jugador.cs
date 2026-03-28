namespace Practica_Final.Jugadores;

using Practica_Final.Cartas;
public abstract class Jugador
{
    public string Nombre { get; protected set; }
    public List<Carta> Mano { get; set; }
    public bool Eliminado { get; private set; }

    public abstract void IniciarTurno();
    public abstract void RobarCarta(Carta carta);
}