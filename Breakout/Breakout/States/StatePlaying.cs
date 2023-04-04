using System;
using System.Collections.Generic;
using System.Linq;
using Breakout.Entities;
using Breakout.Entities.Sprites;
using Breakout.Entities.Sprites.Bricks;
using Breakout.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*
 * Quick Mini-Postmortem: I Know very well many ways in which this could be optimized, for example I could have
 * an 'Instantiate' function that just takes in a prefab of an object in this State allowing for easy creation
 * of entities, but this is a small project so I don't feel like going through the effort of creating a proper
 * ECS, though when I prepare to tackle larger projects that is something that I will 'attempt' to create.
 */

/*
 * Also, by the way, I know it would be better to move the game over into a new state but Its 10:52 and
 * I don't really feel like it.
 */

namespace Breakout.States
{
    public class StatePlaying : StateBase
    {
        private SpriteFont scoreFont;
        private SpriteFont finalScoreFont;

        private float medalScaleLerp;
        private float medalScale;

        private bool once = true; // <- I am so sorry this is a horrible choice to make

        private int targetFinalScore;
        private int finalScore;
        
        private Texture2D ballTex;
        
        private Texture2D gameOverImage;
        private List<Texture2D> medals;

        private List<Texture2D> brickTextures;
        
        public static List<Entity> Entities;

        public bool GameOver { get; set; }
        
        public static int Score { get; set; }

        public static bool SpawnBalls = false;

        public StatePlaying(Game1 game, ContentManager content)
            : base(game, content)
        {
        }

        public override void LoadContent()
        {
            this.ballTex = Content.Load<Texture2D>("world/ball");
            var paddleTex = Content.Load<Texture2D>("world/paddle"); 
            
            brickTextures = new List<Texture2D>();
            brickTextures.Add(Content.Load<Texture2D>("world/bricks/brick0"));
            brickTextures.Add(Content.Load<Texture2D>("world/bricks/brick1"));
            brickTextures.Add(Content.Load<Texture2D>("world/bricks/ballBrick"));
            brickTextures.Add(Content.Load<Texture2D>("world/bricks/scoreBrick"));
            brickTextures.Add(Content.Load<Texture2D>("world/bricks/timeBrick"));
            brickTextures.Add(Content.Load<Texture2D>("world/bricks/bombBrick"));

            scoreFont = Content.Load<SpriteFont>("fonts/pixellari");
            finalScoreFont = Content.Load<SpriteFont>("fonts/pixellari");

            gameOverImage = Content.Load<Texture2D>("gui/gameOverImage");
            
            medals = new List<Texture2D>();
            medals.Add(Content.Load<Texture2D>("gui/medals/bronzeMedal"));
            medals.Add(Content.Load<Texture2D>("gui/medals/silverMedal"));
            medals.Add(Content.Load<Texture2D>("gui/medals/leafMedal"));
            medals.Add(Content.Load<Texture2D>("gui/medals/lavaMedal"));

            Entities = new List<Entity>();
            
            var ballPrefab = new Ball(ballTex)
            {
                Position = new Vector2(Game1.WorldWidth / 2f, Game1.WorldHeight / 2f),
                Colour = Color.White,
                Layer = 0.3f
            };

            var paddlePrefab = new Paddle(paddleTex)
            {
                Position = new Vector2(Game1.WorldWidth / 2f, Game1.WorldHeight - 32.5f),
                Colour = Color.White,
                Layer = 0.4f,
                Input = new PlayerInput()
                {
                    MoveLeft = Keys.A,
                    MoveRight = Keys.D,
                    
                    AltMoveLeft = Keys.Left,
                    AltMoveRight = Keys.Right
                }
            };
            
            Entities.Add(ballPrefab);
            Entities.Add(paddlePrefab);

            GenerateBricks(brickTextures, 12, 5);
        }

        public override void UnloadContent()
        {
        }

