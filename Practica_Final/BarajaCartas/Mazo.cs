using Practica_Final.Cartas;

namespace Practica_Final.BarajaCartas;

public class Mazo<T>
{
    public Stack<T> Baraja { get;  set; } = new();
    private List<T> barajaTemporal = new();
    private Random rand = new();

    public void Barajar()
    {
        foreach (var carta in Baraja)
        {
            barajaTemporal.Add(carta);
        }
        int n = barajaTemporal.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            (barajaTemporal[k], barajaTemporal[n]) = (barajaTemporal[n], barajaTemporal[k]);
            //ESTA ES LA RECOMENDACION DE RIDER Y ME ESTABA DANDO TOC LAS BARRAS DEBAJO DEL CODIGO
            //ESTO ES LO QUE ME HA CONVERTIDO EN LO DE ARRIBA 
            // T value = barajaTemporal[k];
            // barajaTemporal[k] = barajaTemporal[n];
            // barajaTemporal[n] = value;
        }
        Baraja.Clear();
        foreach (var carta in barajaTemporal)
        {
            Baraja.Push(carta);
        }
    }

}