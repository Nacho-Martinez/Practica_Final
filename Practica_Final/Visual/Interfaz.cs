using System.Diagnostics;
using Practica_Final.BarajaCartas;
using Practica_Final.Cartas;
using Practica_Final.Estados;
using Practica_Final.Jugadores;
using Practica_Final.Managers;
using Practica_Final.Visual;
using SFML.Graphics;
using SFML.System;
using SFML.Window;



public class Interfaz
{
    public static Interfaz Instancia { get; private set; } = new();
    public RenderWindow Ventana { get; private set; }
    public Jugador JugadorActual { get;private set; }
    public CircleShape BotonJugar { get; private set; }
    public CircleShape BotonSaltarTurno { get; private set; }
    public uint VentanaAncho { get; } = 1200;
    public uint VentanaAlto { get; } = 800;
    public List<int> IndicesSeleccionados = new();
    public float Separacion { get; set; } = 120f;
    public Font Fuente { get; } = new Font(@"C:\Windows\Fonts\arial.ttf");
    public int IndiceEnemigo { get; set; } = 0;
    public Carta cartaBomba;
    public int IndiceInsercion { get; set; } = 0;
    private Carta[] cartasJugadas = new Carta[2];
    public List<Animaciones> AnimacionesActivas = new ();
    private Dictionary<Jugador, int> _slotsPorJugador = new();


    public void GenerarVentana()
    {
        BotonJugar = new CircleShape(78);
        BotonJugar.FillColor = Color.Transparent;
        BotonJugar.Position = new Vector2f(300, 230);
        
        BotonSaltarTurno = new CircleShape(78);
        BotonSaltarTurno.FillColor = Color.Transparent;
        BotonSaltarTurno.Position = new Vector2f(300, 400);
        Ventana.Closed += (_, _) => Ventana.Close();
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

    public void MostrarTurnoActual()
    {
        if (TurnManager.Instance.JugadorActual == null) return;
        Text textoTurno = new Text(Fuente, $"Turno de: {TurnManager.Instance.JugadorActual.Nombre}");
        textoTurno.CharacterSize = 24;
        textoTurno.FillColor = Color.Black; 
        textoTurno.Position = new Vector2f(850, 30); 

        Ventana.Draw(textoTurno);
    }

    public void ResterarIndiceInsercion()
    {
        IndiceInsercion = 0;
    }

    public void DibujarCartasJugadas()
    {
        if(cartasJugadas == null) return;
        if (cartasJugadas[0] != null)
        {
            RectangleShape carta1 = new RectangleShape(new Vector2f(100, 200));
            carta1.Position = new Vector2f(720, 300);
            carta1.OutlineColor = Color.Black;
            carta1.OutlineThickness = 1;
            carta1.FillColor = Color.White;
            carta1.Texture = SpritesManager.Instancia.ConseguirTextura(cartasJugadas[0].Dibujo);
            Ventana.Draw(carta1);
        }

        if (cartasJugadas[1] != null)
        {
            RectangleShape carta2 = new RectangleShape(new Vector2f(100, 200));
            carta2.Position = new Vector2f(820, 300);
            carta2.OutlineColor = Color.Black;
            carta2.OutlineThickness = 1;
            carta2.FillColor = Color.White;
            carta2.Texture = SpritesManager.Instancia.ConseguirTextura(cartasJugadas[1].Dibujo);
            Ventana.Draw(carta2);
        }
    }

    public void RellenarCartasJugadas(Carta[] cartas)
    {
        Array.Clear(cartasJugadas, 0, cartasJugadas.Length);
        for (int i = 0; i < cartas.Length; i++)
        {
            cartasJugadas[i] = cartas[i];
        }
    }

    public void DibujarFondo()
    {
        RectangleShape fondo = new RectangleShape(new Vector2f(1200,800));
        fondo.FillColor = Color.White;
        fondo.Texture = SpritesManager.Instancia.ConseguirTextura("Sprites\\FondoParaMesa.jpg");
        Ventana.Draw(fondo);
    }

    public void DibujarRivales()
    {
        foreach (var jugador in TurnManager.Instance.jugadoresVivos)
        {
            if (jugador is Jugador_Robot && _slotsPorJugador.TryGetValue(jugador, out int numeroRobot))
            {
                float escala = 0.6f;
                float ancho = 100f * escala;
                float alto = 150f * escala;
                float separacion = 25f;
                int cantidadCartas = jugador.Mano.Count;

                for (int i = 0; i < cantidadCartas; i++)
                {
                    RectangleShape cartaRival = new RectangleShape(new Vector2f(ancho, alto));
                    cartaRival.Origin = new Vector2f(ancho / 2, alto / 2);
                    cartaRival.Texture = SpritesManager.Instancia.ConseguirTextura("Sprites\\ReversoCarta.png");

                    if (numeroRobot == 0)
                    {
                        float inicioY = 400f - ((cantidadCartas - 1) * separacion) / 2f;
                        cartaRival.Position = new Vector2f(70f, inicioY + (i * separacion));
                        cartaRival.Rotation = 90f;
                    }
                    else if (numeroRobot == 1)
                    {
                        float inicioX = 600f - ((cantidadCartas - 1) * separacion) / 2f;
                        cartaRival.Position = new Vector2f(inicioX + (i * separacion), 80f);
                        cartaRival.Rotation = 180f;
                    }
                    else if (numeroRobot == 2)
                    {
                        float inicioY = 400f - ((cantidadCartas - 1) * separacion) / 2f;
                        cartaRival.Position = new Vector2f(1130f, inicioY + (i * separacion));
                        cartaRival.Rotation = -90f;
                    }
                    Instancia.Ventana.Draw(cartaRival);
                }
            }
        }

    }

    public void LanzarAnimacion(Texture textura, Vector2f inicion, Vector2f fin, float duracion,Action alTerminar = null)
    {
        AnimacionesActivas.Add(new Animaciones(textura,inicion,fin,duracion,alTerminar));
    }

    public Vector2f ObtenerPosicionRobot(Jugador robot)
    {
        if (!_slotsPorJugador.TryGetValue(robot, out int numeroRobot))
            return new Vector2f(600, 600);

        if (numeroRobot == 0) return new Vector2f(70f, 400f);
        if (numeroRobot == 1) return new Vector2f(600f, 80f);
        if (numeroRobot == 2) return new Vector2f(1130f, 400f);

        return new Vector2f(600, 600);
    }
    public void AsignarHuecoDeJugador(List<Jugador> todosLosJugadores)
    {
        int slot = 0;
        foreach (var jugador in todosLosJugadores)
        {
            if (jugador is Jugador_Robot)
            {
                _slotsPorJugador[jugador] = slot;
                slot++;
            }
        }
    }
}