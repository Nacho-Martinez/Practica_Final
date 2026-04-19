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
    private uint ventanaAncho = 1200;
    private uint ventanaAlto = 800;
    public List<int> IndicesSeleccionados = new();
    public float Separacion { get; private set; } = 120f;
    private Font fuente = new Font(@"C:\Windows\Fonts\arial.ttf");
    public int IndiceEnemigo { get; set; } = 0;
    private  Clock cronometroFuturo = new Clock();
    public Carta cartaBomba;
    public int IndiceInsercion { get; private set; } = 0;


    public void GenerarVentana()
    {
        
        BotonJugar = new RectangleShape(new Vector2f(200, 80));
        BotonJugar.FillColor = Color.Green;
        BotonJugar.Position = new Vector2f(80, 360);
        
        BotonSaltarTurno = new RectangleShape(new Vector2f(200, 80));
        BotonSaltarTurno.FillColor = Color.Red;
        BotonSaltarTurno.Position = new Vector2f(380, 360);
        
        Ventana.Closed += (_, _) => Ventana.Close();
        
        InputManager.Instance.ClicksRaton();
        InputManager.Instance.FlechasTeclado();
        InputManager.Instance.TeclaEnter();
        while (Ventana.IsOpen)
        {
           Ventana.DispatchEvents();
           Ventana.Clear(new Color(30,30,30));
           Ventana.Draw(BotonJugar);
           Ventana.Draw(BotonSaltarTurno);
           JugadorActual = TurnManager.Instance.JugadorActual;
           
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
               if (cartaActual.Resaltada)
               {
                   rect.OutlineColor = Color.Yellow;
                   rect.OutlineThickness = 2;
               }

               
               Ventana.Draw(rect);
           }
           VerFuturo();
           EsperandoAtaque();
            GatoBoom();
            InsertarBomba();
            MostrarTurnoActual();
            
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

    public void VerFuturo()
    {
        
        if (StateManager.Intancia.EstadoActual != StateManager.Estados.ViendoFuturo)
        {
            if (JugadorActual is not Jugador_Humano)
            {
                return;
            }
            cronometroFuturo.Restart();
            return;
        } 
        List<Carta> listaTemp = new();
        listaTemp = Mazo<Carta>.Instancia.DevolverMazoTemporal();
        RectangleShape panelFondo = new RectangleShape(new Vector2f(900, 700));
        panelFondo.Position = new Vector2f(150, 50);
        panelFondo.FillColor = new Color(128, 0, 128, 150); //Morado Semitransparente
        panelFondo.OutlineColor = Color.Black;
        panelFondo.OutlineThickness = 3;
        Ventana.Draw(panelFondo);

        for (int i = 0; i < 3; i++)
        {
            if (i >= listaTemp.Count) break;
            RectangleShape carta = new RectangleShape(new Vector2f(293.33f, 300));
            carta.Position = new Vector2f(i * 293.33f + 150 + (10), 200);
            carta.Texture = SpritesManager.Instancia.ConseguirTextura(listaTemp[i].Dibujo);
            Ventana.Draw(carta);

        }
        if (cronometroFuturo.ElapsedTime.AsSeconds() > 5f)
        {
            StateManager.Intancia.CambiarEstado(StateManager.Estados.Normal);
        }
        
    }

    public void GatoBoom()
    {
        if (StateManager.Intancia.EstadoActual != StateManager.Estados.DefusandoBomba) return;
        RectangleShape carta = new RectangleShape(new Vector2f(200,350));
        carta.Position = new Vector2f(400,50);
        carta.Texture = SpritesManager.Instancia.ConseguirTextura(cartaBomba.Dibujo);
        Ventana.Draw(carta);
        
    }

    public void InsertarBomba()
    {
        if (StateManager.Intancia.EstadoActual != StateManager.Estados.InsertandoBomba) return;
        RectangleShape panelFondo = new RectangleShape(new Vector2f(400, 300));
        panelFondo.Position = new Vector2f(400, 250);
        panelFondo.FillColor = Color.Black; 
        panelFondo.OutlineColor = Color.Black;
        panelFondo.OutlineThickness = 3;
        Ventana.Draw(panelFondo);
        
        ConvexShape flechaDerecha = new ConvexShape(3);
        flechaDerecha.SetPoint(0, new Vector2f(20, 0)); 
        flechaDerecha.SetPoint(1, new Vector2f(-20, -20)); 
        flechaDerecha.SetPoint(2, new Vector2f(-20, 20)); 
        flechaDerecha.FillColor = Color.White;
        flechaDerecha.Position = new Vector2f(625, 300);
        Ventana.Draw(flechaDerecha);
        ConvexShape flechaIzquierda = new ConvexShape(3);
        flechaIzquierda.SetPoint(0, new Vector2f(-20, 0)); 
        flechaIzquierda.SetPoint(1, new Vector2f(20, -20)); 
        flechaIzquierda.SetPoint(2, new Vector2f(20, 20)); 
        flechaIzquierda.FillColor = Color.White;
        flechaIzquierda.Position = new Vector2f(425, 300);
        Ventana.Draw(flechaIzquierda);
        int indiceVisual = Mazo<Carta>.Instancia.Baraja.Count - IndiceInsercion;
        Text numero = new Text(fuente, $"{indiceVisual}");
        numero.Position = new Vector2f(525, 275);
        Ventana.Draw(numero);
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
        Text textoTurno = new Text(fuente, $"Turno de: {JugadorActual.Nombre}");
        textoTurno.CharacterSize = 24;
        textoTurno.FillColor = Color.Yellow; 
        textoTurno.Position = new Vector2f(950, 30); 

        Ventana.Draw(textoTurno);
    }

    public void ResterarIndiceInsercion()
    {
        IndiceInsercion = 1;
    }
}