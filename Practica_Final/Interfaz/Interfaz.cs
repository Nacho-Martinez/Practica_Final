using System.Diagnostics;
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
    private uint ventanaAncho = 1200;
    private uint ventanaAlto = 800;
    public List<int> IndicesSeleccionados = new();
    public float Separacion { get; private set; } = 120f;
    private Font fuente = new Font(@"C:\Windows\Fonts\arial.ttf");
    public int IndiceEnemigo { get; set; } = 0;


    public void GenerarVentana()
    {
        JugadorActual = TurnManager.Instance.ObtenerJugadorActual();
        BotonJugar = new RectangleShape(new Vector2f(200, 80));
        BotonJugar.FillColor = Color.Green;
        BotonJugar.Position = new Vector2f(80, 360);
        
        Ventana.Closed += (_, _) => Ventana.Close();
        
        InputManager.Instance.ClicksRaton();
        InputManager.Instance.FlechasTeclado();
        InputManager.Instance.TeclaEnter();
        while (Ventana.IsOpen)
        {
           Ventana.DispatchEvents();
           Ventana.Clear(new Color(30,30,30));
           Ventana.Draw(BotonJugar);

           float margenIzquierdo = 100f;
           float anchoDisponible = ventanaAncho - (margenIzquierdo * 2);
           float anchoCarta = 100f;
           if (JugadorActual.Mano.Count > 1)
           {
               float separacionNecesaria = anchoDisponible / (JugadorActual.Mano.Count - 1);
               Separacion = Math.Min(120f, separacionNecesaria);
           }

           for (int i = 0; i < JugadorActual.Mano.Count; i++)
           {
               Carta cartaActual = JugadorActual.Mano[i];
               
               RectangleShape rect = new RectangleShape(new Vector2f(anchoCarta, 150));
               float posX = margenIzquierdo + (i * Separacion);
               float posY = IndicesSeleccionados.Contains(i) ? 570f : 600f;
               rect.Position = new Vector2f(posX, posY);
               rect.Texture = SpritesManager.Instancia.ConseguirTextura(cartaActual.Dibujo);
               rect.FillColor = Color.White;

               
               Ventana.Draw(rect);
           }

           if (StateManager.Intancia.EstadoActual == StateManager.Estados.EsperandoAtaque)
           {
               EsperandoAtaque();
           }
           Ventana.Display();
        }
        
    }
    public void CrearVentana()
    {
        Ventana = new RenderWindow(new VideoMode(new Vector2u(ventanaAncho,ventanaAlto),32), "Exploding Kittens");
        Ventana.SetFramerateLimit(60); 
    }

    public void EsperandoAtaque()
    {
        if (StateManager.Intancia.EstadoActual != StateManager.Estados.EsperandoAtaque) return;
        RectangleShape panelFondo = new RectangleShape(new Vector2f(1000, 700));
        panelFondo.Position = new Vector2f(100, 50);
        panelFondo.FillColor = new Color(128, 128, 128, 150); //Gris SemiTransparente
        panelFondo.OutlineColor = Color.Black;
        panelFondo.OutlineThickness = 3;
        Ventana.Draw(panelFondo);
        Text titulo = new Text(fuente, ">> Elige a un Jugador<<");
        titulo.CharacterSize = 24;
        titulo.Position = new Vector2f(150, 100);
        Ventana.Draw(titulo);
        for(int i =0; i<TurnManager.Instance.jugadoresVivos.Count;i++)
        {
            RectangleShape nuevaCasilla = new RectangleShape(new Vector2f(300, 90));
            float posY = 200 + (i * 100);
            float posX = 100;
            nuevaCasilla.Position = new Vector2f(posX, posY);
            nuevaCasilla.FillColor = Color.Transparent;
            if (i == IndiceEnemigo)
            {
                nuevaCasilla.OutlineThickness = 3;
                nuevaCasilla.OutlineColor = Color.Yellow;
            }
            else
            {
                nuevaCasilla.OutlineThickness = 0;
                nuevaCasilla.OutlineColor = Color.Transparent;
            }
            Text nombreJugador = new Text(fuente, TurnManager.Instance.jugadoresVivos[i].Nombre);
            nombreJugador.Position = new Vector2f(posX+15, posY);
            Ventana.Draw(nuevaCasilla);
            Ventana.Draw(nombreJugador);
        }
    }

    public void ModificarIndiceEnemigo(int clave)
    {

        IndiceEnemigo = clave > 0
            ? (IndiceEnemigo - 1 +TurnManager.Instance.jugadoresVivos.Count ) % TurnManager.Instance.jugadoresVivos.Count
            : (IndiceEnemigo + 1) % TurnManager.Instance.jugadoresVivos.Count;
    }
}