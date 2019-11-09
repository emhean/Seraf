using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Seraf.XNA.Collision;
using Seraf.XNA.NSECS.Components;
using Seraf.XNA.Tiled;

namespace Seraf.XNA.NSECS
{
    public class Engine
    {
        public List<Entity> entities;
        public TiledMap map;

        UUID _uuid;

        public Engine(TiledMap map)
        {
            this.map = map;

            this.entities = new List<Entity>();
            this._uuid = new UUID();
        }

        public Scene Scene { get; set; }


        public Entity GetEntity(int uuid)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                if (entities[i].uuid == uuid)
                    return entities[i];
            }
            throw new System.Exception("Entity not found!");
        }

        public void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
            _uuid.GetUUID(entity.uuid);

            System.Console.WriteLine("Added Entity of UUID: " + entity.uuid);
        }

        public void RemoveEntity(int uuid)
        {
            for (int i = 0; i < entities.Count; ++i)
                if (entities[i].uuid == uuid)
                {
                    entities.RemoveAt(i);
                    _uuid.FreeUUID(uuid);

                    System.Console.WriteLine("Removed Entity of UUID: " + uuid);
                }
        }

        public void Update(float delta)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                for (int j = 0; j < entities[i].components.Count; ++j)
                {
                    entities[i].components[j].Update(delta);

                    if (entities[i].components[j].Expired) // Remove expired components
                    {
                        entities[i].RemoveComponent(entities[i].components[j]);
                    }
                }

                if (entities[i].Expired) // Remove entity if expired.
                    this.RemoveEntity(entities[i].uuid);
            }


            for (int i = 0; i < entities.Count; ++i)
            {
                Collider collider = entities[i].GetComponent<Collider>();

                if (collider != null)
                {
                    foreach (var layer in map.tileLayers)
                    {
                        foreach (var tile in layer.tiles)
                        {
                            if (tile.id == 0 ||!tile.rect.Intersects(collider.rect))
                                continue;

                            var side = collider.GetIntersectionSide(tile.rect);

                            //var phys = entities[i].GetComponent<Physics>();
                            Rectangle intersection = Rectangle.Intersect(collider.rect, tile.rect);

                            if (side == COLLISION_SIDE.Top)
                            {
                                entities[i].pos.Y = tile.rect.Y - collider.rect.Height;
                                //entities[i].pos.Y -= intersection.Height;
                                collider.Update(delta);
                                continue;
                            }
                            if (side == COLLISION_SIDE.Bottom)
                            {
                                entities[i].pos.Y = tile.rect.Y + tile.rect.Width;
                                //entities[i].pos.Y += intersection.Height;
                                collider.Update(delta);
                                continue;
                            }
                            if (side == COLLISION_SIDE.Left)
                            {
                                entities[i].pos.X = tile.rect.X - collider.rect.Width;
                                //entities[i].pos.X -= intersection.Width;
                                collider.Update(delta);
                                continue;
                            }
                            if (side == COLLISION_SIDE.Right)
                            {
                                entities[i].pos.X = tile.rect.X + tile.rect.Width;
                                //entities[i].pos.X += intersection.Width;
                                collider.Update(delta);
                                continue;
                            }

                            //if (side.HasFlag(CollisionDetection.COLLISION_SIDE.Top))
                            //{
                            //    //entities[i].pos.Y = tile.rect.Y - collider.rect.Height;
                            //    entities[i].pos.Y -= intersection.Height;
                            //    break;
                            //}
                            //else if (side.HasFlag(CollisionDetection.COLLISION_SIDE.Bottom))
                            //{
                            //    //entities[i].pos.Y = tile.rect.Y + tile.rect.Width;
                            //    entities[i].pos.Y += intersection.Height;
                            //    break;
                            //}

                            //if (side.HasFlag(CollisionDetection.COLLISION_SIDE.Left))
                            //{
                            //    //entities[i].pos.X = tile.rect.X - collider.rect.Width;
                            //    entities[i].pos.X -= intersection.Width;
                            //    break;
                            //}
                            //else if (side.HasFlag(CollisionDetection.COLLISION_SIDE.Right))
                            //{
                            //    //entities[i].pos.X = tile.rect.X + tile.rect.Width;
                            //    entities[i].pos.X += intersection.Width;
                            //    break;
                            //}
                        }
                    }
                }
                //for (int x = 0; x < entities.Count; ++x)
                //{
                //    if (i != x)
                //    {

                //    }
                //}
            }
        }

        public void Render(Scene scene)
        {
            for (int i = 0; i < map.tileLayers.Count; ++i)//for (int i = (map.tileLayers.Count - 1); i > -1; --i)
            {
                foreach (var tile in map.tileLayers[i].tiles)
                {
                    foreach (var ts in map.tileSets)
                    {
                        if (ts.IsTileIDPartOfSet(tile.id))
                        {
                            if (tile.id != 0)
                            {
                                //            SpriteBatch.Draw(tex, pos, clip, color, rot, orig, effects, layerDepth);
                                scene.Render(ts.image.texture, tile.rect, ts.tile_data[tile.id - ts.firstgid].clip[0], (Color.White * map.tileLayers[i].Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);
                            }
                            //else spriteBatch.Draw(ts.image.texture, tile.rect, ts.tile_clips[tile.id], Color.White * map.tileLayers[i].Opacity);
                        }
                    }
                }
            }


            for (int i = 0; i < entities.Count; ++i)
            {
                foreach (var c in entities[i].components)
                {
                    c.Render(scene);
                }
            }
        }
    }
}
