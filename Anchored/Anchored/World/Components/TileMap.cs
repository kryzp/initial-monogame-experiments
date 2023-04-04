using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Arch.Assets.Maps;
using Anchored.Assets.Maps;
using Arch;
using System.Reflection;
using System.Linq;
using Arch.Graphics;
using Anchored.Graphics;
using Arch.World;

namespace Anchored.World.Components
{
	public class TileMap : GraphicsComponent
	{
		private AnchoredMap map;
		private List<Collider> colliders;
		
		private Camera camera;

		public AnchoredMap Map => map;

		public TileMap()
		{
			this.colliders = new List<Collider>();
			this.LayerDepth = 0.5f;
		}

		public TileMap(AnchoredMap map, Camera camera)
			: this()
		{
			this.map = map;
			this.camera = camera;
		}

		public override void Update()
		{
			base.Update();

			Map.Update();
		}

		public override void DrawBegin(SpriteBatch sb)
		{
		}

		public override void Draw(SpriteBatch sb)
		{
			Map.Draw(sb);
		}

		public override void DrawEnd(SpriteBatch sb)
		{
		}

		public void LoadEntities()
		{
			EntityWorld world = Entity.World;

			foreach (var e in Map.Entities)
			{
				var name = e.Name;
				var type = e.Type;
				var level = e.Level;
				var position = e.Position;
				var z = e.Z;
				var settings = e.Settings;

				Entity entity = world.AddEntity(name);

				entity.Transform.Position = position;
				entity.Transform.Z = z;
				entity.Level = level;

				type.Create(entity);
			}
		}

		public void ClearColliders()
		{
			for (int ii = 0; ii < colliders.Count; ii += 1)
			{
				Entity entity = colliders[ii].Entity;
				entity.RemoveComponent(colliders[ii]);
			}

			colliders.Clear();
		}
	}
}
