using Anchored.Assets.Textures;
using Anchored.Graphics;
using Anchored.World.Components;
using Arch;
using Arch.Math;
using Arch.Util;
using Arch.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anchored.World.Types
{
    public class TreeType : EntityType
    {
        [EntityTypeSetting("sheet")]
        public string Sheet;

        [EntityTypeSetting("texture")]
        public string Texture;
        
        public TreeType()
        {
        }

        public override void Create(Entity entity)
        {
            base.Create(entity);

            var tex = SubTextureManager.Get(Sheet, Texture);

            entity.Transform.Origin = Utility.GetOrigin(OriginPosition.BottomCenter, tex);

            var sprite = entity.AddComponent(new Sprite(tex));

            var collider = entity.AddComponent(new Collider());
            collider.Mask = Masks.Solid;
            collider.MakeRect(
                new RectangleF(
                    0,
                    0,
                    16,
                    16
                )
            );

            collider.Transform.Position = new Vector2(16, 48);

            var depth = entity.AddComponent(new DepthSorter(sprite));
        }

        public override void DrawPreview(SpriteBatch sb, Vector2 position, float scale = 1f, float alpha = 1f)
        {
            if (Sheet == null || Texture == null)
                return;

            if (Sheet != "" && Texture != "")
            {
                SubTextureManager.Get(Sheet, Texture).Draw(
                    position,
                    Vector2.Zero,
                    Color.White * alpha,
                    0f,
                    Vector2.One,
                    0.5f,
                    sb
                );
            }
        }

		public override RectangleF GetDrawPreviewBounds()
		{
            return new RectangleF(0, 0, 1, 1);
		}
	}
}
