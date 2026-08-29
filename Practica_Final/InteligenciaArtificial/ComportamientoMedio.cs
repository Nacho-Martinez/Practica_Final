using Practica_Final.Cartas;
using Practica_Final.Jugadores;

namespace Practica_Final.InteligenciaArtificial;

public class ComportamientoMedio : Comportamiento
{
    private List<Carta> cartasAgresivas = new();
    private List<Carta> cartasPasivas = new();
    private Random rand = new Random();



    public override void RellenarListas(Jugador_Robot robot)
    {
        cartasAgresivas.Clear();
        cartasPasivas.Clear();
        foreach (var carta in robot.Mano)
        {
            switch (carta)
            {
                case Carta_Nope:
                    continue;
                case Carta_Ataque:
                case Carta_Favor:
                case Carta_Gato:
                    cartasAgresivas.Add(carta);
                    break;
                default:
                {
                    if (carta is not Carta_Defuser)
                    {
                        cartasPasivas.Add(carta);
                    }

                    break;
                }
            }
        }

        List<Carta> cartasGato = new();
        int counter = 0;
        foreach (var cartaGato in cartasAgresivas)
        {
            if (cartaGato is Carta_Gato)
            {
                counter++;
                cartasGato.Add(cartaGato);
            }
        }

        if (counter % 2 != 0)
        {
            cartasPasivas.Add(cartasAgresivas[0]);
            cartasAgresivas.RemoveAt(0);
        }
    }

    public override Carta[] CartasParaJugar()
    {
        if (cartasPasivas.Count == 0 && cartasAgresivas.Count == 0)
        {
            //Console.WriteLine("No hay cartas el las listas");
            return null;
        }
        int randomNum = rand.Next(0, 11);
        int randCarta;
        bool jugarPasiva = (randomNum <= 6);
        if (jugarPasiva && cartasPasivas.Count == 0) jugarPasiva = false;
        if (!jugarPasiva && cartasAgresivas.Count == 0) jugarPasiva = true;
        if (jugarPasiva)
        {
            List<Carta> cartasPasivasValidas = cartasPasivas
                .Where(c => c is not Carta_Gato)
                .ToList();
    
            if (cartasPasivasValidas.Count == 0)
            {
                if (cartasAgresivas.Count == 0) return null;
                randCarta = rand.Next(0, cartasAgresivas.Count);
                return [cartasAgresivas[randCarta]];
            }
    
            randCarta = rand.Next(0, cartasPasivasValidas.Count);
            return [cartasPasivasValidas[randCarta]];
        }
        else
        {
            randomNum = rand.Next(0, cartasAgresivas.Count);
            if (cartasAgresivas[randomNum] is Carta_Gato)
            {
                Carta segundaCartaGato = null;
                foreach (var carta in cartasAgresivas)
                {
                    if (carta is Carta_Gato)
                    {
                        segundaCartaGato = carta;
                        break;
                    }
                }

                if (segundaCartaGato != null)
                {
                    return [cartasAgresivas[randomNum], segundaCartaGato];
                }
                else
                {
                    //Console.WriteLine("NO ha encontrado pareja para su gato");
                    return null;
                }
            }
            else
            {
                return [cartasAgresivas[randomNum]];
            }
            
        }
    }

    public override int NumeroDeJugadas()
    {
        return rand.Next(1, 4);
    }
}