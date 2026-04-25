using System.Diagnostics;
using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.Estados;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;



public class Interfaz
{
    public static Interfaz Instancia { get; private set; } = new();
    public RenderWindow Ventana { get; private set; }
    public Jugador JugadorActual { get;private set; }
    public RectangleShape BotonJugar { get; private set; }
    public RectangleShape BotonSaltarTurno { get; private set; }
    public uint VentanaAncho { get; } = 1200;
    public uint VentanaAlto { get; } = 800;
    public List<int> IndicesSeleccionados = new();
    public float Separacion { get; set; } = 120f;
    public Font Fuente { get; } = new Font(@"C:\Windows\Fonts\arial.ttf");
    public int IndiceEnemigo { get; set; } = 0;
    public Carta cartaBomba;
    public int IndiceInsercion { get; private set; } = 0;
    private Carta[] cartasJugadas = new Carta[2];


    public void GenerarVentana()
    {
        BotonJugar = new RectangleShape(new Vector2f(200, 80));
        BotonJugar.FillColor = Color.Green;
        BotonJugar.Position = new Vector2f(80, 360);
        
        BotonSaltarTurno = new RectangleShape(new Vector2f(200, 80));
        BotonSaltarTurno.FillColor = Color.Red;
        BotonSaltarTurno.Position = new Vector2f(380, 360);
        Ventana.Closed += (_, _) => Ventana.Close();
        while (Ventana.IsOpen)
        {
           JugadorActual = TurnManager.Instance.JugadorActual;
           if (JugadorActual is Jugador_Robot)
           {
               StateManager.Intancia.EstadoActual.ComportameintoIA();
           }
           StateManager.Intancia.EstadoActual.Inputs();
           Ventana.DispatchEvents();
           Ventana.Clear(new Color(30,30,30));
           Ventana.Draw(BotonJugar);
            DibujarCartasJugadas();
           Ventana.Draw(BotonSaltarTurno);
           StateManager.Intancia.EstadoActual.Dibujar();
           MostrarTurnoActual();
           Ventana.Display();
           
        }
        
    }
    
    public void CrearVentana()
    {
        Ventana = new RenderWindow(new VideoMode(new Vector2u(VentanaAncho,VentanaAlto),32), "Exploding Kittens");
        Ventana.SetFramerateLimit(60); 
    }
    public void ModificarIndiceEnemigo(int clave)
    {

        IndiceEnemigo = clave > 0
            ? (IndiceEnemigo - 1 +TurnManager.Instance.jugadoresVivos.Count ) % TurnManager.Instance.jugadoresVivos.Count
            : (IndiceEnemigo + 1) % TurnManager.Instance.jugadoresVivos.Count;
    }
    
    public void ModificarIndiceInsercion(int clave)
    {

        IndiceInsercion = clave > 0
            ? (IndiceInsercion - 1 + Mazo<Carta>.Instancia.Baraja.Count) % Mazo<Carta>.Instancia.Baraja.Count
            : (IndiceInsercion + 1) % Mazo<Carta>.Instancia.Baraja.Count;
    }

    private void MostrarTurnoActual()
    {
        if (JugadorActual == null) return;
        Text textoTurno = new Text(Fuente, $"Turno de: {JugadorActual.Nombre}");
        textoTurno.CharacterSize = 24;
        textoTurno.FillColor = Color.Yellow; 
        textoTurno.Position = new Vector2f(950, 30); 

        Ventana.Draw(textoTurno);
    }

    public void ResterarIndiceInsercion()
    {
        IndiceInsercion = 1;
    }

    public void DibujarCartasJugadas()
    {
        if(cartasJugadas == null) return;
        
        RectangleShape carta1 = new RectangleShape(new Vector2f(100, 200));
        carta1.Position = new Vector2f(1000, 200);
        if (cartasJugadas[0] != null)
        {
            carta1.Texture = SpritesManager.Instancia.ConseguirTextura(cartasJugadas[0].Dibujo);
        }
        Ventana.Draw(carta1);
        RectangleShape carta2 = new RectangleShape(new Vector2f(100, 200));
        carta2.Position = new Vector2f(1100, 200);
        if (cartasJugadas[1] != null)
        {
            carta2.Texture = SpritesManager.Instancia.ConseguirTextura(cartasJugadas[1].Dibujo);
        }
        Ventana.Draw(carta2);
    }

    public void RellenarCartasJugadas(Carta[] cartas)
    {
        Array.Clear(cartasJugadas, 0, cartasJugadas.Length);
        for (int i = 0; i < cartas.Length; i++)
        {
            cartasJugadas[i] = cartas[i];
        }
    }
}