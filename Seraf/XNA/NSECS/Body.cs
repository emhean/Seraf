using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NSECS
{
    public class Body
    {
        public Collider collider;
        public Sprite sprite;
        Entity entity;

        public Body(Entity entity, Collider collider, Sprite sprite)
        {
            this.entity = entity;
            this.collider = collider;
            this.sprite = sprite;
        }

        public void Update(float delta)
        {
            if(collider != null)
                collider.Update(delta);

            if(sprite != null)
                sprite.Update(delta);

        }

        public bool HasSprite()
        {
            return (sprite != null);
        }

        public bool HasCollider()
        {
            return (collider != null);
        }

        public void Render(Scene scene)
        {
            scene.Render(entity);

            //scene.Render(sprite.tex, entity.pos, sprite.clip, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
