using System.Drawing;
using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.Estados;
using Practica_Final.InteligenciaArtificial;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using Practica_Final.Visual;
using Color = SFML.Graphics.Color;

namespace Practica_Final;

public class ElJuego
{
    private bool trampaBloqueada = false;
    public bool TurnoConfirmadoPendiente { get; private set; } = false;
    public int TurnoSinRobarPendiente { get; private set; } = 0;
    public bool HayTurnoSinRobarPendiente => TurnoSinRobarPendiente > 0;

    private void TrucosDebug()
    {
        
        if (!SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.K) && 
            !SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.L))
        {
            trampaBloqueada = false;
            return;
        }

        if (trampaBloqueada) return;

        
        if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.K))
        {
            for (int i = 0; i < TurnManager.Instance.jugadoresVivos.Count; i++)
            {
                if (TurnManager.Instance.jugadoresVivos[i] is Jugador_Robot)
                {
                    //Console.WriteLine($"[DEBUG] Destruyendo IA: {TurnManager.Instance.jugadoresVivos[i].Nombre}");
                    TurnManager.Instance.jugadoresVivos.RemoveAt(i);
                    MotorJuego.Intancia.RevisarFInPartida();
                    trampaBloqueada = true;
                    break; 
                }
            }
        }

        
        if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.L))
        {
            for (int i = 0; i < TurnManager.Instance.jugadoresVivos.Count; i++)
            {
                if (TurnManager.Instance.jugadoresVivos[i] is Jugador_Humano)
                {
                    //Console.WriteLine($"[DEBUG] Humano destruido.");
                    TurnManager.Instance.jugadoresVivos.RemoveAt(i);
                    MotorJuego.Intancia.RevisarFInPartida();
                    trampaBloqueada = true;
                    break; 
                }
            }
        }
    }
    public enum dificultades
    {
        Facil,
        Medio,
        Dificil
    };

    private Random rand = new Random();
    private string nombreJugador;
    public static ElJuego Instancia { get; private set; } = new ElJuego();
    private List<string> nombresIA =new List<string>(){ 
        "Miau-quina",
        "Cyber-Michi",
        "Garritas-IA",
        "Robo-Gato",
        "Láser-Cat",
        "Miau-Tron 3000",
        "Binary-Whiskers",
        "Micro-Michi",
        "Nano-Gato",
        "Súper-Sardino",
        "Bigotes-Wifi",
        "Chip-Gato",
        "Gato-Código",
        "Pixel-Michi",
        "Byte-Gato",
        "Electro-Ronroneo",
        "Sir Purr-a-Lot",
        "Data-Cat",
        "Kernel-Gato",
        "Mecha-Michi"
    };

    public bool haEmpezadoElJuego = false;

    public void CrearJugadores(dificultades eleccion,string nombreJugador)
    {
        Jugador_Humano jugador1 = new(nombreJugador);
        TurnManager.Instance.jugadoresVivos.Add(jugador1);
        List<Comportamiento> comportamientos = new List<Comportamiento>();
        switch (eleccion)
        {
            case dificultades.Facil:
                comportamientos.Add(new ComportamientoFacil());
                break;
            case dificultades.Medio:
                comportamientos.Add(new ComportamientoFacil());
                comportamientos.Add(new ComportamientoMedio());
                comportamientos.Add(new ComportamientoDificil());
                break;
            case dificultades.Dificil:
                comportamientos.Add(new ComportamientoDificil());
                break;
                    
              
        }
        for (int i = 0; i < 3; i++)
        {
            int indiceCerebro = rand.Next(0, comportamientos.Count);
            Comportamiento cerebroElegido = comportamientos[indiceCerebro];
            
            int numeroAleatorio = rand.Next(0, nombresIA.Count);
            string nombre = nombresIA[numeroAleatorio];
            nombresIA.RemoveAt(numeroAleatorio);
            
            Jugador_Robot nuevaIa = new(nombre,cerebroElegido);
            
            //Console.WriteLine($"[CONFIG] IA Creada: {nombre} | Dificultad: {cerebroElegido.GetType().Name}");
            TurnManager.Instance.jugadoresVivos.Add(nuevaIa);
        }
    }

    public void RepartirCartas()
    {
        foreach (var jugador in TurnManager.Instance.jugadoresVivos)
        {
            jugador.Mano = MotorJuego.Intancia.RepatirCartas(Mazo<Carta>.Instancia);
        }
    }

    public void Inicializar(dificultades dificultad)
    {
        // Interfaz.Instancia.CrearVentana();
        Mazo<Carta>.Instancia.Baraja = CreacionMazo.Instancia.PrepararMazo(4);
        Mazo<Carta>.Instancia.Barajar();
        CrearJugadores(dificultad,nombreJugador);
        RepartirCartas();
        
        Interfaz.Instancia.ResterarIndiceInsercion();
        CreacionMazo.Instancia.MeterBombas(4);
        Mazo<Carta>.Instancia.Barajar();
        Interfaz.Instancia.GenerarVentana();
        TurnManager.Instance.InicializarJugadorActual();
        Interfaz.Instancia.AsignarHuecoDeJugador(TurnManager.Instance.jugadoresVivos);
        BucleJuego();
    }
    public void BucleJuego()
    {
        while (Interfaz.Instancia.Ventana.IsOpen && haEmpezadoElJuego)
        {
            try
            {
                TrucosDebug();
                bool hayAnimaciones = Interfaz.Instancia.AnimacionesActivas.Count > 0;

                if (!hayAnimaciones)
                {
                    Jugador jugadorActual = TurnManager.Instance.JugadorActual;
                    if (jugadorActual is Jugador_Robot)
                    {
                        StateManager.Intancia.EstadoActual.ComportameintoIA();
                    }

                    if (StateManager.Intancia.EstadoActual is Estado_EsperandoTrasJugada ||
                        TurnManager.Instance.JugadorActual is Jugador_Humano)
                    {
                        StateManager.Intancia.EstadoActual.Inputs();
                    }
                }

                if (TurnoConfirmadoPendiente && Interfaz.Instancia.AnimacionesActivas.Count == 0 && StateManager.Intancia.EstadoActual is Estado_Normal)
                {
                    //Console.WriteLine("[DEBUG] Procesando turnoConfirmadoPendiente");
                    TurnoConfirmadoPendiente = false;
                    TurnoSinRobarPendiente = 0;
                    TurnManager.Instance.ConfirmarPasoDeTurno();
                }
                if (TurnoSinRobarPendiente > 0 && 
                    Interfaz.Instancia.AnimacionesActivas.Count == 0 &&
                    StateManager.Intancia.EstadoActual is Estado_Normal)
                {
                    TurnoSinRobarPendiente--;
                    if (TurnoSinRobarPendiente == 0)
                    {
                        int indice = TurnManager.Instance.jugadoresVivos.IndexOf(TurnManager.Instance.JugadorActual);
                        indice = (indice + 1) % TurnManager.Instance.jugadoresVivos.Count;
                        TurnManager.Instance.DarTurno(TurnManager.Instance.jugadoresVivos[indice]);
                        //Console.WriteLine("[DEBUG] Procesando turnoSinRobarPendiente");
                        EventManager.Instancia.SiguenteTurno();
                    }
                }


                Interfaz.Instancia.Ventana.DispatchEvents();
                Interfaz.Instancia.Ventana.Clear(new Color(30, 30, 30));
                Interfaz.Instancia.DibujarFondo();
                Interfaz.Instancia.Ventana.Draw(Interfaz.Instancia.BotonJugar);
                Interfaz.Instancia.DibujarCartasJugadas();
                Interfaz.Instancia.Ventana.Draw(Interfaz.Instancia.BotonSaltarTurno);
                StateManager.Intancia.EstadoActual.Dibujar();
                Interfaz.Instancia.MostrarTurnoActual();

                for (int i = Interfaz.Instancia.AnimacionesActivas.Count - 1; i >= 0; i--)
                {
                    Animaciones anim = Interfaz.Instancia.AnimacionesActivas[i];
                    Interfaz.Instancia.Ventana.Draw(anim.Carta);

                    if (anim.Actualizar())
                    {
                        try
                        {
                         anim.AlTerminar?.Invoke();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"[ERROR CALLBACK] {e.GetType().Name}: {e.Message}");
                            Console.WriteLine(e.StackTrace);
                        }
                        
                        Interfaz.Instancia.AnimacionesActivas.RemoveAt(i);
                    }
                }

                Interfaz.Instancia.Ventana.Display();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] {e.GetType().Name}: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }
    }

    public void BucleMenu()
    {
        while (Interfaz.Instancia.Ventana.IsOpen && !haEmpezadoElJuego) 
        {
            TurnManager.Instance.LimpiarListar();
            Mazo<Carta>.Instancia.LimpiarMazo();
            Interfaz.Instancia.Ventana.DispatchEvents();
            StateManager.Intancia.EstadoActual.Inputs();
            Interfaz.Instancia.Ventana.Clear(Color.Black);
            StateManager.Intancia.EstadoActual.Dibujar();
            Interfaz.Instancia.Ventana.Display();
            Interfaz.Instancia.Ventana.Clear();
        }
    }
    public void AsignarNombreJugador(string nombre)
    {
        nombreJugador = nombre;
    }
    public void MarcarTurnoPendiente()
    {
        TurnoConfirmadoPendiente = true;
    }
    public void MarcarTurnoSinRobarPendiente()
    {
        TurnoSinRobarPendiente = 2;
    }
    public void LimpiarFlagsPendientes()
    {
        TurnoConfirmadoPendiente = false;
        TurnoSinRobarPendiente = 0;
    }
}