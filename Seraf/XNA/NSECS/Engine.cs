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
        UUID _uuid;
        public List<Entity> entities;
        public TTileMap map;

        /// <summary>
        /// The rendered scenes.
        /// </summary>
        public List<Scene> rend_scenes;

        public Engine(TTileMap map)
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

        public IEnumerable<TTile> GetRelevantTiles(TTileLayer layer, Vector2 pos, int distance)
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

                    yield return layer.tileData.tiles[y, x];
                }
            }
        }

        /// <summary>
        /// Update entity and its components.
        /// </summary>
        private void UpdateEntity(int uuid, float delta)
        {
            for (int j = 0; j < entities[uuid].GetComponentCount(); ++j)
            {
                if (entities[uuid][j].IsActive)
                    entities[uuid][j].Update(delta);

                if (entities[uuid][j].IsExpired) // Remove expired components
                    entities[uuid].RemoveComponent(entities[uuid][j]);
            }

            if (entities[uuid].Expired) // Remove entity if expired.
                this.RemoveEntity(entities[uuid].uuid);
        }

        private void ApplyGravityOnEntity(int uuid, Physics phys, float delta)
        {
            entities[uuid].pos.Y += phys.gravity;
            if (entities[uuid].HasComponent(out Collider foo))
                foo.Update(delta);
        }

        public void Update(float delta)
        {
            for (int i = 0; i < entities.Count; ++i)
                UpdateEntity(i, delta);

            for (int i = 0; i < entities.Count; ++i)
            {
                if (entities[i].HasComponent(out Physics phys))
                    ApplyGravityOnEntity(i, phys, delta); // Apply gravity on entity.

                if (entities[i].IsCollidble)
                {
                    #region Tile Collision
                    if (entities[i].HasComponent(out Collider collider))
                    {
                        foreach (var layer in map.tileLayers)
                        {
                            if (!layer.Properties.ContainsProperty("Collision"))
                                continue;

                            foreach (var tile in GetRelevantTiles(layer, entities[i].pos, 8))
                            {
                                if (tile.id == 0 || !tile.rect.Intersects(collider.rect))
                                    continue;

                                Rectangle intersection = Rectangle.Intersect(collider.rect, tile.rect);

                                COLLISION_SIDE side = collider.GetIntersectionSide(tile.rect);

                                //Console.WriteLine(side);

                                if (side == COLLISION_SIDE.Top)// && collider.rect.Top < intersection.Top
                                {
                                    entities[i].pos.Y -= intersection.Height;
                                    phys?.Land();

                                    collider.OnCollided(new CollisionArgs(side));
                                    collider.Update(delta);
                                    continue;
                                }
                                else if (side == COLLISION_SIDE.Bottom)// && collider.rect.Bottom > intersection.Top
                                {
                                    entities[i].pos.Y += (intersection.Height);

                                    if (phys?.jump < 0)
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
                                else if ((collider.rect.Bottom - phys.gravity) > tile.rect.Top && side == COLLISION_SIDE.Right)  //&& collider.rect.Bottom < tile.rect.Top
                                {
                                    entities[i].pos.X += intersection.Width;
                                    collider.OnCollided(new CollisionArgs(side));
                                    collider.Update(delta);
                                    continue;
                                }
                                #endregion
                            }
                        }
                    }


                    #region Other Colliders

                    // To avoid crash
                    if (collider == null)
                        continue;

                    for (int j = 0; j < entities.Count; ++j)
                    {
                        if (i == j)
                            continue;

                        if (entities[j].HasComponent(out Collider other))
                        {
                            Rectangle intersection = Rectangle.Intersect(collider.rect, other.rect);

                            COLLISION_SIDE side = collider.GetIntersectionSide(other);

                            if (side != COLLISION_SIDE.None)
                            {
                                other.OnCollided(new CollisionArgs(side));

                                if (entities[j].HasComponent(out Collectible collectible))
                                    if (entities[i].HasComponent(out Inventory inv))
                                        inv.AddToInventory(collectible);


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
                    }
                    #endregion
                }
            }
        }

        public void Render(Scene scene)
        {
            for (int i = 0; i < map.tileLayers.Count; ++i)
            {
                var relevant_tiles = GetRelevantTiles(map.tileLayers[i], scene.Camera.Position, 20);

                foreach (var tile in relevant_tiles)
                {
                    foreach (var ts in map.tileSets)
                    {
                        if (ts.IsTileIDPartOfSet(tile.id))
                        {
                            if (tile.id != 0)
                            {
                                // Here we subtract the firstgid (firstgid of that tileset)
                                //   of real tile id to get the index of the tileset (the tile id of that tileset)
                                int tset_index = (tile.id - ts.firstgid);
                                if (tset_index < 0)
                                    tset_index = 0;

                                foreach (var bounds in ts.tile_data[tset_index].bounds.objects)
                                {
                                    var clip = ts.tile_data[tset_index].clip;
                                    var bounds_clip = TUtilities.GetRectangle(bounds);
      
                                    bounds_clip.X += clip.X;
                                    bounds_clip.Y += clip.Y;

                                    scene.Render(ts.image.texture, tile.rect, bounds_clip, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

                                    //scene.Render(ts.image.texture, tile.rect, ts.tile_data[tset_index].clip, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                                    //if (tset_index == bounds.id) { }
                                }
                            }
                        }
                    }
                }
            }

            //for (int i = 0; i < map.tileLayers.Count; ++i)//for (int i = (map.tileLayers.Count - 1); i > -1; --i)
            //{
            //    foreach (var tile in GetRelevantTiles(map.tileLayers[i], scene.Camera.Position, 20))//foreach (var tile in map.tileLayers[i].tiles)
            //    {
            //        foreach (var ts in map.tileSets)
            //        {
            //            if (ts.IsTileIDPartOfSet(tile.id))
            //            {
            //                if (tile.id != 0)
            //                {
            //                    int id = (tile.id - ts.firstgid);
            //                    if (id < 0)
            //                        id = 0;

            //                    scene.Render(
            //                        ts.image.texture,
            //                        tile.rect,
            //                        ts.tile_data[id].clip[0],
            //                        (Color.White * map.tileLayers[i].Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);
            //                }
            //                //else spriteBatch.Draw(ts.image.texture, tile.rect, ts.tile_clips[tile.id], Color.White * map.tileLayers[i].Opacity);
            //            }
            //        }
            //    }
            //}

            for (int i = 0; i < entities.Count; ++i)
            {
                for (int j = 0; j < entities[i].GetComponentCount(); ++j)
                    if (entities[i].IsVisible)
                        entities[i][j].Render(scene);
            }
        }
    }
}
