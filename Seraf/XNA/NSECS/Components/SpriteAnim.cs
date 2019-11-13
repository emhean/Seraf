using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Seraf.XNA.NSECS.Components
{
    [ComponentBlueprint("anim")]
    public class SpriteAnim : Component
    {
        public Sprite[] Sprites { get; private set; }
        public int currentIndex;
        public int animSpeed;

        public float t; // Time
        public float _interval = 0.016f; // XNA approximation of 1 frame

        public SpriteEffects spriteEffects;

        public SpriteAnim(Entity entity, params Sprite[] sprites) : base(entity)
        {
            this.Sprites = sprites;
            this.currentIndex = 0;
            this.animSpeed = 10;
        }

        public override void Initialize(XmlElement e)
        {
            var tex = ContentPipeline.Instance.Load<Texture2D>(e.GetAttribute("texture"));
            List<Sprite> sprites = new List<Sprite>();

            foreach(XmlElement sprite in e.GetElementsByTagName("sprite"))
            {
                sprites.Add(new Sprite(this.Entity, tex, new Rectangle(
                    int.Parse(sprite.GetAttribute("x")),
                    int.Parse(sprite.GetAttribute("y")),
                    int.Parse(sprite.GetAttribute("width")),
                    int.Parse(sprite.GetAttribute("height")))));
            }

            this.Sprites = sprites.ToArray();
        }

        public void SetSprites(Sprite[] sprites)
        {
            if(Sprites != null)
            {
                if (!this.Sprites.Equals(sprites))
                {
                    this.Sprites = sprites;
                    this.currentIndex = 0;
                }
            }
            else
            {
                this.Sprites = sprites;
                this.currentIndex = 0;
            }
        }

        public override void Update(float delta)
        {
            t += delta;
            if (t > (_interval * animSpeed))
            {
                currentIndex += 1;
                if (currentIndex == Sprites.Length) // if out of bounds index
                    currentIndex = 0;

                t = 0; // Reset time.
            }

            base.Update(delta);
        }

        public override void Render(Scene scene)
        {
            //var drawPos = new Vector2((int)Entity.pos.X, (int)Entity.pos.Y);
            //var drawPos = Entity.pos;

            scene.Render(Sprites[currentIndex].tex, Entity.pos, Sprites[currentIndex].clip, Color.White, 0f, Vector2.Zero, Vector2.One, spriteEffects, 0f);
        }
    }
}
