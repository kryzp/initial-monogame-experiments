using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Graphics.ShaderEffects
{
	public class MarauderUIEffect : ShaderEffect
	{
		private float fadeInTimer;
		
		public MarauderUIEffect(ContentManager content)
			: base(content, "shaders/marauder_ui")
		{
		}

		public override void Init()
		{
			fadeInTimer = 0f;
		}

		public override void Update()
		{
			fadeInTimer += Time.Delta;
		}
		
		public override void Apply()
		{
			Effect?.Parameters["uFadeTime"].SetValue((int)fadeInTimer);
			Effect?.Parameters["uTime"].SetValue((int)Time.Total);
			ShaderEffects.MarauderUIEffect.Effect.Parameters["uWindowHeight"].SetValue(Game1.WINDOW_HEIGHT);
			Effect?.CurrentTechnique.Passes[0].Apply();
		}
	}
}
