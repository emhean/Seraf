using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Seraf.XNA.Tiled
{
    //<objectgroup draworder="index">
    //<object id="1" x="0" y="0" width="8" height="4"/>
    //</objectgroup>
    public struct TTileDataAnim
    {
        public int anim_ms;
        public int tile_id;

        public TTileDataAnim(int tile_id, int anim_ms)
        {
            this.tile_id = tile_id;
            this.anim_ms = anim_ms;
        }
    }

    public struct TTileData : ITiledProperties
    {
        public int id; // usually same as index

        public string type;
        public Rectangle[] bounds;
        public Rectangle[] clip;

        public TTileDataAnim[] tileDataAnims;

        public TTileData(int id, string type, Rectangle[] bounds, Rectangle[] clip, TTileDataAnim[] tileDataAnims, TProperties properties)
        {
            this.id = id;
            this.type = type;
            this.bounds = bounds;
            this.clip = clip;
            this.tileDataAnims = tileDataAnims;
            this.Properties = properties;
        }

        public TProperties Properties { get; }

        public override string ToString() => string.Concat("id:", id, ", bounds:", bounds);
    }
}
