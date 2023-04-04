using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellpath.Actors;

namespace Spellpath.Tools
{
    public abstract class Tool : Item
    {
        public virtual void DoFunction(Player player) { }
        public virtual void Update(Player player, GameTime time) { }
        public virtual void Draw(Player player, GameTime time, SpriteBatch b, float layerDepthb) { }
        public virtual void DrawUI(Player player, GameTime time, SpriteBatch b, float layerDepth) { }
    }
}
