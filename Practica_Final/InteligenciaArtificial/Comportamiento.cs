using Practica_Final.Cartas;
using Practica_Final.Jugadores;

namespace Practica_Final.InteligenciaArtificial;

public abstract class Comportamiento
{
    public abstract void RellenarListas(Jugador_Robot robot);
    public abstract Carta[] CartasParaJugar();
    public abstract int NumeroDeJugadas();
}