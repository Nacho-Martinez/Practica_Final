using SFML.Graphics;
using SFML.System;

namespace Practica_Final.Visual;

public class Animaciones
{
    public RectangleShape Carta { get; }
    private Vector2f posicionInicio;
    private Vector2f posicionFinal;
    private float Duracion;
    private Clock cronometro;
    public Action AlTerminar;

    public Animaciones(Texture texture, Vector2f inicio, Vector2f fin, float duracion,Action alTerminar = null)
    {
        Carta = new RectangleShape(new Vector2f(100, 150));
        Carta.Texture = texture;
        Carta.Position = inicio;

        posicionInicio = inicio;
        posicionFinal = fin;
        Duracion = duracion;
        AlTerminar = alTerminar;
        cronometro = new Clock();
    }

    public bool Actualizar()
    {
        float progreso = cronometro.ElapsedTime.AsSeconds() / Duracion;
        if (progreso >= 1)
        {
            Carta.Position = posicionFinal;
            return true;
        }

        float xActual = posicionInicio.X + (posicionFinal.X - posicionInicio.X) * progreso;
        float yActual = posicionInicio.Y + (posicionFinal.Y - posicionInicio.Y) * progreso;
        Carta.Position = new Vector2f(xActual, yActual);

        return false;
    }

}