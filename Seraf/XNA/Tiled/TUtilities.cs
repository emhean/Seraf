using Microsoft.Xna.Framework;

namespace Seraf.XNA.Tiled
{
    public static class TUtilities
    {
        public static Rectangle GetRectangle(TObject tobj)
        {
            return new Rectangle((int)tobj.pos.X, (int)tobj.pos.Y, (int)tobj.size.X, (int)tobj.size.Y);
        }
    }
}
