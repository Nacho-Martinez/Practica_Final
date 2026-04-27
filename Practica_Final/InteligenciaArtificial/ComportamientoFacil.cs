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
        Dictionary<string, List<Carta>> gatosAgrupados = new Dictionary<string, List<Carta>>();
        foreach (var carta in robot.Mano)
        {
            switch (carta)
            {
                case Carta_Nope:
                    continue;
                case Carta_Ataque:
                case Carta_Favor:
                    cartasAgresivas.Add(carta);
                    break;
                case Carta_Gato:
                    if (!gatosAgrupados.ContainsKey(carta.Nombre))
                    {
                        gatosAgrupados[carta.Nombre] = new List<Carta>();
                    }
                    gatosAgrupados[carta.Nombre].Add(carta);
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

        foreach (var grupo in gatosAgrupados)
        {
            List<Carta> gatosDelMismoTipo = grupo.Value;
            int paresCompletos = gatosDelMismoTipo.Count / 2;
            int cartasQueFormanPar = paresCompletos * 2;
            for (int i = 0; i < cartasQueFormanPar; i++)
            {
                cartasAgresivas.Add(gatosDelMismoTipo[i]);
            }

            if (gatosDelMismoTipo.Count % 2 != 0)
            {
                cartasPasivas.Add(gatosDelMismoTipo.Last());
            }

        }
    }

    public override Carta[] CartasParaJugar()
    {
        int randomNum = rand.Next(0, 11);
        int randCarta;
        if (randomNum <= 8)
        {
            do
            {
             randCarta = rand.Next(0, cartasPasivas.Count);
                
            } while (cartasPasivas[randCarta] is Carta_Gato);
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