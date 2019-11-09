using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Seraf.XNA.Tiled
{
    //A tileset usually looks like:
    //<tileset version="1.2" tiledversion="1.2.5" name="tes_overworld" tilewidth="8" tileheight="8" tilecount="16" columns="4">
    //<image source="tes_overworld.png" width="32" height="32"/>
    //<tile id = "0" type="Grass">
    // <objectgroup draworder = "index" >
    //  < object id="1" x="0" y="0" width="8" height="8"/>
    // </objectgroup>
    // <animation>
    //  <frame tileid = "0" duration="300"/>
    //  <frame tileid = "1" duration="300"/>
    //  <frame tileid = "2" duration="300"/>
    //  <frame tileid = "3" duration="300"/>
    // </animation>
    //</tile>
    //</tileset>
    public class TTileSet : ITiledProperties
    {
        public string name;

        public string source;
        public int firstgid; // First grid id. id 1 == 0, so offset by 1.

        public int tileWidth;
        public int tileHeight;
        public int tileCount;
        public int columns;

        public TImage image;

        /// <summary>
        /// The collisions of this tileset.
        /// </summary>
        public List<TTileData> tile_data;
        //public List<Rectangle> tile_clips;

        public TProperties Properties { get; }

        public TTileSet(string source, int firstgid, string name, int tileWidth, int tileHeight, int tileCount, int columns)
        {
            this.source = source;
            this.firstgid = firstgid;
            this.name = name;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tileCount = tileCount;
            this.columns = columns;
            this.tile_data = new List<TTileData>(); // THIS MIGHT NOT BE GOOD CUZ WE WILL ADD STUFF OUTSIDE THIS CONSTRUCTOR
        }

        public void CreateClips()
        {
            #region Tile clips
            List<Rectangle> tile_clips = new List<Rectangle>();

            int sq = (int)Math.Sqrt(tileCount);  // TO GET THE WIDTH AND SIZE CUZ ITS ALL SQUARED ANYWAYS
            // START WITH CANCEROUS FIRST LINE CUZ IT FUCKS THE INDEXES
            // DONT FUCKING TOUCH THIS
            for (int i = 0; i < sq; ++i)
            {
                tile_clips.Add(new Rectangle(tileWidth * i, 0, tileWidth, tileHeight));
            }
            // THE REST OF THE LINES CUZ IT WORKS
            for (int row = 1; row < sq; ++row)
            {
                for (int col = 0; col < sq; ++col)
                {
                    tile_clips.Add(new Rectangle(tileWidth * col, tileHeight * row, tileWidth, tileHeight));
                }
            }
            // ASSIGN THIS SCOPE LIST TO ARRAY
            for (int i = 0; i < tile_data.Count; ++i)
                tile_data[i].clip[0] = tile_clips[i];

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
            //if (tileid == 0 && this.firstgid == 1)
            //    return true;

            return (tileid >= (this.firstgid - 1) && (tileid < this.firstgid + this.tileCount));
        }


        //public TTileData GetTileData(int tileid)
        //{

        //}


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
