using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollisionTest2
{
	public interface IComponent
	{
		void Update(float deltaTime);
		void Draw(float deltaTime, SpriteBatch spriteBatch);
	}
}
