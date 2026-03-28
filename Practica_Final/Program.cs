using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.Jugadores;
using Practica_Final.Managers;

namespace Practica_Final;

class Program
{
    static void Main(string[] args)
    {
        Mazo<Carta> Mazo = new Mazo<Carta>();
        Mazo.Baraja = CreacionMazo.Instancia.PrepararMazo(4);
        Console.WriteLine("_____________________________");
        Console.WriteLine("SIN BARAJAR");
        Console.WriteLine("_____________________________");
        foreach (var carta in Mazo.Baraja)
        {
            Console.WriteLine($"Carta : {carta.Nombre} || Sprite: {carta.Dibujo}");
        }
        Mazo.Barajar();
        Console.WriteLine("_____________________________");
        Console.WriteLine("BARAJADA");
        Console.WriteLine("_____________________________");
        foreach (var carta in Mazo.Baraja)
        {
            Console.WriteLine($"Carta : {carta.Nombre} || Sprite: {carta.Dibujo}");
        }
        Console.WriteLine("_____________________________");
        Jugador_Humano Jugador_1 = new();
        Jugador_1.Mano = MotorJuego.Intancia.RepatirCartas(Mazo);
        foreach (var carta in Jugador_1.Mano)
        {
            Console.WriteLine($"Carta : {carta.Nombre} || Sprite: {carta.Dibujo}");
        }

    }
}