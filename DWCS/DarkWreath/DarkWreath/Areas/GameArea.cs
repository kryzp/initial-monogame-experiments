using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using DarkWreath.Actors;
using DarkWreath.Math;
using TiledCS;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace DarkWreath.Areas
{
    public class GameArea
    {
        protected List<Actor> actors = new List<Actor>();
        public List<Actor> Actors => actors;

        public TiledMap Map { get; protected set; }
        public TiledLayer CollisionLayer { get; private set; }
        public List<(TiledTile, Vector2)> CollidableTiles { get; private set; }
        
        public GameArea()
        {
            CollidableTiles = new List<(TiledTile, Vector2)>();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Actor actor in Actors)
            {
                actor.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch b)
        {
            foreach (Actor actor in Actors)
            {
                actor.Draw(gameTime, b);
            }
        }

        public virtual void DrawUI(GameTime gameTime, SpriteBatch b)
        {
            foreach (Actor actor in Actors)
            {
                actor.DrawUI(gameTime, b);
            }
        }
        
        public void AddActor(Actor a)
        {
            a.CurrentArea = this;
            actors.Add(a);
        }

        public void RemoveActor(Actor a)
        {
            actors.Remove(a);
        }

        public void LoadMap(string map)
        {
            Map = new TiledMap("Content/" + map + ".tmx");
            Game1.MapRenderer.Load(Map);
            CollisionLayer = Map.Layers.First(x => x.name == "Collision");

            CollidableTiles.Clear();

            for (int y = 0; y < CollisionLayer.height; y++)
            {
                for (int x = 0; x < CollisionLayer.width; x++)
                {
                    var index = x + (y * CollisionLayer.width);
                    var gid = CollisionLayer.data[index];

                    if (gid == 0)
                        continue;
                    
                    var tileX = x;
                    var tileY = y;
                    
                    var mapTileset = Map.GetTiledMapTileset(gid);
                    var tileset = Game1.MapRenderer.Tilesets[mapTileset.firstgid];

                    var tile = Map.GetTiledTile(mapTileset, tileset, gid);

                    if (tile.properties.Length > 0)
                    {
                        Rectangle tileRect = new Rectangle(tileX, tileY, 1, 1);
                        CollidableTiles.Add((tile, new Vector2(tileX, tileY)));
                    }
                }
            }
        }

        public void OverrideMap(string map, Rectangle source, int destX, int destY)
        {
            /*
            OverrideMap(
                Game1.TiledContent.Load<TiledMap>(map),
                source,
                destX, destY
            );
            */
        }

        public void OverrideMap(TiledMap overrideMap, Rectangle source, int destX, int destY)
        {
            /*
            foreach (var layer0 in overrideMap.TileLayers)
            {
                for (ushort y = 0; y < MathHelper.Min(source.Height, layer0.Height); y++)
                {
                    for (ushort x = 0; x < MathHelper.Min(source.Width, layer0.Width); x++)
                    {
                        var layer1 = Map.GetLayer<TiledMapTileLayer>(layer0.Name);
                        var tile = layer0.GetTile(x, y);

                        layer1.SetTile((ushort)(destX+x), (ushort)(destY+y), (uint)tile.GlobalIdentifier);
                    }
                }
            }

            Game1.MapRenderer.Load(Map);
            */
        }

        public virtual List<Hit> GetCollidingColliders(Collider collider)
        {
            List<Hit> result = new List<Hit>();

            foreach (Actor act in actors)
            {
                if (act.Collider != null && act.Collider != collider)
                {
                    if (act.Collider.Overlaps(collider, out Vector2 push))
                    {
                        result.Add(new Hit()
                        {
                            Other = act.Collider,
                            Pushout = push
                        });
                    }
                }
            }

            return result;
        }
        
        public virtual bool ResolveCollidingPosition(Collider collidingPosition)
        {
            RectangleF colliderRect = collidingPosition.WorldBounds;
            {
                colliderRect.X /= 16f;
                colliderRect.Y /= 16f;
                colliderRect.Width /= 16f;
                colliderRect.Height /= 16f;
            }
            
            List<(TiledTile, Vector2)> collidingTiles = new List<(TiledTile, Vector2)>();

            foreach (var tile in CollidableTiles)
			{
                RectangleF tileRect = new RectangleF(tile.Item2.X, tile.Item2.Y, 1f, 1f);

                if (colliderRect.Intersects(tileRect))
                    collidingTiles.Add(tile);
			}

            if (collidingTiles.Count > 0)
            {
                /*
                foreach (var hit in GetHitsFromColliderAndTiles(collidingPosition, collidingTiles))
                {
                    if (hit.Item2 == "StairLeft" || hit.Item2 == "StairRight")
                    {
                        character.Transform.MoveY(MathF.Abs(amount.X) * ((amount.X < 0) ? 0.707f : -0.707f));
                        break;
                    }
                }
                */

                var sortedhits =
                    GetHitsFromColliderAndTiles(collidingPosition, collidingTiles)
                    .OrderBy(x => x.Item1.GetDistanceFromOtherSquared(collidingPosition, true))
                    .ToList();
                
                sortedhits.RemoveAll(x => !x.Item1.Solid);
                ResolveCollisionHits(collidingPosition, sortedhits);
            }

            return false;
        }

        private void ResolveCollisionHits(Collider collidingPosition, List<(Hit, string)> hits)
        {
            foreach (var hit in hits)
            {
                collidingPosition.Actor.Transform.Position += hit.Item1.Pushout;
                collidingPosition.Actor.OnCollisionHit(hit.Item1);

                // deflect projectile off of wall
                //if (collidingPosition.Actor is Projectile proj)
                //{
                //    proj.Velocity = Util.ReflectVector(proj.Velocity, hit.Item1.Pushout.Normalized());
                //}

                bool exit = false;
                foreach (var hit2 in hits)
                {
                    if (!collidingPosition.Overlaps(hit2.Item1.Other, out _))
                        exit = true;
                    else
                    {
                        exit = false;
                        break;
                    }
                }

                if (exit)
                    break;
            }
        }

        private bool IsColliderTypeSolid(string type)
        {
            return type == "S" || type == "TL" || type == "TR" || type == "BL" || type == "BR";
        }

        private List<(Hit, string)> GetHitsFromColliderAndTiles(Collider collider, List<(TiledTile, Vector2)> collidingTiles)
        {
            List<(Hit, string)> result = new List<(Hit, string)>();

            foreach (var tilePair in collidingTiles)
            {
                var tile = tilePair.Item1;
                string type = tile.properties.First(x => x.name == "ColliderType").value;

                Hit hit = new Hit();
                {
                    Collider tileCollider = new Collider();

                    float w = Map.TileWidth;
                    float h = Map.TileHeight;
                    
                    switch (type)
                    {
                        // solid
                        case "S":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(0, 0),
                                    new Vector2(w, 0),
                                    new Vector2(w, h),
                                    new Vector2(0, h)
                                });
                            break;
                        
                        // top left triangle
                        case "TL":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(w, 0),
                                    new Vector2(0, h),
                                    new Vector2(w, h)
                                });
                            break;
                        
                        // top right triangle
                        case "TR":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(0, 0),
                                    new Vector2(0, h),
                                    new Vector2(w, h)
                                });
                            break;
                        
                        // bottom left triangle
                        case "BL":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(0, 0),
                                    new Vector2(w, 0),
                                    new Vector2(w, h)
                                });
                            break;
                        
                        // bottom right triangle
                        case "BR":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(0, 0),
                                    new Vector2(w, 0),
                                    new Vector2(0, h)
                                });
                            break;
                    }

                    tileCollider.Transform.Position = tilePair.Item2 * 16f;

                    hit.Pushout = Vector2.Zero;
                    if (collider.Overlaps(tileCollider, out Vector2 pushout))
                        hit.Pushout = pushout;

                    hit.Other = tileCollider;
                    hit.Solid = IsColliderTypeSolid(type);
                    hit.Type = HitType.Tile;
                }

                result.Add((hit, type));
            }

            return result;
        }
    }
}
