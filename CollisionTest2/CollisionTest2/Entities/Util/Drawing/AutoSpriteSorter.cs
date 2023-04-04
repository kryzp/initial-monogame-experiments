using System;
using System.Collections.Generic;
using System.Linq;
using CollisionTest2.Entities.Sprites;
using Microsoft.Xna.Framework;

namespace CollisionTest2.Entities.Util.Drawing
{
    public class AutoSpriteSorter
    {
        private bool continuous = true;
        
        private List<Entity> entities;
        
        public AutoSpriteSorter(List<Entity> entities)
        {
            this.entities = entities;
            SortBasedOnY();
        }

        public void Update(float deltaTime)
        {
            if(continuous)
                SortBasedOnY();
        }

        private void SortBasedOnY()
        {
            foreach(Sprite sprite in entities)
            {
                var bottom = sprite.Rectangle.Bottom + sprite.YSortOffset;
                sprite.Layer = (int)(bottom);
            }
        }
    }
}