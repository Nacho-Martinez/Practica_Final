using System.Diagnostics;
using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
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


    public void GenerarVentana()
    {
        EventManager.Instancia.EnCambioDeEstado += AsignarInput;
        BotonJugar = new RectangleShape(new Vector2f(200, 80));
        BotonJugar.FillColor = Color.Green;
        BotonJugar.Position = new Vector2f(80, 360);
        
        BotonSaltarTurno = new RectangleShape(new Vector2f(200, 80));
        BotonSaltarTurno.FillColor = Color.Red;
        BotonSaltarTurno.Position = new Vector2f(380, 360);
        
        Ventana.Closed += (_, _) => Ventana.Close();
        while (Ventana.IsOpen)
        {
           StateManager.Intancia.EstadoActual.Inputs();
           Ventana.DispatchEvents();
           Ventana.Clear(new Color(30,30,30));
           Ventana.Draw(BotonJugar);
           Ventana.Draw(BotonSaltarTurno);
           StateManager.Intancia.EstadoActual.Dibujar();
           JugadorActual = TurnManager.Instance.JugadorActual;
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

    public void AsignarInput()
    {
        StateManager.Intancia.EstadoActual.Inputs();
    }
}