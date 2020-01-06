using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    [Serializable]
    [XmlType("tileset")]
    /// <summary>
    /// A Tiled tileset.
    /// </summary>
    public class TTileSet : ITiledProperties
    {
        // Note on how this class is serialized:
        // First the element is serialized in the map file. This element has only two
        //  attributes (firstgid and source). A *.tsx file is then loaded, where
        // the star (*.tsx) is the source attribute of this class.
        // That being said: This class contains the attributes for that source file.

        #region Fields
        public TTileSet()
        {
            //image = new TImage();
            tile_data = new List<TTileData>();
        }


        /// <summary>
        /// First grid id.
        /// </summary>
        [XmlAttribute("firstgid")]
        public int firstgid; // First grid id. id 1 == 0, so offset by 1.

        /// <summary>
        /// Tileset source file (*.tsx).
        /// </summary>
        [XmlAttribute("source")]
        public string source;

        [XmlAttribute("name")]
        public string name;

        /// <summary>
        /// Width of a tile.
        /// </summary>
        [XmlAttribute("tilewidth")]
        public int tileWidth;

        /// <summary>
        /// Height of a tile.
        /// </summary>
        [XmlAttribute("tileheight")]
        public int tileHeight;

        /// <summary>
        /// The n amount of tiles in this set.
        /// </summary>
        [XmlAttribute("tilecount")]
        public int tileCount;

        /// <summary>
        /// The n amount of columns in set.
        /// </summary>
        [XmlAttribute("columns")]
        public int columns;

        /// <summary>
        /// The image file element of this tileset.
        /// </summary>
        [XmlElement("image")]
        public TImage image;

        [XmlElement("tile")]
        public List<TTileData> tile_data;

        [XmlElement("properties")]
        public TProperties Properties { get; set; } = new TProperties();

        #endregion


        // TODO:  Move this method into the Load() method of tiled map 
        public void CreateClips() 
        {
            #region Tile clips
            List<Rectangle> tile_clips = new List<Rectangle>();

            int sq = this.columns; //(int)Math.Sqrt(tileCount);  // TO GET THE WIDTH AND SIZE CUZ ITS ALL SQUARED ANYWAYS
            // START WITH CANCEROUS FIRST LINE CUZ IT FUCKS THE INDEXES
            // DONT FUCKING TOUCH THIS

            for (int i = 0; i < sq; ++i)
            {
                tile_clips.Add(new Rectangle(i * tileWidth, 0, tileWidth, tileHeight));
            }

            // THE REST OF THE LINES CUZ IT WORKS
            for (int row = 1; row < sq; ++row)
            {
                for (int col = 0; col < sq; ++col)
                {
                    tile_clips.Add(new Rectangle(col * tileWidth, row * tileHeight, tileWidth, tileHeight));
                }
            }
            // ASSIGN THIS SCOPE LIST TO ARRAY
            for (int i = 0; i < tile_clips.Count; ++i)
            {
                // Assign clip
                this.tile_data[i].clip = tile_clips[i];
            }


            #endregion
        }


        public void SetImage(TImage image)
        {
            this.image = image;
        }

        /// <summary>
        /// Checks if a Tile's ID is a part of this tileset. 
        /// </summary>
        public bool IsTileIDPartOfSet(int tileid)
        {
            return (tileid >= (this.firstgid - 1) && (tileid < this.firstgid + this.tileCount));
        }


        public TTileData GetTileData(int tileid)
        {
            return this.tile_data[tileid];
        }

        public override string ToString()
        {
            return string.Concat(
                "name:", name,
                ", source", source,
                ", firstgid:", firstgid,
                ", tileWidth:", tileWidth,
                ", tileHeight:", tileHeight,
                ", tileCount:", tileCount,
                ", columns:", columns,
                ", tileData:", tile_data.Count);
        }
    }
}
