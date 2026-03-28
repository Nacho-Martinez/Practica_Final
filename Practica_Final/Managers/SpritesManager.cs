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
            cache[ruta] = new Texture(ruta);
        }

        return cache[ruta];
    }
}