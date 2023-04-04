using System.Collections.Generic;
using System.Linq;
using Arch.Assets.Maps.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Arch.Assets.Maps
{
	public class Map
	{
		public string Name = "";

		public int MapWidth = 0;
		public int MapHeight = 0;
		
		public int MasterLevel = 0;
		public List<Level> Levels = new List<Level>();

		public List<Layer> Layers = new List<Layer>();

		protected MapJson jsonData;
		public MapJson JsonData
		{
			get
			{
				UpdateJsonData();
				return jsonData;
			}

			set
			{
				jsonData = value;
			}
		}

		public Map(MapJson data)
		{
			this.jsonData = data;

			this.Name = data.Name;

			this.MapWidth = data.MapWidth;
			this.MapHeight = data.MapHeight;

			this.MasterLevel = data.MasterLevel;

			this.Levels.Clear();
			foreach (var d in data.Levels)
				this.Levels.Add(new Level(d));

			this.Layers.Clear();
			foreach (var d in data.Layers)
				this.Layers.Add(new Layer(d));
		}

		public virtual void UpdateJsonData()
		{
			jsonData.Name = this.Name;

			jsonData.MapWidth = this.MapWidth;
			jsonData.MapHeight = this.MapHeight;

			jsonData.MasterLevel = this.MasterLevel;

			jsonData.Levels.Clear();
			foreach (var d in this.Levels)
				jsonData.Levels.Add(new LevelJson()
				{
					Height = d.Height
				});

			jsonData.Layers.Clear();
			foreach (var d in this.Layers)
				jsonData.Layers.Add(new LayerJson()
				{
					Name = d.Name,
					Type = d.Type,
					ID = d.ID,
					Level = d.Level,
					Width = d.Width,
					Height = d.Height,
					TilesetName = d.Tileset.Name,
					Opacity = d.Opacity,
					Repeat = d.Repeat,
					Distance = d.Distance,
					YDistance = d.YDistance,
					TileSize = d.Tileset.TileSize,
					MoveSpeed = new Vector2Json(d.MoveSpeed.X, d.MoveSpeed.Y),
					Data = d.Data
				});
		}

		public void Update()
		{
			foreach (var layer in Layers)
			{
				layer.Update();
			}
		}
		
		public void Draw(SpriteBatch sb, float scale = 1f)
		{
			foreach (var layer in Layers.OrderBy(x => x.Height))
			{
				layer.Draw(sb, scale);
			}
		}

		public void AddLayer(Layer layer)
		{
			Layers.Add(layer);
		}
	}
}
