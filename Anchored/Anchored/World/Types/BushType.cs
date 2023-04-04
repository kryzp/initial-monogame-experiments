using Anchored.Assets.Textures;
using Anchored.Util;
using Anchored.World.Components;
using Arch;
using Arch.Math;
using Arch.Util;
using Arch.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anchored.World.Types
{
	public class BushType : EntityType
	{
		private TextureRegion texture => SubTextureManager.Get(Sheet, Texture);

		[EntityTypeSetting("sheet")]
		public string Sheet = "sheets\\decor";

		[EntityTypeSetting("texture")]
		public string Texture = "bush1";

		public BushType()
		{
		}

		public override void Create(Entity entity)
		{
			base.Create(entity);
			entity.Transform.Origin = new Vector2(8, 16);

			var sprite = entity.AddComponent(new Sprite(texture));

			var depth = entity.AddComponent(new DepthSorter(sprite));

			var collider = entity.AddComponent(new Collider());
			collider.MakeRect(new RectangleF(0, 12, 16, 6));
			collider.Mask = Masks.Solid;
		}

		public override void DrawPreview(SpriteBatch sb, Vector2 position, float scale = 1f, float alpha = 1f)
		{
			if (Sheet == null || Texture == null)
				return;

			if (Sheet != "" && Texture != "")
			{
				texture.Draw(
					position,
					Vector2.Zero,
					Color.White * alpha,
					0f,
					Vector2.One * scale,
					0.5f,
					sb
				);
			}
		}

		public override RectangleF GetDrawPreviewBounds()
		{
			return new RectangleF(0, 0, 16, 16);
		}
	}
}
