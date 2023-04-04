using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spellpath.Tools;
using System.Collections.Generic;
using Spellpath.Input;

namespace Spellpath.Actors
{
    public class Player : Actor
    {
        private List<Item> m_inventory = new List<Item>();
        private int m_currentSelectedItemIndex;
        private InputProvider m_inputProvider;

        public Item CurrentItem
		{
            get
			{
                if (m_inventory.Capacity > 0)
                    return m_inventory[m_currentSelectedItemIndex];

                return null;
			}
		}

        public Tool CurrentTool
        {
            get
			{
                if (CurrentItem != null)
				{
                    if (CurrentItem is Tool)
                        return (Tool)CurrentItem;
				}

                return null;
			}
        }

        public Player()
        {
            Collider.MakeRect(0, 12, 16, 8);

            Sprite = new PlayerSprite("maps\\outdoors_tileset_img", 16, 16);
            Sprite.SetCurrentAnimation(new List<AnimationFrame>()
            {
                new AnimationFrame(0, 0, false)
            });

            //Sprite.Loop = false;

            ExtraTextureScale = new Vector2(1, 2);
            Transform.Origin = new Vector2(8, 16);

            m_inputProvider = new InputProvider();

            // temp debug
            //AddItemToInventory(new ElectrodeRifle());
            m_currentSelectedItemIndex = 0;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            Vector2 delta = m_inputProvider.GetState().Movement;
            
            if (delta != Vector2.Zero)
                Sprite.AnimateForward(time);

            CurrentTool?.Update(this, time);

            Move(delta);
        }

        public override void Draw(GameTime time, SpriteBatch b)
        {
            base.Draw(time, b);
            CurrentTool?.Draw(this, time, b, 0.7f);
        }

        public override void DrawUI(GameTime time, SpriteBatch b)
        {
            base.DrawUI(time, b);
            CurrentTool?.DrawUI(this, time, b, 0.7f);
        }

        public void AddItemToInventory(Item item)
        {
            m_inventory.Add(item);
        }
        
        public bool HasItemInInventory(Item item)
        {
            return m_inventory.Contains(item);
        }
    }
}
