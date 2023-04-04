using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DarkWreath
{
	public struct AnimationFrame
	{
		public delegate void OnAnimationFrame();

		public Rectangle Region;
		public int Milliseconds;
		public bool Flip;
		public OnAnimationFrame OnFrameStart;
		public OnAnimationFrame OnFrameEnd;

		public AnimationFrame(
			Rectangle region,
			int milliseconds,
			bool flip,
			OnAnimationFrame onFrameStart = null,
			OnAnimationFrame onFrameEnd = null
		)
		{
			this.Region = region;
			this.Milliseconds = milliseconds;
			this.Flip = flip;
			this.OnFrameStart = onFrameStart;
			this.OnFrameEnd = onFrameEnd;
		}
	}
	
	public class AnimatedSprite
	{
		private Rectangle sourceRect;
		
		public Texture2D Texture { get; private set; }

		public Rectangle SourceRect
		{
			get
			{
				UpdateSourceRect();
				return sourceRect;
			}

			private set
			{
				sourceRect = value;
			}
		}

		public int TextureWidth => Texture != null ? Texture.Width : 0;
		public int TextureHeight => Texture != null ? Texture.Height : 0;

		public float Timer { get; private set; }

		public List<AnimationFrame> Frames { get; private set; }
		public int FrameCount => Frames.Count;
		public int CurrentFrameIndex { get; private set; }
		public bool Loop = true;

		public AnimationFrame CurrentFrame => Frames[CurrentFrameIndex];

		public delegate void EndOfAnimationFn();

		public AnimatedSprite()
		{
			Texture = null;
			Frames = new List<AnimationFrame>();
		}

		public AnimatedSprite(Texture2D texture)
		{
			Texture = texture;
			Frames = new List<AnimationFrame>()
			{
				new AnimationFrame(Rectangle.Empty, 0, false)
			};
		}

		public AnimatedSprite(Texture2D texture, List<AnimationFrame> frames)
			: this(texture)
		{
			Frames = frames;

			UpdateSourceRect();
		}

		public void AnimateForward(GameTime time)
		{
			Timer += (float)time.ElapsedGameTime.TotalMilliseconds;

			if (Timer > CurrentFrame.Milliseconds)
			{
				CurrentFrameIndex++;
				Timer = 0.0f;

				if (CurrentFrameIndex >= FrameCount && Loop)
					CurrentFrameIndex = 0;
			}

			UpdateSourceRect();
		}

		public virtual void UpdateSourceRect()
		{
			var source = CurrentFrame.Region;

			if (source == Rectangle.Empty)
				source = new Rectangle(0, 0, TextureWidth, TextureHeight);

			this.sourceRect = source;

			/*
			this.sourceRect = new Rectangle(
				(Frames[CurrentFrame].Frame * SpriteWidth) % TextureWidth,
				(Frames[CurrentFrame].Frame * SpriteWidth) / TextureWidth * SpriteHeight,
				SpriteWidth,
				SpriteHeight
			);
			*/
		}

		public virtual void Draw(
			SpriteBatch b,
			Vector2 position,
			Vector2 origin,
			float layerDepth,
			int xOffset,
			int yOffset,
			Color c,
			Vector2 scale,
			float rotation = 0.0f,
			bool flip = false
		)
		{
			if (this.Texture == null)
				return;
			
			bool flipflip = Frames[CurrentFrameIndex].Flip;

			if (flip)
				flipflip = !flipflip;

			b.Draw(
				this.Texture,
				position,
				new Rectangle(
					SourceRect.X + xOffset,
					SourceRect.Y + yOffset,
					SourceRect.Width,
					SourceRect.Height
				),
				c,
				rotation,
				origin,
				scale,
				flipflip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
				layerDepth
			);
		}

		public void SetCurrentAnimation(List<AnimationFrame> frames)
		{
			Frames = frames;
		}
	}
}