        public override void Update(float deltaTime)
        {
            if(GameOver) return;

            foreach(var entity in Entities)
                entity.Update(deltaTime);

            if(Entities.Count(c => c is Brick) == 0)
            {
                GenerateBricks(brickTextures, 12, 5);
            }

            if(Entities.Count(c => c is Ball) == 0)
            {
                GameOver = true;
            }
            
            if(SpawnBalls)
            {
                for(int ii = 0; ii < 2; ii++)
                {
                    Entities.Add(new Ball(ballTex)
                    {
                        Position = new Vector2(Game1.WorldWidth / 2f, Game1.WorldHeight / 2f),
                        Colour = Color.White,
                        Layer = 0.3f
                    });
                }

                SpawnBalls = false;
            }
        }

        public override void PostUpdate(float deltaTime)
        {
            /*
             * I think I must have passed out halfway though writing this -
             * it works but I'm not touching it since it is very clearly
             * being held together with glue and duct tape.
             */
            
            var collidableSprites = Entities.Where(c => c is Sprite && c is ICollidable);

            foreach(Sprite a in collidableSprites)
            {
                foreach(Sprite b in collidableSprites)
                {
                    if(a == b) continue;
                    
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
                
                // ?? (entity.Children = new List<Entity>();) ??
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
            if(GameOver) return;
            
            foreach(var entity in Entities)
                entity.Draw(deltaTime, spriteBatch);
        }

        public override void DrawGUI(float deltaTime, SpriteBatch spriteBatch)
        {
            float scoreHeight = scoreFont.MeasureString(Score.ToString()).Y;

            Vector2 scorePosition = new Vector2(
                Game1.ScreenWidth / 2f,
                Game1.ScreenHeight - 50f
            );
            
            if(GameOver)
            {
                scorePosition = new Vector2(
                    (Game1.ScreenWidth / 2f) + 40.5f,
                    (Game1.ScreenHeight / 2f) + 82.5f
                );
            }
            
            if(!GameOver)
            {
                spriteBatch.DrawString(
                    scoreFont,
                    Score.ToString(),
                    scorePosition,
                    Color.White,
                    0f,
                    new Vector2(
                        0f,
                        scoreHeight / 2f
                    ),
                    1f,
                    SpriteEffects.None,
                    1f
                );
            }

            if(GameOver)
            {
                if(once)
                {
                    targetFinalScore = Score;
                    once = false;
                }
                
                Texture2D medal = medals[0];

                // Bronze Medal
                if(targetFinalScore >= 0 && targetFinalScore < 1250)
                    medal = medals[0];
                
                // Silver Medal
                else if(targetFinalScore >= 1250 && targetFinalScore < 2500)
                    medal = medals[1];
                
                // Leaf Medal
                else if(targetFinalScore >= 2500 && targetFinalScore < 5000)
                    medal = medals[2];
                
                // Lava Medal
                else if(targetFinalScore >= 5000)
                    medal = medals[3];

                // Draw Medal Podium
                spriteBatch.Draw(
                    gameOverImage,
                    new Vector2(
                        Game1.ScreenWidth / 2f,
                        Game1.ScreenHeight / 2f
                    ),
                    null,
                    Color.White,
                    0f,
                    new Vector2(
                        gameOverImage.Width / 2f,
                        gameOverImage.Height / 2f
                    ),
                    12f,
                    SpriteEffects.None,
                    0.9f
                );
                
                // Draw Medal
                spriteBatch.Draw(
                    medal,
                    new Vector2(
                        (Game1.ScreenWidth / 2f) + (8 * 12) + 12.0f,
                        (Game1.ScreenHeight / 2f) - 66f
                    ),
                    null,
                    Color.White,
                    0f,
                    new Vector2(
                        medal.Width / 2f,
                        medal.Height / 2f
                    ), 
                    medalScale,
                    SpriteEffects.None,
                    0.95f
                );

                // Final Score
                spriteBatch.DrawString(
                    finalScoreFont,
                    finalScore.ToString(),
                    new Vector2(
                        scorePosition.X,
                        scorePosition.Y + 12f
                    ), 
                    new Color(125, 56, 51),
                    0f,
                    new Vector2(
                        (finalScoreFont.MeasureString(finalScore.ToString()).X) / 2f,
                        (finalScoreFont.MeasureString(finalScore.ToString()).Y) / 2f
                    ),
                    2f,
                    SpriteEffects.None,
                    0.98f
                );
                
                spriteBatch.DrawString(
                    finalScoreFont,
                    finalScore.ToString(),
                    scorePosition,
                    new Color(207, 117, 43),
                    0f,
                    new Vector2(
                        (finalScoreFont.MeasureString(finalScore.ToString()).X) / 2f,
                        (finalScoreFont.MeasureString(finalScore.ToString()).Y) / 2f
                    ),
                    2f,
                    SpriteEffects.None,
                    0.99f
                );

                if(finalScore != targetFinalScore)
                {
                    // this does give very false results (by about 100-50 pts or so) but it looks much nicer so shut.
                    finalScore = (int)Math.Ceiling(MathHelper.LerpPrecise(finalScore, targetFinalScore, 0.1f));
                }
                
                medalScaleLerp = MathHelper.Lerp(medalScaleLerp, (12f - medalScale) * 0.5f, 0.15f);
                medalScale += medalScaleLerp;
            }
        }

        private void GenerateBricks(List<Texture2D> brickTextures, int xAmount, int yAmount)
        {
            bool useColourA = true;
            bool useSpecialBrick = false;

            for(int yy = 10; yy < (15 * yAmount); yy += 15)
            {
                for(int xx = 16; xx < Game1.WorldWidth; xx += (Game1.WorldWidth / xAmount))
                {
                    Texture2D brickTex;

                    useSpecialBrick = Game1.Random.Next(100) > 80;
                    
                    if(useSpecialBrick)
                    {
                        int rand = Game1.Random.Next(100);
                        if(rand > 90)
                        {
                            brickTex = brickTextures[5];
                        }
                        else if(rand > 80)
                        {
                            brickTex = brickTextures[4];
                        }
                        else if(rand > 70)
                        {
                            brickTex = brickTextures[2];
                        }
                        else
                        {
                            brickTex = brickTextures[3];
                        }
                    }
                    else
                        if(useColourA)
                            brickTex = brickTextures[0];
                        else
                            brickTex = brickTextures[1];

                    useColourA = !useColourA;

                    var brick = new Brick(brickTex) // Regular Brick
                    {
                        Position = new Vector2(Game1.WorldWidth / 2f, 0f),
                        Colour = Color.White,
                        Layer = 0.25f
                    };
                    
                    if(brickTex == brickTextures[2]) // Ball Brick
                    {
                        brick = new BallBrick(brickTex)
                        {
                            Position = new Vector2(Game1.WorldWidth / 2f, 0f),
                            Colour = Color.White,
                            Layer = 0.25f
                        };
                    }
                    else if(brickTex == brickTextures[3]) // Score Brick
                    {
                        brick = new ScoreBrick(brickTex)
                        {
                            Position = new Vector2(Game1.WorldWidth / 2f, 0f),
                            Colour = Color.White,
                            Layer = 0.25f
                        };
                    }
                    else if(brickTex == brickTextures[4]) // Time Brick
                    {
                        brick = new TimeBrick(brickTex)
                        {
                            Position = new Vector2(Game1.WorldWidth / 2f, 0f),
                            Colour = Color.White,
                            Layer = 0.25f
                        };
                    }
                    else if(brickTex == brickTextures[5]) // Bomb Brick
                    {
                        brick = new BombBrick(brickTex)
                        {
                            Position = new Vector2(Game1.WorldWidth / 2f, 0f),
                            Colour = Color.White,
                            Layer = 0.25f
                        };
                    }
                    
                    brick.Position = new Vector2(xx, yy);
                    Entities.Add(brick);
                }
                
                useColourA = !useColourA;
            }
        }
    }
}