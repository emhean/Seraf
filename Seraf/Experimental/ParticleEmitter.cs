using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Seraf.XNA;
using Seraf.XNA.NSECS;
using Seraf.XNA.NSECS.Components;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Seraf.Experimental
{
    public struct SpriteData
    {
        public Texture2D tex;
        public Rectangle clip;
        public Color color;
        public float rot;
        //public Vector2 pos;
        public Vector2 origin;
        public SpriteEffects spriteEffects;
        public float layerDepth;

        public SpriteData(Texture2D tex, Vector2 origin, Rectangle clip, Color color, float rot, SpriteEffects spriteEffects, float layerDepth)
        {
            this.tex = tex;
            //this.pos = pos;
            this.origin = origin;
            this.clip = clip;
            this.color = color;
            this.rot = rot;
            this.spriteEffects = spriteEffects;
            this.layerDepth = layerDepth;
        }

        //public SpriteData(Texture2D tex, Vector2 pos, Vector2 origin, Rectangle clip, float rot, SpriteEffects spriteEffects, float layerDepth)
        //{
        //    this.tex = tex;
        //    this.pos = pos;
        //    this.origin = origin;
        //    this.clip = clip;
        //    this.color = Color.White;
        //    this.rot = rot;
        //    this.spriteEffects = spriteEffects;
        //    this.layerDepth = layerDepth;
        //}
    }

    public interface IPoolable
    {
        bool Expired { get; set; }
    }

    public class Pool<T> where T : struct, IPoolable
    {
        private T[] pool;
        private Predicate<T> condition;

        public delegate T PoolDelegate(T obj);
        private PoolDelegate method { get; set; }

        public Pool(int size, PoolDelegate method, Predicate<T> condition)
        {
            this.pool = new T[size];
            this.condition = condition;
            this.method = method;
        }

        public void SetExpiredPredicate(Predicate<T> predicate)
        {
            this.condition = predicate;
        }


        public void Update()
        {
            for(int i = 0; i < pool.Length; ++i)
            {
                pool[i].Expired = condition.Invoke(pool[i]);
                if(!pool[i].Expired)
                {
                    pool[i] = method.Invoke(pool[i]);
                }
            }
        }

        /// <summary>
        /// Object from pool, available or not.
        /// </summary>
        public T this[int id] { get => pool[id]; set => pool[id] = value; }

        public IEnumerable GetEnumerable()
        {
            foreach (var o in pool)
                yield return o;
        }
        public IEnumerable GetEnumerableAvailable()
        {
            foreach (var o in pool)
                if(o.Expired)
                    yield return o;
        }
        public IEnumerable GetEnumerableActive()
        {
            foreach (var o in pool)
                if ( !o.Expired)
                    yield return o;
        }


        /// <summary>
        /// Gets total count of objects in pool.
        /// </summary>
        public int GetCount() => pool.Length;

        /// <summary>
        /// Gets total count of available objects in pool.
        /// </summary>
        public int GetAvailableCount()
        {
            int count = 0;
            for(int i = 0; i < pool.Length; ++i)
                if(pool[i].Expired)
                    count += 1;
            return count;
        }

        /// <summary>
        /// Gets an object from pool.
        /// </summary>
        public T Get(int id) => pool[id];

        /// <summary>
        /// Get next object in pool and next id of it. Returns -1 if none are available. 
        /// </summary>
        public void GetNext(out int id)
        {
            for(int i = 0; i < pool.Length; ++i)
            {
                if (pool[i].Expired)
                {
                    id = i;
                    return;
                }
            }

            id = -1;
        }

        /// <summary>
        /// Gets a copy of the next available object in pool. Returns false if none are available.
        /// </summary>
        public bool GetNext(out T obj)
        {
            foreach(var p in pool)
            {
                if(p.Expired)
                {
                    obj = p;
                    return true;
                }
            }

            obj = default(T);
            return false;
        }

        public void GetNext(out T obj, out int id)
        {
            for (int i = 0; i < pool.Length; ++i)
            {
                if (pool[i].Expired)
                {
                    id = i;
                    obj = pool[i];
                    return;
                }
            }

            throw new Exception("No object available!");
        }

        /// <summary>
        /// Get a copy of the next available object in pool. Throws Exception if none available.
        /// </summary>
        public T GetNext()
        {
            foreach (var p in pool)
            {
                if (p.Expired)
                {
                    return p;
                }
            }

            throw new Exception("No object in pool available!");
        }

        /// <summary>
        /// Set object in pool
        /// </summary>
        /// <param name="id">ID of pool object.</param>
        public void Set(T obj, int id)
        {
            pool[id] = obj;
        }

        /// <summary>
        /// Sets the next available object in pool.
        /// </summary>
        /// <param name="id">ID of pool object.</param>
        public void SetNext(T obj)
        {
            this.GetNext(out int id);
            pool[id] = obj;
        }

        /// <summary>
        /// Sets the next available object in pool.
        /// </summary>
        /// <param name="id">ID of pool object.</param>
        public void TrySetNext(T obj)
        {
            this.GetNext(out int id);
            if (id != -1)
            {
                pool[id] = obj;
            }
        }
    }

    public class ParticleEmitter : Component
    {
        public struct Particle : IPoolable
        {
            public Vector2 pos, spd, dir;
            public float lifeSpan;
            public float current_lifeSpan;
            public Vector2 GetVelocity() => (spd * dir);

            public bool Expired { get; set; } 

            public SpriteData spriteData;

            public Particle(Vector2 pos, Vector2 spd, Vector2 dir, float lifeSpan, SpriteData spriteData)
            {
                this.pos = pos;
                this.spd = spd;
                this.dir = dir;
                this.lifeSpan = lifeSpan;
                this.spriteData = spriteData;

                this.Expired = true;
                this.current_lifeSpan = 0f;
            }
        }

        Pool<Particle> particles;

        public ParticleEmitter(Entity entity) : base(entity)
        {
            int size = 10;
            this.particles = new Pool<Particle>(size, UpdateParticle, new Predicate<Particle>(x => (x.lifeSpan < x.current_lifeSpan)));

            for (int i = 0; i < size; ++i)
                this.particles.Set(CreateParticle(), i);
        }

        Particle UpdateParticle(Particle particle)
        {
            particle.pos += particle.GetVelocity();
            return particle;
        }

        Particle CreateParticle()
        {
            Random rnd = new Random();

            var dir = new Vector2(rnd.Next(-1, 2), rnd.Next(-1, 2));
            var tex = ContentPipeline.Instance.Load<Texture2D>("sprites/box");
            var spriteData = new SpriteData(tex, Vector2.Zero, new Rectangle(0, 0, tex.Width, tex.Height), Color.Red, 0f, SpriteEffects.None, 0f);

            return new Particle(this.Entity.pos, -dir, dir, 2f, spriteData);
        }

        public override void Update(float delta)
        {
            particles.Update();

            base.Update(delta);
        }

        public override void Render(Scene scene)
        {
            //for(int i = 0; i < particles.GetCount(); ++i)
            //{
            //    if(particles[i].Expired == false)
            //        scene.Render(particles[i].pos, particles[i].spriteData);
            //}

            foreach(Particle p in particles.GetEnumerableActive())
            {
                scene.Render(p.pos, p.spriteData);
            }

            base.Render(scene);
        }
    }
}
