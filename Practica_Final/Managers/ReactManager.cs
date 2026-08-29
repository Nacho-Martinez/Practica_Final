using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.Interfaces;
using Practica_Final.Jugadores;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Practica_Final.Managers;

public class ReactManager
{
    public static ReactManager Instance = new ReactManager();
    public Carta JugadaPendiente { get; private set; }
    public bool EfectoCancelado { get; private set; } = false;


    public void ProcesarJugada(Jugador jugador,params Carta[] cartasJugadas)
        {
            if (cartasJugadas != null && cartasJugadas.Length > 0)
            {
                //Console.WriteLine("Procesar jugada Ia");
                Interfaz.Instancia.RellenarCartasJugadas(cartasJugadas);
                foreach (var carta in cartasJugadas)
                {
                    //Console.WriteLine($"Carta para eliminar: {carta.Nombre}");
                    jugador.Mano.Remove(carta);
                }
            }
            else
            {
               // Console.WriteLine("Procesar jugada Jugador");
                List<Carta> cartasHumano = new List<Carta>();
                foreach (var indice in  Interfaz.Instancia.IndicesSeleccionados)
                {
                    cartasHumano.Add(jugador.Mano[indice]);
                }
                Interfaz.Instancia.RellenarCartasJugadas(cartasHumano.ToArray());
             foreach (int i in Interfaz.Instancia.IndicesSeleccionados.OrderByDescending(x => x))
             {
                 jugador.Mano.RemoveAt(i);
             }
             Interfaz.Instancia.IndicesSeleccionados.Clear();
            }
           // Console.WriteLine($"Tamaño del Mazo : {Mazo<Carta>.Instancia.Baraja.Count}");
        }
    
    public void InvertirEfecto()
    {
        EfectoCancelado = !EfectoCancelado;
        //Console.WriteLine($"[LOG] ¡Nope jugado! Estado de cancelación: {EfectoCancelado}");
    }

    public void MeterJugadaEnCola(Carta carta)
    {
        JugadaPendiente = carta;
    }

    public void ResetearEfecto()
    {
        EfectoCancelado = false;
    }
    public void LimpiarJugadaPendiente()
    {
        JugadaPendiente = null;
    }
}