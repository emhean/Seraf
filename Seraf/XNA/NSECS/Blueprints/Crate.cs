using Microsoft.Xna.Framework;
using Seraf.XNA.NSECS;

namespace Seraf.XNA.ScriptingEngine
{
    [EntityBlueprint("crate")]
    public class Crate : Entity
    {
        public Crate(int uuid, Vector2 pos, Vector2 size) : base(uuid, pos, size)
        {
        }
    }
}
