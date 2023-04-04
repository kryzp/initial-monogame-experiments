using Arch.Math;
using Arch.Streams;
using Arch.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arch.World
{
	public abstract class EntityType
	{
		public bool Serializable = false;

		public virtual void Create(Entity entity)
		{
			entity.Type = this;
		}

		public virtual void Save(FileWriter stream)
		{
		}
		
		public virtual void Load(FileReader stream)
		{
		}

		public virtual void DrawPreview(SpriteBatch sb, Vector2 position, float scale = 1f, float alpha = 1f)
		{
			throw new NotImplementedException();
		}

		public virtual RectangleF GetDrawPreviewBounds()
		{
			throw new NotImplementedException();
		}
	}
}
