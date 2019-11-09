using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Seraf.Experimental.Boomerang
{
    public class BoomerangActor
    {
        public Texture2D texture;
        public Vector2 pos;
        Boomerang rang;

        bool rang_thrown;
        float rang_t;

        public BoomerangActor(Texture2D texture, Vector2 pos, Boomerang boomerang)
        {
            this.texture = texture;
            this.pos = pos;
            this.rang = boomerang;
        }

        public void ThrowRang()
        {
            rang.spd = Vector2.One * 4;
            rang.dir = new Vector2(1, -0.1f);
            //rang.dir = Vector2.Normalize(Vector2.Subtract(rang.pos, this.pos));
            angle = System.Math.Atan2(rang.dir.X, -rang.dir.Y);


            rang.rot = 0f;

            rang.pos = this.pos;


            rang_t = 0;
            rang_thrown = true;
        }

        float cos;
        double angle;

        public void Update(float delta)
        {
            if(rang_thrown)
            {
                rang_t += delta;

                //rang.pos += rang.spd * rang.dir;
                rang.rot += delta * 10;


                

                Vector2 a = Vector2.Normalize(Vector2.Subtract(this.pos, rang.pos));

 
                angle = System.Math.Atan2(rang.dir.X, -rang.dir.Y);

                cos = (float)Math.Cos(angle);

                rang.pos.X += cos; 


                if (rang_t > 1f)
                {
                    //rang.dir.Y = 10 * (float)Math.Sin(a.Y);

                    //rang.dir = Vector2.Normalize(Vector2.Subtract(this.pos, rang.pos));
                }

                Console.WriteLine(cos);
 
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, pos, Color.White);

            if (rang_thrown)
            {
                spriteBatch.Draw(rang.tex, rang.pos, null, rang.color, rang.rot, rang.orig, rang.scale, SpriteEffects.None, 0f);
            }
        }
    }
}
