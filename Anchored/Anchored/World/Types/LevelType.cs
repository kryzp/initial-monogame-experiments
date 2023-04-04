using Anchored.Assets;
using Anchored.World.Components;
using Arch.Assets.Maps;
using Arch.Streams;
using Arch.Assets;
using Anchored.Assets.Maps;
using Arch.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Arch.World;
using Arch;
using Arch.Math;

namespace Anchored.World.Types
{
	public class LevelType : EntityType
	{
		private AnchoredMap map;
		private bool loadEntities;

		public LevelType(AnchoredMap map, bool loadEntities = true)
		{
			Serializable = true;
			
			this.map = map;
			this.loadEntities = loadEntities;
		}

		public override void Create(Entity entity)
		{
			base.Create(entity);

			var tileMap = entity.AddComponent(new TileMap(map, Camera.Main));

			if (loadEntities)
				tileMap.LoadEntities();
		}

		public override void DrawPreview(SpriteBatch sb, Vector2 position, float scale = 1f, float alpha = 1f)
		{
		}

		protected void LoadEntitiesFromTileMap(EntityWorld world, AnchoredMap map)
		{
			/*
			foreach (var obj in map.GetLayer<TiledMapObjectLayer>("Entities").Objects)
			{
				TiledMapTileObject entityObj = (TiledMapTileObject)obj;
				string entityName = entityObj.Name;
				Vector2 entityPosition = entityObj.Position;
				float entityRotation = entityObj.Rotation;

				var entity = world.AddEntity(entityName);

				if (entityObj.Tile.Type == "Destructible")
				{
					string entityTypeName = entityObj.Tile.Properties["Entity"];
					var tileRegion = entityObj.Tileset.GetTileRegion(entityObj.Tile.LocalTileIdentifier);
					var sprite = new TextureRegion(entityObj.Tileset.Texture, tileRegion);

					EntityType entityType = (EntityType)Activator.CreateInstance(
						Type.GetType(
							$"Anchored.World.Types.{entityTypeName}Type",
							true,
							false
						),
						sprite
					);

					entityType.Create(entity);
				}

				entity.Transform.Position = entityPosition;
				entity.Transform.RotationDegrees = entityRotation;
			}
			*/
		}

		public override RectangleF GetDrawPreviewBounds()
		{
			return RectangleF.Empty;
		}
	}
}
