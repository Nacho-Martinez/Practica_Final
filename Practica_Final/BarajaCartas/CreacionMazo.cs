using Practica_Final.Cartas;

namespace Practica_Final.BarajaCartas;

public class CreacionMazo
{
    public static CreacionMazo Instancia = new ();
    private Stack<Carta> barajaTemporal = new();

    public Stack<Carta> PrepararMazo(int numeroJugadores)
    {
        AgregarVarias(4,() => new Carta_Ataque("Carta_Ataque"));
        AgregarVarias(4,() => new Carta_Favor("Carta_Favor"));
        AgregarVarias(4,() => new Carta_Barajar("Carta_Barajar"));
        AgregarVarias(4,() => new Carta_Saltar("Carta_Saltar"));
        AgregarVarias(5,()=> new Carta_Futuro("Carta_Futuro"));
        AgregarVarias(5,() => new Carta_Nope("Nope"));
        string[] tiposGatos = { "GatoTaco", "GatoSandia", "GatoBarba", "GatoPatata" };
        foreach (var tipos in tiposGatos)
        {
            AgregarVarias(4,() => new Carta_Gato(tipos));
        }
        int defuserSobrantes = 6 - numeroJugadores;
        
        AgregarVarias(defuserSobrantes,() => new Carta_Defuser("Defuser"));
        AgregarVarias(numeroJugadores-1,() => new Carta_Explosion("GatoExplosivo"));

        return barajaTemporal;

    }

    private void AgregarVarias(int cantidad, Func<Carta> creador)
    {
        for (int i = 0; i < cantidad; i++)
        {
            barajaTemporal.Push(creador());
        }
    }
}