using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Seraf.XNA.NSECS.Components
{
    public class Sprite : Component
    {
        public Texture2D tex;
        public Rectangle clip;
        public Color color;
        public float rot;
        //public Vector2 pos; // Position is currently based of the entity position.
        public Vector2 origin;
        public SpriteEffects spriteEffects;
        public float layerDepth;

        public Sprite(Entity entity, Texture2D tex, Rectangle clip) : base(entity)
        {
            this.tex = tex;
            this.clip = clip;
            this.color = Color.White;
        }
        public Sprite(Entity entity, SpriteData spriteData) : base(entity)
        {
            this.tex = spriteData.tex;
            this.clip = spriteData.clip;
            this.color = spriteData.color;
            this.rot = spriteData.rot;
            this.origin = spriteData.origin;
            this.spriteEffects = spriteData.spriteEffects;
            this.layerDepth = spriteData.layerDepth;
        }

        public override void Update(float delta)
        {
        }

        public override void Render(Scene scene)
        {
            //scene.Render(tex, Entity.pos, clip, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            scene.Render(tex, Entity.pos, clip, color, rot, origin, Vector2.One, spriteEffects, layerDepth);
        }
    }
}
