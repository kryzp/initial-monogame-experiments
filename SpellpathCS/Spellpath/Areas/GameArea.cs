using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using Spellpath.Actors;

namespace Spellpath.Areas
{
    public class GameArea
    {
        protected List<Actor> actors = new List<Actor>();
        public List<Actor> Actors => actors;

        public List<Warp> warps = new List<Warp>();
        
        public TiledMap Map { get; protected set; }

        public GameArea()
        {
        }

        public virtual void Update(GameTime time)
        {
            actors.ForEach(it => it.Update(time));

            var warp = CheckForCollidingWarp();
            if (warp != null)
            {
                Game1.WarpPlayer(warp.TargetArea, warp.TargetTileX * 16, warp.TargetTileY * 16);
            }
        }

        public virtual void Draw(GameTime time, SpriteBatch b)
        {
            actors.ForEach(it => it.Draw(time, b));
        }

        public virtual void DrawUI(GameTime time, SpriteBatch b)
        {
            actors.ForEach(it => it.DrawUI(time, b));
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

        public void OverrideMap(string map, Rectangle source, int destX, int destY)
        {
            OverrideMap(
                Game1.TiledContent.Load<TiledMap>(map),
                source,
                destX, destY
            );
        }

        public void OverrideMap(TiledMap overrideMap, Rectangle source, int destX, int destY)
        {
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
        }

        public void LoadMap(string map)
        {
            Map = Game1.TiledContent.Load<TiledMap>(map);
            Game1.MapRenderer.Load(Map);
            UpdateWarps();
        }

        public void UpdateWarps()
        {
            string property = Map.Properties["Warp"];
            var split = property.Split(' ');

            for (int i = 0; i < split.Length; i += 5)
            {
                int fromX = Int32.Parse(split[i]);
                int fromY = Int32.Parse(split[i + 1]);
                var targetArea = split[i + 2];
                int tgtX = Int32.Parse(split[i + 3]);
                int tgtY = Int32.Parse(split[i + 4]);
                
                warps.Add(new Warp(targetArea, fromX, fromY, tgtX, tgtY));
            }
        }
        
        public Warp CheckForCollidingWarp()
        {
            foreach (var warp in warps)
            {
                var warpCollider = new Collider(warp.FromTileX * 16, warp.FromTileY * 16, 16, 16);
                
                if (Game1.Player.Collider.Overlaps(warpCollider, out _))
                    return warp;
            }

            return null;
        }

        public virtual List<Hit> GetCollidingColliders(Collider col)
        {
            List<Hit> result = new List<Hit>();

            foreach (Actor act in actors)
            {
                if (act.Collider != null)
                {
                    if (act.Collider.Overlaps(col, out Vector2 push))
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

        public virtual bool ResolveCollidingPosition(
            Collider collidingPosition,
            Actor character,
            Vector2 amount
        )
        {
            RectangleF colliderRect = collidingPosition.GetWorldBounds();
            {
                colliderRect.X /= 16;
                colliderRect.Y /= 16;
                colliderRect.Width /= 16;
                colliderRect.Height /= 16;
            }

            TiledMapTileLayer metaLayer = Map.GetLayer<TiledMapTileLayer>("Collision");
            List<TiledMapTile> collidingTiles = new List<TiledMapTile>();

            foreach (TiledMapTile tile in metaLayer.Tiles)
            {
                if (!tile.IsBlank)
                {
                    Rectangle tileRect = new Rectangle(tile.X, tile.Y, 1, 1);

                    if (colliderRect.Intersects(tileRect.ToRectangleF()))
                        collidingTiles.Add(tile);
                }
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

                var sortedhits = GetHitsFromColliderAndTiles(collidingPosition, collidingTiles).OrderBy(x => x.Item1.GetDistanceFromOtherSquared(collidingPosition, true)).ToList();
                sortedhits.RemoveAll(x => !x.Item1.Solid);
                ResolveCollisionHits(character, collidingPosition, sortedhits);
            }

            return false;
        }

        private void ResolveCollisionHits(Actor character, Collider collidingPosition, List<(Hit, string)> hits)
        {
            foreach (var hit in hits)
            {
                character.Transform.Position += hit.Item1.Pushout;

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

        private List<(Hit, string)> GetHitsFromColliderAndTiles(Collider collider, List<TiledMapTile> collidingTiles)
        {
            List<(Hit, string)> result = new List<(Hit, string)>();

            TiledMapTileset tileset = Map.GetTilesetByTileGlobalIdentifier(collidingTiles[0].GlobalIdentifier);
            int fid = Map.GetTilesetFirstGlobalIdentifier(tileset);

            List<Collider> colliders = new List<Collider>();

            foreach (TiledMapTile tile in collidingTiles)
            {
                int lid = tile.GlobalIdentifier - fid;
                TiledMapTilesetTile tilesetTile = tileset.Tiles.Find(x => x.LocalTileIdentifier == lid);
                string type = tilesetTile.Properties["ColliderType"];

                Hit hit = new Hit();
                {
                    Collider tileCollider = new Collider();

                    switch (type)
                    {
                        case "S":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(0, 0),
                                    new Vector2(16, 0),
                                    new Vector2(16, 16),
                                    new Vector2(0, 16)
                                });
                            break;

                        case "TL":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(16, 0),
                                    new Vector2(0, 16),
                                    new Vector2(16, 16)
                                });
                            break;

                        case "TR":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(0, 0),
                                    new Vector2(0, 16),
                                    new Vector2(16, 16)
                                });
                            break;

                        case "BL":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(0, 0),
                                    new Vector2(16, 0),
                                    new Vector2(16, 16)
                                });
                            break;

                        case "BR":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(0, 0),
                                    new Vector2(16, 0),
                                    new Vector2(0, 16)
                                });
                            break;

                        /*
                        case "StairLeft":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(0, 0),
                                    new Vector2(64, 0),
                                    new Vector2(64, 64),
                                    new Vector2(0, 64)
                                });
                            break;

                        case "StairRight":
                            tileCollider.MakePolygon(new List<Vector2>()
                                {
                                    new Vector2(0, 0),
                                    new Vector2(64, 0),
                                    new Vector2(64, 64),
                                    new Vector2(0, 64)
                                });
                            break;
                        */
                    }

                    tileCollider.Transform.Position = new Vector2(tile.X*16, tile.Y*16);

                    hit.Pushout = Vector2.Zero;
                    if (collider.Overlaps(tileCollider, out Vector2 pushout))
                        hit.Pushout = pushout;

                    hit.Other = tileCollider;
                    hit.Solid = IsColliderTypeSolid(type);
                }

                result.Add((hit, type));
            }

            return result;
        }
    }
}
