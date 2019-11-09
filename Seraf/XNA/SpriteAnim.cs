using Microsoft.Xna.Framework;

namespace Seraf.XNA
{
    public class SpriteAnim : Sprite
    {
        public Sprite[] sprites;
        public int currentIndex;
        public int animSpeed;

        public float t; // Time
        public float _interval = 0.016f; // XNA approximation of 1 frame

        public SpriteAnim(params Sprite[] sprites) : base(null, Rectangle.Empty)
        {
            this.sprites = sprites;
            this.currentIndex = 0;
            this.animSpeed = 10;

            this.tex = sprites[currentIndex].tex;
            this.clip = sprites[currentIndex].clip;
        }

        public override void Update(float delta)
        {
            t += delta;
            if (t > (_interval * animSpeed))
            {
                currentIndex += 1;
                if (currentIndex == sprites.Length) // if out of bounds index
                    currentIndex = 0;

                // Set this class stuff to current sprite of array
                this.tex = sprites[currentIndex].tex;
                this.clip = sprites[currentIndex].clip;

                t = 0; // Reset time.
            }

            base.Update(delta);
        }
    }
}
