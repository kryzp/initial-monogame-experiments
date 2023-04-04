using Arch;
using Arch.Assets.Maps;
using Arch.Assets.Maps.Serialization;
using Arch.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anchored.Assets.Maps
{
	public class AnchoredMap : Map
	{
		public List<MapEntity> Entities = new List<MapEntity>();

		public AnchoredMap(Map map)
			: base(map.JsonData)
		{
			Entities.Clear();

			foreach (var d in map.JsonData.Entities)
			{
				AnchoredMapEntity e = new AnchoredMapEntity(d);

				foreach (var field in e.Type.GetType().GetFields())
				{
					object[] attribs = field.GetCustomAttributes(true);

					foreach (var attrib in attribs)
					{
						if (attrib.GetType() == typeof(EntityTypeSetting))
						{
							var entityAttrib = (EntityTypeSetting)attrib;

							byte[] data = (byte[])(e.Settings[entityAttrib.Name]);

							var variable = Utility.FromByteArray(field.FieldType, data);
							field.SetValue(e.Type, variable);
						}
					}
				}

				Entities.Add(e);
			}
		}

		public AnchoredMap(MapJson data)
			: base(data)
		{
		}

		public override void UpdateJsonData()
		{
			base.UpdateJsonData();

			jsonData.Entities.Clear();
			int i = 0;
			foreach (var d in this.Entities)
			{
				Dictionary<string, byte[]> byteSettings = new Dictionary<string, byte[]>();
				foreach (var set in d.Settings)
					byteSettings.Add(set.Key, (byte[])set.Value);

				Dictionary<string, object> settings = new Dictionary<string, object>();

				foreach (var set in byteSettings)
				{
					settings.Add(set.Key, Utility.FromByteArray(typeof(string), set.Value));
				}

				jsonData.Entities.Add(new MapEntityJson()
				{
					Name = d.Name,
					Type = d.Type.GetType().Name,
					Level = d.Level,
					X = (int)(d.Position.X),
					Y = (int)(d.Position.Y),
					Z = (int)(d.Z),
					Settings = settings
				});

				i++;
			}
		}
	}
}
