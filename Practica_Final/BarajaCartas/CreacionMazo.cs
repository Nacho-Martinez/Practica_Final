using Practica_Final.Cartas;

namespace Practica_Final.BarajaCartas;

public class CreacionMazo
{
    public static CreacionMazo Instancia { get; private set; }= new ();
    private Stack<Carta> barajaTemporal = new();
    private Random rand = new Random();
    public string[] dibujosDefuser { get; private set; }= { "Practica_Final/Sprites/Defuser1.png", "Practica_Final/Sprites/Defuser2.png"};

    public Stack<Carta> PrepararMazo(int numeroJugadores)
    {
        string[] dibujosAtaque = { "Practica_Final/Sprites/Ataque1.png", "Practica_Final/Sprites/Ataque2.png"};
        string[] dibujosFavor = { "Practica_Final/Sprites/Favor1.png", "Practica_Final/Sprites/Favor2.png"};
        string[] dibujosBarajar = { "Practica_Final/Sprites/Barajar1.png", "Practica_Final/Sprites/Barajar2.png"};
        string[] dibujosSaltar = { "Practica_Final/Sprites/Saltar1.png", "Practica_Final/Sprites/Saltar2.png"};
        string[] dibujosFuturo = { "Practica_Final/Sprites/Futuro1.png", "Practica_Final/Sprites/Futuro2.png"};
        string[] dibujosNope = { "Practica_Final/Sprites/Nope1.png", "Practica_Final/Sprites/Nope2.png"};
        string[] dibujosExplosion = { "Practica_Final/Sprites/Explosion.png"};
        string[] dibujosGato = { "Practica_Final/Sprites/GatoTaco.png" ,"Practica_Final/Sprites/GatoSandia.png","Practica_Final/Sprites/GatoBarba.png","Practica_Final/Sprites/GatoPatata.png"};
        
        AgregarVarias(4,dibujosAtaque, (r) => new Carta_Ataque("Carta_Ataque", r));
        AgregarVarias(4,dibujosFavor, (r) => new Carta_Favor("Carta_Favor", r));
        AgregarVarias(4,dibujosBarajar, (r) => new Carta_Barajar("Carta_Barajar", r));
        AgregarVarias(4,dibujosSaltar, (r) => new Carta_Saltar("Carta_Saltar", r));
        AgregarVarias(4,dibujosFuturo, (r) => new Carta_Futuro("Carta_Futuro", r));
        AgregarVarias(4,dibujosNope, (r) => new Carta_Nope("Carta_Nope", r));
        string[] tiposGatos = { "GatoTaco", "GatoSandia", "GatoBarba", "GatoPatata" };
        foreach (var tipos in tiposGatos)
        {
            AgregarVarias(4,dibujosGato, (r) => new Carta_Gato(tipos, r));
        }
        int defuserSobrantes = 6 - numeroJugadores;
        
        AgregarVarias(defuserSobrantes,dibujosDefuser, (r) => new Carta_Defuser("Carta_Defuser", r));
        AgregarVarias(numeroJugadores-1,dibujosExplosion, (r) => new Carta_Explosion("Carta_Explosion", r));

        return barajaTemporal;

    }

    private void AgregarVarias(int cantidad,string[] poolDeRutas, Func<string,Carta> creador)
    {
        for (int i = 0; i < cantidad; i++)
        {
            string rutaAleatoria = poolDeRutas[rand.Next(poolDeRutas.Length)];
            barajaTemporal.Push(creador(rutaAleatoria));
        }
    }
}