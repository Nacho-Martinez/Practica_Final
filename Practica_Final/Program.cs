using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;

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
            Console.WriteLine($"Carta : {carta.Nombre}");
        }
        Mazo.Barajar();
        Console.WriteLine("_____________________________");
        Console.WriteLine("BARAJADA");
        Console.WriteLine("_____________________________");
        foreach (var carta in Mazo.Baraja)
        {
            Console.WriteLine($"Carta : {carta.Nombre}");
        }
        Console.WriteLine("_____________________________");
        
    }
}