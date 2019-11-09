using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;

namespace Seraf.XNA.Tiled
{
    /// <summary>
    /// A Tiled Object.
    /// </summary>
    public class TObject : ITiledProperties
    {
        public int id;
        public string name, type;
        public Vector2 pos, size;
        public TProperties Properties { get; }

        public TObject(int id, string name, string type, Vector2 pos, Vector2 size, TProperties properties)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.pos = pos;
            this.size = size;
            this.Properties = properties;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Concat("id: ", id, ", name:", name, ", type:", type, ". pos:", pos, ", size", size));

            sb.AppendLine("Properties:");
            foreach (var prop in Properties)
                sb.AppendLine(string.Concat("key: ", prop.Key, ", value:", prop.Value));

            return sb.ToString();
        }
    }
}
