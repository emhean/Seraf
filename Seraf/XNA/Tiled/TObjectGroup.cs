using System.Collections.Generic;
using System.Text;

namespace Seraf.XNA.Tiled
{
    public class TObjectGroup : ITiledProperties
    {
        public int id;
        public string name;

        public List<TObject> objects;

        public TObjectGroup(int id, string name)
        {
            this.id = id;
            this.name = name;
            this.objects = new List<TObject>();
        }

        public TProperties Properties { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Concat("[Group] id:", id, ", name:", name, ", count:", objects.Count));
            foreach (var o in objects)
            {
                sb.AppendLine(string.Concat("in group: {\n", o.ToString(), "}"));
            }

            return sb.ToString();
        }
    }
}
