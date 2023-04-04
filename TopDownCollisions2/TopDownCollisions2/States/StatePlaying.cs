using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownCollisions2.Entities;
using TopDownCollisions2.Entities.Sprites;

namespace TopDownCollisions2.States
{
    public class StatePlaying : StateBase
    {
        public static List<Entity> Entities;
        
        public StatePlaying(Game1 game, ContentManager content)
            : base(game, content)
        {
        }

        public override void LoadContent()
        {
            var playerTex = Content.Load<Texture2D>("world/player");
            var solidTex = Content.Load<Texture2D>("world/debugSolid");

            var playerPrefab = new Player(playerTex)
            {
                Position = new Vector2(100, 50)
            };

            var solidPrefab = new Solid(solidTex)
            {
                Position = new Vector2(Game1.WorldWidth / 2f, Game1.WorldHeight / 2f)
            };

            Entities = new List<Entity>();
            
            Entities.Add(playerPrefab);
            Entities.Add(solidPrefab);
        }

        public override void UnloadContent()
        {
        }

        public override void Update(float deltaTime)
        {
            foreach(var entity in Entities)
            {
                entity.Update(deltaTime);
            }
        }

        public override void PostUpdate(float deltaTime)
        {
            var collidableSprites = Entities.Where(c => c is Sprite && c is ICollidable);
            foreach(Sprite a in collidableSprites)
            {
                foreach(Sprite b in collidableSprites)
                {
                    if(a == b)
                        continue;
                    
                    if(a.Intersects(b, Vector2.Zero))
                        ((ICollidable)a).OnCollide(b);
                }
            }

            int entityCount = Entities.Count;

            for(int ii = 0; ii < entityCount; ii++)
            {
                Entity entity = Entities[ii];
                for(int jj = 0; jj < entity.Children.Count; jj++)
                {
                    Entities.Add(entity.Children[jj]);
                }
            }

            for(int ii = 0; ii < Entities.Count; ii++)
            {
                if(Entities[ii].IsRemoved)
                {
                    Entities.RemoveAt(ii);
                }
            }
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            foreach(var entity in Entities)
            {
                entity.Draw(deltaTime, spriteBatch);
            }
        }

        public override void DrawGUI(float deltaTime, SpriteBatch spriteBatch)
        {
        }
    }
}