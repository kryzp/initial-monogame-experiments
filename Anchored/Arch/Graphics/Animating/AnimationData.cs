﻿using Arch.Assets;
using Arch.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Arch.Graphics.Animating
{
	public class AnimationData
	{
		public Dictionary<string, List<AnimationFrame>> Layers = new Dictionary<string, List<AnimationFrame>>();
		public Dictionary<string, AnimationTag> Tags = new Dictionary<string, AnimationTag>();
		public Dictionary<string, TextureRegion> Slices = new Dictionary<string, TextureRegion>();
		public Texture2D Texture;

		public AnimationData Recolor(Color[] from, Color[] to)
		{
			if (from.Length != to.Length)
			{
				return null;
			}

			AnimationData data = new AnimationData();

			return data;
		}

		public AnimationTag? GetTag(string tagName)
		{
			AnimationTag tag;

			if (tagName == null)
			{
				tag = Tags.FirstOrDefault().Value;
			}
			else if (!Tags.TryGetValue(tagName, out tag))
			{
				return null;
			}

			return tag;
		}

		public TextureRegion GetSlice(string name, bool error = true)
		{
			if (Slices.TryGetValue(name, out var region))
				return region;

			if (!error)
				return null;

			return TextureManager.NULL;
		}

		public AnimationFrame? GetFrame(string layer, uint id)
		{
			List<AnimationFrame> frames;

			if (layer == null)
			{
				frames = Layers.FirstOrDefault().Value;
			}
			else if (!Layers.TryGetValue(layer, out frames))
			{
				return null;
			}

			if (frames.Count < id)
				return null;

			return frames[(int)id];
		}

		public Animation CreateAnimation(string layer = null)
		{
			return new Animation(this, layer);
		}
	}
}
