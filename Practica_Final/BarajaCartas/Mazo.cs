using Practica_Final.Cartas;
using Practica_Final.Jugadores;
using Practica_Final.Managers;

namespace Practica_Final.BarajaCartas;

public class Mazo<T> where T : Carta
{
    public static Mazo<T> Instancia { get; private set; }= new Mazo<T>();
    public Stack<T> Baraja { get;  set; } = new();
    private Random rand = new();
    private Mazo()
    {
        EventManager.Instancia.EnSigueinteTurnoParaRobar += Robar;
        EventManager.Instancia.EnInsercionRealizada += InsertarCarta;
    }

    private void InsertarCarta(int indice)
    {
      List<T> barajaTemporal = new();
        
        foreach (var carta in Baraja)
        {
            barajaTemporal.Add(carta);
        }
        T bombaConvertida = (T)Interfaz.Instancia.cartaBomba;
        barajaTemporal.Insert(indice,bombaConvertida);
        Baraja.Clear();
        foreach (var c in barajaTemporal.AsEnumerable().Reverse())
        {
            Baraja.Push(c);
        }
    }

    private void Robar()
    {
        Carta cartaRobar = DarPrimeraCarta();
        if (cartaRobar is Carta_Explosion)
        {
            Interfaz.Instancia.cartaBomba = cartaRobar;
            EventManager.Instancia.GatoBoom();
            return;
        }
        TurnManager.Instance.JugadorActual.Mano.Add(cartaRobar);
        
    }

    public void Barajar()
    {
        List<T> barajaTemporal = new();
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
        foreach (var carta in barajaTemporal.AsEnumerable().Reverse())
        {
            Baraja.Push(carta);
        }
    }

    public  T DarPrimeraCarta() 
    {
        //Buen sitio para meter gestion de errores
        if (Baraja.Count == 0) return null;
        return Baraja.Pop();
    }

    public List<T> DevolverMazoTemporal()
    {
        List<T> listaTemp = new();
        foreach (var carta in Baraja)
        {
            listaTemp.Add(carta);
        }
        return listaTemp;
    }

}