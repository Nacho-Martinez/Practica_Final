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
        if (cartasPasivas.Count == 0 && cartasAgresivas.Count == 0)
        {
            Console.WriteLine("No hay cartas el las listas");
            return null;
        }
        int randomNum = rand.Next(0, 11);
        int randCarta;
        bool jugarPasiva = (randomNum <= 8);
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
                    Console.WriteLine("NO ha encontrado pareja para su gato");
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
        return rand.Next(1, 3);
    }
}