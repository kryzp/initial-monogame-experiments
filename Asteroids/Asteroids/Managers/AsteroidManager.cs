using System.Collections.Generic;
using Asteroids.Models;
using Asteroids.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Managers
{
	public class AsteroidManager
	{
		private List<Texture2D> textures;
		private float timer;
		
		public float SpawnTimer { get; set; }

		public bool CanAdd { get; set; }

		public AsteroidManager(ContentManager content)
		{
			textures = new List<Texture2D>()
			{
				content.Load<Texture2D>("smallAsteroids"),
				content.Load<Texture2D>("asteroids")
			};
		}

		public void Update(GameTime gameTime)
		{
			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			CanAdd = false;

			if(timer >= SpawnTimer)
			{
				CanAdd = true;
				timer = 0;
			}
		}

		public Asteroid GetAsteroid()
		{
			return new Asteroid(new Dictionary<string, Animation>()
			{
				{
					"Types",
					new Animation(textures[1], 4, 1)
					{
						IsLooping = false
					}
				}
			})
			{
				SmallAsteroid = new SmallAsteroid(new Dictionary<string, Animation>()
				{
					{
						"Types",
						new Animation(textures[0], 4, 1)
						{
							IsLooping = false
						}
					}
				})
				{
					Colour = Color.White,
					Position = Vector2.Zero,
					Layer = 0.35f
				},
				Colour = Color.White,
				Position = Vector2.Zero,
				Layer = 0.4f
			};
		}
	}
}