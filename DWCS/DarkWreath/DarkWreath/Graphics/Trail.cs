using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath
{
	public class Trail
	{
		private class Particle
		{
			public float Opacity;
			public Vector2 Position;
		}

		private List<Particle> particles = new List<Particle>();

		public Color Colour;
		public float FadeTime;

		public Trail()
		{
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < particles.Count; i++)
			{
				var particle = particles[i];
				
				particle.Opacity -= 1f / FadeTime;

				if (particle.Opacity <= 0f)
					particles.Remove(particle);
			}
		}
		
		public void Draw(SpriteBatch b, float layerDepth)
		{
			for (int i = 0; i < particles.Count - 1; i++)
			{
				var currParticle = particles[i];
				var nextParticle = particles[i + 1];

				b.DrawLine(
					currParticle.Position,
					nextParticle.Position,
					Colour * currParticle.Opacity,
					layerDepth,
					0.5f
				);
				
				/*
				b.Draw(
					ParticleTexture.Texture,
					particle.Position,
					ParticleTexture.Region,
					Color.White * particle.Opacity,
					0f,
					Vector2.Zero,
					1f,
					SpriteEffects.None,
					layerDepth
				);
				*/
			}
		}

		public void AddPartile(Vector2 position)
		{
			particles.Add(new Particle()
			{
				Position = position,
				Opacity = 1f
			});
		}
	}
}
