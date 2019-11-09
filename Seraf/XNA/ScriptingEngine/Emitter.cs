using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSECS;

namespace Seraf.ScriptingEngine
{

    //[AttributeUsage(AttributeTargets.Method)]
    //public class EntityTemplate : Attribute
    //{
    //    public EntityTemplate(string type)
    //    {
    //        this.Type = type;
    //    }

    //    public string Type { get; }
    //}

    //public delegate Entity EntityTemplateDelegate();

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class EntityBlueprint : Attribute
    {
        public EntityBlueprint(string type)
        {
            this.Type = type;
        }

        public string Type { get; }
    }

    [EntityBlueprint("emitter")]
    public class Emitter : Entity
    {
        public Emitter(int uuid, Vector2 pos, Vector2 size) : base(uuid, pos, size)
        {
            this.body = new NSECS.Body(this, new NSECS.BoxCollider(this),
                new NSECS.SpriteAnim(
                    new NSECS.Sprite[]
                    {
                        new NSECS.Sprite(Game2.Game1.ContentPipeline.Load<Texture2D>("sprites/anim1"), new Rectangle(0,0, 64, 64)),
                        new NSECS.Sprite(Game2.Game1.ContentPipeline.Load<Texture2D>("sprites/anim1"), new Rectangle(64,0, 64, 64)),
                        new NSECS.Sprite(Game2.Game1.ContentPipeline.Load<Texture2D>("sprites/anim1"), new Rectangle(128,0, 64, 64)),
                        new NSECS.Sprite(Game2.Game1.ContentPipeline.Load<Texture2D>("sprites/anim1"), new Rectangle(192,0, 64, 64))
                    }));
        }
    }

    [EntityBlueprint("crate")]
    public class Crate : Entity
    {
        public Crate(int uuid, Vector2 pos, Vector2 size) : base(uuid, pos, size)
        {
        }
    }
}
