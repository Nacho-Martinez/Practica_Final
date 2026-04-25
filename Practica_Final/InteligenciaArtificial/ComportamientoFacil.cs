using System.Diagnostics;
using Practica_Final.Cartas;
using Practica_Final.Jugadores;

namespace Practica_Final.InteligenciaArtificial;

public class ComportamientoFacil : Comportamiento
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
        int randomNum = rand.Next(0, 11);
        if (randomNum <= 8)
        {
            int randCarta = rand.Next(0, cartasPasivas.Count);

            return [cartasPasivas[randCarta]];
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
                    return null;
                }

                return [cartasAgresivas[randomNum], segundaCartaGato];
            }
            else
            {
                return [cartasAgresivas[randomNum]];
            }
            
        }
    }

    public override int NumeroDeJugadas()
    {
        return rand.Next(0, 2);
    }
}