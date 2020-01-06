using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    /// <summary>
    /// A wrapper class for Tile data that is used upon constructing the TileLayers.
    /// </summary>
    public class TileData
    {
        [XmlAttribute("encoding")]
        public string encoding; // CSV is only supported right now.Maybe more support for others in the future?

        /// <summary>
        /// After serialization has set this string then the tiles are properly constructed.
        /// </summary>
        [XmlText]
        public string tiles_str
        {
            get;set;
        }

        /// <summary>
        /// 2D array of Tiles. Indexed by [y, x].
        /// </summary>
        [XmlIgnore]
        public TTile[,] tiles;

        public void ConstructTiles(int tileSize, int layerWidth, int layerHeight) // TODO: Somehow do this on construction?
        {
            var tiles = new TTile[layerHeight, layerWidth];

            List<int> ids = new List<int>();
            string[] split = tiles_str.Split(',');
            foreach(var s in split)
            {
                ids.Add(int.Parse(s));
            }

            int tileid;
            Rectangle tilerect;
            //int o = 0; // Iteration count
            int x = 0;
            int y = 0;

            for (int h = 0; h < layerHeight; ++h)
            {
                for (int w = 0; w < layerWidth; ++w)
                {
                    tileid = ids[0];
                    ids.RemoveAt(0);

                    x = 0 + tileSize * w;
                    y = 0 + tileSize * h;

                    if (tileid == 0)
                        tilerect = new Rectangle(x, y, 0, 0);
                    else
                        tilerect = new Rectangle(x, y, tileSize, tileSize);


                    tiles[h, w] = new TTile(tileid, tilerect);
                }
            }

            this.tiles = tiles;
        }
    }
}
