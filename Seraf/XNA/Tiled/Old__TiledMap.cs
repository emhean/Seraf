using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Tiled
{
    public enum TiledObjectType
    {
        Object,
        Tile,
        Image,
    }

    public enum TiledLayerType
    {
        ObjectLayer,
        TileLayer,
        ImageLayer
    }

    public class TiledMap
    {

    }

    public class TiledTile : TiledObject
    {
        public TiledTile(int id, string name, string type, Rectangle rect, TiledProperties properties) : base(id, name, type, rect, properties)
        {
        }
    }

    public class TiledImage : TiledObject
    {
        Texture2D image;

        public TiledImage(int id, string name, string type, Texture2D image, Rectangle rect, TiledProperties properties) : base(id, name, type, rect, properties)
        {
            this.image = image;
        }
    }

    public class TiledImageLayer : TiledObjectLayer<TiledImage>
    {
        public TiledImageLayer(int id, string name, List<TiledImage> tiledObjects) : base(id, name, tiledObjects)
        {
        }
    }

    public class TiledTileLayer : TiledObjectLayer<TiledTile>
    {
        public TiledTile[,] tiles;

        public TiledTileLayer(int id, string name, TiledTile[,] tiledObjects) : base(id, name, null)
        {
        }
    }

    public abstract class TiledObjectLayer<T> where T : TiledObject
    {
        int id;
        string name;
        readonly IEnumerable<T> tiledObjects;

        public TiledObjectLayer(int id, string name, List<T> tiledObjects)
        {
            this.id = id;
            this.name = name;
            this.tiledObjects = tiledObjects;
        }
    }

    public abstract class TiledObject
    {
        int id;
        string name;
        string type;

        /// <summary>
        /// Position x and y, size w and h.
        /// </summary>
        Rectangle rect;

        TiledProperties properties;

        public TiledObject(int id, string name, string type, Rectangle rect, TiledProperties properties)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.rect = rect;
            this.properties = properties;
        }
    }

    public class TiledProperties
    {
        Dictionary<string, string> properties;

        public TiledProperties()
        {
            this.properties = new Dictionary<string, string>();
        }

        public void AddProperty(string key, string value)
        {
            properties.Add(key, value);
        }

        public string GetPropertyValue(string key)
        {
            return properties[key];
        }
    }
}
