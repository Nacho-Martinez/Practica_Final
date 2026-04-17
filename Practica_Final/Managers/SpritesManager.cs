using SFML.Graphics;

namespace Practica_Final.Managers;

public class SpritesManager
{
    public static SpritesManager Instancia { get; private set; }= new();
    private Dictionary<string, Texture> cache = new ();

    public Texture ConseguirTextura(string ruta)
    {
        if (!cache.ContainsKey(ruta))
        {
            Texture nuevaTextura = new Texture(ruta);
            nuevaTextura.Smooth = true;
            cache[ruta] = nuevaTextura;
        }

        return cache[ruta];
    }
}