using Practica_Final.Cartas;

namespace Practica_Final.BarajaCartas;

public class CreacionMazo
{
    public static CreacionMazo Instancia { get; private set; }= new ();
    private Stack<Carta> barajaTemporal = new();
    private Random rand = new Random();
    public string[] dibujosDefuser { get; private set; }= { "Sprites/Defuser1.png", "Sprites\\Defuser2.png"};

    public Stack<Carta> PrepararMazo(int numeroJugadores)
    {
        string[] dibujosAtaque = { "Sprites\\Ataque1.png", "Sprites\\Ataque2.png"};
        string[] dibujosFavor = { "Sprites\\Favor1.png", "Sprites\\Favor2.png"};
        string[] dibujosBarajar = { "Sprites\\Barajar1.png", "Sprites\\Barajar2.png"};
        string[] dibujosSaltar = { "Sprites\\Saltar1.png", "Sprites\\Saltar2.png"};
        string[] dibujosFuturo = { "Sprites\\Futuro1.png", "Sprites\\Futuro2.png"};
        string[] dibujosNope = { "Sprites\\Nope1.png", "Sprites\\Nope2.png"};
        string[] dibujosGato = { "Sprites\\GatoTaco.png" ,"Sprites\\GatoSandia.png", "Sprites\\GatoBarba.png","Sprites\\GatoPatata.png"};
        
        AgregarVarias(4,dibujosAtaque, (r) => new Carta_Ataque("Carta_Ataque", r));
        AgregarVarias(4,dibujosFavor, (r) => new Carta_Favor("Carta_Favor", r));
        AgregarVarias(4,dibujosBarajar, (r) => new Carta_Barajar("Carta_Barajar", r));
        AgregarVarias(4,dibujosSaltar, (r) => new Carta_Saltar("Carta_Saltar", r));
        AgregarVarias(5,dibujosFuturo, (r) => new Carta_Futuro("Carta_Futuro", r));
        AgregarVarias(5,dibujosNope, (r) => new Carta_Nope("Carta_Nope", r));
        Carta_Gato.TiposGato[] tiposGatos = { Carta_Gato.TiposGato.GatoTaco, Carta_Gato.TiposGato.GatoSandia,Carta_Gato.TiposGato.GatoBarba, Carta_Gato.TiposGato.GatoPatata};
        int i = 0;
        foreach (var tipos in tiposGatos)
        {
            AgregarVariasGatos(5,dibujosGato[i],tipos, (r,t) => new Carta_Gato(t.ToString(), r,t));
            i++;
        }
        int defuserSobrantes = 6 - numeroJugadores;
        
        AgregarVarias(defuserSobrantes,dibujosDefuser, (r) => new Carta_Defuser("Carta_Defuser", r));

        return barajaTemporal;

    }

    private void AgregarVarias(int cantidad, string poolDeRutas, Func<string, Carta_Gato> creador)
    {
        for (int i = 0; i < cantidad; i++)
        {
            barajaTemporal.Push(creador(poolDeRutas));
        }
    }

    private void AgregarVarias(int cantidad,string[] poolDeRutas, Func<string,Carta> creador)
    {
        for (int i = 0; i < cantidad; i++)
        {
            string rutaAleatoria = poolDeRutas[rand.Next(poolDeRutas.Length)];
            barajaTemporal.Push(creador(rutaAleatoria));
        }
    }
    
    private void AgregarVariasGatos(int cantidad, string ruta, Carta_Gato.TiposGato tipo, Func<string, Carta_Gato.TiposGato, Carta_Gato> creador)
    {
        for (int i = 0; i < cantidad; i++)
        {
            barajaTemporal.Push(creador(ruta, tipo));
        }
    }

    public void MeterBombas(int numeroJugadores)
    {
        
        string[] dibujosExplosion = { "Sprites\\Explosion.png"};
        for (int i = 0; i < numeroJugadores - 1; i++)
        {
            Carta_Explosion nuevaBomba = new Carta_Explosion("Carta_Explosion", dibujosExplosion[0]);
            Mazo<Carta>.Instancia.Baraja.Push(nuevaBomba);
        }
    }
    
}