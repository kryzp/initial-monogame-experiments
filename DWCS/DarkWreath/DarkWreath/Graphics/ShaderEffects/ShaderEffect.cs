using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Graphics.ShaderEffects
{
	public abstract class ShaderEffect
	{
		public Effect Effect;
		
		public ShaderEffect(ContentManager content, string effectName)
		{
			this.Effect = content.Load<Effect>(effectName);
		}

		public virtual void Init()
		{
		}
		
		public virtual void Update()
		{
		}
		
		public virtual void Apply()
		{
			Effect?.CurrentTechnique.Passes[0].Apply();
		}
	}
}
