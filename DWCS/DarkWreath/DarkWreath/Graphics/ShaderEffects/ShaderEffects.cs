using Microsoft.Xna.Framework.Content;

namespace DarkWreath.Graphics.ShaderEffects
{
	public static class ShaderEffects
	{
		public static MarauderUIEffect MarauderUIEffect;

		public static void Load(ContentManager content)
		{
			MarauderUIEffect = new MarauderUIEffect(content);
		}
	}
}
