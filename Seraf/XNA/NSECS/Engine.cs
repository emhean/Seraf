using System;
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

        public Scene Scene { get; set; }

        public Engine(TiledMap map)
        {
            this.map = map;
            this.entities = new List<Entity>();
            this._uuid = new UUID();
        }


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


        IEnumerable<TTile> GetRelevantTiles(TTileLayer layer, Vector2 pos, int distance)
        {
            int xStart = ((int)pos.X / 8) - distance;
            int xEnd = ((int)pos.X / 8) + distance;
            int yStart = ((int)pos.Y / 8) - distance;
            int yEnd = ((int)pos.Y / 8) + distance;

            for (int x = xStart; x < xEnd; ++x)
            {
                for (int y = yStart; y < yEnd; ++y)
                {
                    if ((x >= map.mapWidth || x < 0) || (y >= map.mapHeight || y < 0))
                        continue;


                    yield return layer.tiles[y, x];
                }
            }
        }

        public void Update(float delta)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                for (int j = 0; j < entities[i].components.Count; ++j)
                {
                    if(entities[i].components[j].Enabled)
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
                Physics phys = entities[i].GetComponent<Physics>();


                #region Tile Collision
                if (phys != null)
                {
                    entities[i].pos.Y += phys.gravity;

                    if(collider != null)
                        collider.Update(delta);

                }
                if (collider != null)
                {
                    foreach (var layer in map.tileLayers)
                    {
                        if(!layer.Properties.ContainsProperty("Collision"))
                            continue;
                        
                        foreach (var tile in GetRelevantTiles(layer, entities[i].pos, 8))
                        {
                            if (tile.id == 0 || !tile.rect.Intersects(collider.rect))
                                continue;

                            Rectangle intersection = Rectangle.Intersect(collider.rect, tile.rect);

                            COLLISION_SIDE side= collider.GetIntersectionSide(tile.rect);

                            //Console.WriteLine(side);

                            if (side == COLLISION_SIDE.Top)// && collider.rect.Top < intersection.Top
                            {
                                entities[i].pos.Y -= intersection.Height;
                                phys.Land();

                                collider.OnCollided(new CollisionArgs(side));
                                collider.Update(delta);
                                continue;
                            }
                            else if (side == COLLISION_SIDE.Bottom)// && collider.rect.Bottom > intersection.Top
                            {
                                entities[i].pos.Y += (intersection.Height );

                                if (phys.jump < 0)
                                    phys.jump /= 2;


                                collider.OnCollided(new CollisionArgs(side));
                                collider.Update(delta);
                                continue;
                            }

                            if (side == COLLISION_SIDE.Left) //&& collider.rect.Y > tile.rect.Y
                            {
                                entities[i].pos.X -= intersection.Width;
                                collider.OnCollided(new CollisionArgs(side));
                                collider.Update(delta);
                                continue;
                            }
                            else if ( (collider.rect.Bottom - phys.gravity) > tile.rect.Top && side == COLLISION_SIDE.Right)  //&& collider.rect.Bottom < tile.rect.Top
                            {
                                entities[i].pos.X += intersection.Width;
                                collider.OnCollided(new CollisionArgs(side));
                                collider.Update(delta);
                                continue;
                            }

                            #region shiit
                            //if (collider.rect.Left > intersection.Right)
                            //{
                            //    entities[i].pos.X -= intersection.Width;
                            //    collider.OnCollided(new CollisionArgs(COLLISION_SIDE.Right));
                            //    collider.Update(delta);

                            //    continue;
                            //}
                            //else if (collider.rect.Right > intersection.Left)
                            //{
                            //    entities[i].pos.X += intersection.Width;

                            //    collider.OnCollided(new CollisionArgs(COLLISION_SIDE.Left));
                            //    collider.Update(delta);
                            //    continue;
                            //}



                            //var side = collider.GetIntersectionSide(tile.rect);
                            //Rectangle intersection = Rectangle.Intersect(collider.rect, tile.rect);
                            //Console.WriteLine(side);
                            //if (side == COLLISION_SIDE.Top)
                            //{
                            //    //entities[i].pos.Y = tile.rect.Y - collider.rect.Height;
                            //    //entities[i].pos.Y -= 1;

                            //    entities[i].pos.Y -= intersection.Height;
                            //    collider.Update(delta);
                            //    continue;
                            //}
                            //if (side == COLLISION_SIDE.Bottom)
                            //{
                            //    //entities[i].pos.Y = tile.rect.Y + tile.rect.Width;
                            //    entities[i].pos.Y += intersection.Height;
                            //    collider.Update(delta);
                            //    continue;
                            //}
                            //if (side == COLLISION_SIDE.Left)
                            //{
                            //    //entities[i].pos.X = tile.rect.X - collider.rect.Width;
                            //    entities[i].pos.X -= intersection.Width;
                            //    collider.Update(delta);
                            //    continue;
                            //}

                            //if (side == COLLISION_SIDE.Right)
                            //{
                            //    //entities[i].pos.X = tile.rect.X + tile.rect.Width;
                            //    entities[i].pos.X += intersection.Width;
                            //    collider.Update(delta);
                            //    continue;
                            //}
                            #endregion

                            #region shit
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
                            #endregion
                        }
                    }


                }
                #endregion



                #region Other Colliders

                if (collider == null)
                    continue;

                for(int j = 0; j < entities.Count; ++j)
                {
                    if (i == j)
                        continue;

                    Collider other = entities[j].GetComponent<Collider>();

                    if(other != null)
                    {
                        Rectangle intersection = Rectangle.Intersect(collider.rect, other.rect);

                        COLLISION_SIDE side = collider.GetIntersectionSide(other);

                        other.OnCollided(new CollisionArgs(side));

                        if (side == COLLISION_SIDE.Top)
                        {
                            entities[i].pos.Y -= intersection.Height;
                            collider.OnCollided(new CollisionArgs(side));
                            collider.Update(delta);
                            continue;
                        }
                        else if (side == COLLISION_SIDE.Bottom)
                        {
                            entities[i].pos.Y += (intersection.Height);
                            collider.OnCollided(new CollisionArgs(side));
                            collider.Update(delta);
                            continue;
                        }

                        if (side == COLLISION_SIDE.Left)
                        {
                            entities[i].pos.X -= intersection.Width;
                            collider.OnCollided(new CollisionArgs(side));
                            collider.Update(delta);
                            continue;
                        }
                        else if (side == COLLISION_SIDE.Right) 
                        {
                            entities[i].pos.X += intersection.Width;
                            collider.OnCollided(new CollisionArgs(side));
                            collider.Update(delta);
                            continue;
                        }
                    }
                }
                #endregion
            }


        }

        public void Render(Scene scene)
        {
            for (int i = 0; i < map.tileLayers.Count; ++i)//for (int i = (map.tileLayers.Count - 1); i > -1; --i)
            {
                foreach (var tile in GetRelevantTiles(map.tileLayers[i], scene.Camera.Position, 20))//foreach (var tile in map.tileLayers[i].tiles)
                {
                    foreach (var ts in map.tileSets)
                    {
                        if (ts.IsTileIDPartOfSet(tile.id))
                        {
                            if (tile.id != 0)
                            {
                                int id = (tile.id - ts.firstgid);
                                if (id < 0)
                                    id = 0;

                                scene.Render(
                                    ts.image.texture,
                                    tile.rect,
                                    ts.tile_data[id].clip[0],
                                    (Color.White * map.tileLayers[i].Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);
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
