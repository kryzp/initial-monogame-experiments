using ECS1.ComponentArchitecture.Components.Transform;
using ECS1.ComponentArchitecture.Components.Visual;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS1.ComponentArchitecture.Components.Physics
{
	public class TriggerColliderComponent : EntityComponent
	{
		private SpriteComponent sprite;

		public Rectangle CollisionArea { get; set; }

		// TODO: I'm not sure if this is right but hopefully it is so bad I get scared to look at it again!
		public delegate void TriggerEnter();
		public delegate void Trigger();
		public delegate void TriggerExit();

		public TriggerEnter OnTriggerEnter;
		public Trigger OnTrigger;
		public TriggerExit OnTriggerExit;

		public TriggerColliderComponent(SpriteComponent sprite)
		{
			this.sprite = sprite;
		}

		public override void Update(float deltaTime)
		{
			CollisionArea = new Rectangle(
				sprite.Rectangle.X,
				sprite.Rectangle.Y,
				sprite.Rectangle.Width,
				sprite.Rectangle.Height
			);

			if(Intersects(CollisionArea, new Rectangle(0, 0, 15, 16)))
			{
				sprite.Colour = Color.Red;
			}
			else
			{
				sprite.Colour = Color.Blue;
			}
		}

		public override void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
		}

		private bool Intersects(Rectangle r1, Rectangle r2)
		{
			return (
				(r1.X < r2.X + r2.Width) &&
				(r1.X + r1.Width > r2.X) &&
				(r1.Y < r2.Y + r2.Height) &&
				(r1.Y + r1.Height > r2.Y)
			);
		}
	}
}
