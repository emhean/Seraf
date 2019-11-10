namespace Seraf.XNA.Tiled
{
    // A tile layer usually looks like:
    //<layer id = "3" name="Tile Layer 1" width="20" height="20">
    //<data encoding = "csv" >
    // tile data ..........................
    //</data>
    //</layer>

    public class TTileLayer : ITiledProperties
    {
        public int id;
        public string name;
        public string encoding; // CSV is only supported right now. Maybe more support for others in the future?
        public int width, height;

        float opacity; // Use property

        public TTile[,] tiles;

        public TProperties Properties { get; set; }

        public TTileLayer(int id, string name, string encoding, int width, int height)
        {
            this.id = id;
            this.name = name;
            this.encoding = encoding;
            this.width = width;
            this.height = height;
            this.opacity = 1.0f;
        }

        public TTileLayer(int id, string name, string encoding, int width, int height, float opacity)
        {
            this.id = id;
            this.name = name;
            this.encoding = encoding;
            this.width = width;
            this.height = height;
            this.Opacity = opacity;
        }

        public float Opacity
        {
            get => opacity;
            set
            {
                opacity = value;
                if (opacity < 0)
                    opacity = 0f;
                if (opacity > 1f)
                    opacity = 1f;
            }
        }

    }
}
