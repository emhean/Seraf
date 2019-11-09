//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace Seraf.XNA.NSECS.Blueprints
//{
//    [EntityBlueprint("emitter")]
//    public class Emitter : Entity
//    {
//        public Emitter(int uuid, Vector2 pos, Vector2 size) : base(uuid, pos, size)
//        {
//            this.body = new Body(this, new BoxCollider(this),
//                new SpriteAnim(
//                    new Sprite[]
//                    {
//                        new Sprite(ContentPipeline.Instance.Load<Texture2D>("sprites/anim1"), new Rectangle(0,0, 64, 64)),
//                        new Sprite(ContentPipeline.Instance.Load<Texture2D>("sprites/anim1"), new Rectangle(64,0, 64, 64)),
//                        new Sprite(ContentPipeline.Instance.Load<Texture2D>("sprites/anim1"), new Rectangle(128,0, 64, 64)),
//                        new Sprite(ContentPipeline.Instance.Load<Texture2D>("sprites/anim1"), new Rectangle(192,0, 64, 64))
//                    }));
//        }
//    }
//}
