using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Sprites
{
	public class Bullet : Sprite, ICollidable
	{
		private float timer = 0f;

		public float LifeSpan { get; set; }
		public Vector2 Velocity { get; set; }
		
		public Bullet(Texture2D tex)
			: base(tex)
		{
		}

		public override void Update(GameTime gameTime)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			timer += delta;

			if(timer >= LifeSpan)
				IsRemoved = true;

			Position += Velocity * delta;
		}

		public void OnCollide(Sprite other)
		{
			if(other is Bullet)
				return;

			if(other is Player && this.Parent is Player)
				return;

			if(other is Player && ((Player)other).IsDead)
				return;

			if(other is Asteroid && this.Parent is Player)
			{
				((Player)Parent).Score.Value += 5;
				((Asteroid)other).Destroy();
			}
			
			IsRemoved = true;
		}
	}
}