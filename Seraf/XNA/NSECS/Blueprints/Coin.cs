using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Seraf.XNA.NSECS.Components;

namespace Seraf.XNA.NSECS.Blueprints
{
    [EntityBlueprint("Coin")]
    public class Coin : Entity
    {
        public Coin(int uuid, Vector2 pos, Vector2 size) : base(uuid, pos, size)
        {
            var tex = ContentPipeline.Instance.Load<Texture2D>("sprites/coin");

            var sprites = new Sprite[]
            {
                new Sprite(this, tex, new Rectangle(0, 0, 8, 8)),
                new Sprite(this, tex, new Rectangle(8, 0, 8, 8)),
                new Sprite(this, tex, new Rectangle(16, 0, 8, 8)),
                new Sprite(this, tex, new Rectangle(24, 0, 8, 8)),
                new Sprite(this, tex, new Rectangle(32, 0, 8, 8)),
                new Sprite(this, tex, new Rectangle(40, 0, 8, 8))
            };

            var anim = new SpriteAnim(this, sprites);
            this.AddComponent(anim);


            this.AddComponent(new Physics(this));
            this.AddComponent(new Collider(this));
        }
    }
}
