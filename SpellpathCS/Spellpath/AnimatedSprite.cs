using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Spellpath
{
	public class AnimatedSprite
	{
		public Texture2D Texture { get; private set; }
		public Rectangle SourceRect { get; private set; }

		public int TextureWidth => Texture != null ? Texture.Width : 0;
		public int TextureHeight => Texture != null ? Texture.Height : 0;

		public float Timer { get; private set; }

		public int SpriteWidth { get; private set; }
		public int SpriteHeight { get; private set; }

		public List<AnimationFrame> Frames { get; private set; }
		public int FrameCount => Frames.Count;
		public int CurrentFrame { get; private set; }
		public bool Loop = true;

		public delegate void EndOfAnimationFn();

		public AnimatedSprite()
		{
			Texture = null;
			Frames = new List<AnimationFrame>();

			SpriteWidth = 0;
			SpriteHeight = 0;
		}

		public AnimatedSprite(string fileName, int spriteWidth, int spriteHeight)
		{
			Texture = Game1.Content.Load<Texture2D>(fileName);
			Frames = new List<AnimationFrame>();

			SpriteWidth = spriteWidth;
			SpriteHeight = spriteHeight;
		}

		public AnimatedSprite(string fileName, List<AnimationFrame> frames, int spriteWidth, int spriteHeight)
			: this(fileName, spriteWidth, spriteHeight)
		{
			Frames = frames;

			UpdateSourceRect();
		}

		public AnimatedSprite(string fileName, int frames, int milliseconds, int spriteWidth, int spriteHeight)
			: this(fileName, spriteWidth, spriteHeight)
		{
			for (int i = 0; i < frames; i++)
				Frames.Add(new AnimationFrame(i, milliseconds, false));

			UpdateSourceRect();
		}

		public void AnimateOnce(int initialFrame, int frames, float interval, EndOfAnimationFn endOfAnimationFn)
		{
		}

		public void AnimateForward(GameTime time)
		{
			Timer += (float)time.ElapsedGameTime.TotalMilliseconds;

			if (Timer > Frames[CurrentFrame].Milliseconds)
			{
				CurrentFrame++;
				Timer = 0.0f;

				if (CurrentFrame >= FrameCount && Loop)
					CurrentFrame = 0;
			}

			UpdateSourceRect();
		}

		public virtual void UpdateSourceRect()
		{
			this.SourceRect = new Rectangle(
				(Frames[CurrentFrame].Frame * SpriteWidth) % TextureWidth,
				(Frames[CurrentFrame].Frame * SpriteWidth) / TextureWidth * SpriteHeight,
				SpriteWidth,
				SpriteHeight
			);
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

			bool flipflip = Frames[CurrentFrame].Flip;

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

	public struct AnimationFrame
	{
		public delegate void OnAnimationFrame();

		public int Frame;
		public int Milliseconds;
		public bool Flip;
		public OnAnimationFrame OnFrameStart;
		public OnAnimationFrame OnFrameEnd;

		public AnimationFrame(
			int frame,
			int milliseconds,
			bool flip,
			OnAnimationFrame onFrameStart = null,
			OnAnimationFrame onFrameEnd = null
		)
		{
			this.Frame = frame;
			this.Milliseconds = milliseconds;
			this.Flip = flip;
			this.OnFrameStart = onFrameStart;
			this.OnFrameEnd = onFrameEnd;
		}
	}
}
