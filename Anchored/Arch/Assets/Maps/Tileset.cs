using System;
using System.Collections.Generic;
using Arch.Assets.Maps.Serialization;
using Arch.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arch.Assets.Maps
{
    public class Tileset
    {
        public List<Tile> Tiles = new List<Tile>();
        public Texture2D Texture;
        public int TileSize;
        public string Name;

        public Tileset(TilesetJson data)
		{
            this.Texture = TextureManager.Get(data.Texture).Texture;
            this.TileSize = data.TileSize;

            foreach (var t in data.Tiles)
			{
                Tile tile = new Tile(t);
                this.Tiles.Add(tile);
			}
		}

        public Tile GetTile(UInt16 id)
        {
            return Tiles.Find(x => x.ID == id);
        }

        public TextureRegion GetTileTexture(Tile tile)
		{
            return new TextureRegion(Texture, tile.Bounds);
		}

        public TextureRegion GetTileTexture(Rectangle bounds)
        {
            return new TextureRegion(Texture, bounds);
        }
    }
}
