using DarkWreath.Actors;
using DarkWreath.Input;
using DarkWreath.MousePattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Items.Weapons.Ranged.Reloading
{
	public abstract class ReloadPattern : Pattern
	{
		private bool justFinishedReloading;
		
		public RangedWeapon RangedWeapon;
		public bool Reloading;
		
		public ReloadPattern()
			: base()
		{
			justFinishedReloading = false;
			Reloading = false;
		}

		public virtual void Update(GameTime gameTime, Player player)
		{
			var inp = Game1.PlayerInput.GetState();
			
			if (inp.DoReload && !justFinishedReloading)
			{
				if (inp.StartReload)
				{
					pCurrentSegmentStartingPosition =
						pFirstSegmentStartingPosition =
							InputManager.MouseScreenPosition();
					
					CurrentSegment.StartPosition = pCurrentSegmentStartingPosition;
					
					Started();
				}

				Reloading = true;
			}
			else if (inp.QuitReload)
			{
				Reloading = false;
				justFinishedReloading = false;
				pCurrentSegmentIndex = 0;
				
				Finished();
			}

			if (Reloading)
			{
				if (CurrentSegment.Update())
				{
					pCurrentSegmentStartingPosition += CurrentSegment.GetEndPosition();
					pCurrentSegmentIndex++;

					if (pCurrentSegmentIndex >= Segments.Count)
					{
						Reloading = false;
						justFinishedReloading = true;
						
						pCurrentSegmentIndex = 0;

						Finished();
						RangedWeapon.Reload(player);
					}
					else
					{
						CurrentSegment.StartPosition = pCurrentSegmentStartingPosition;
					}
				}
			}
		}

		public virtual void Draw(SpriteBatch b, Player player)
		{
			if (DebugConsole.DBG_DrawDebugView)
				CurrentSegment.DebugDraw(b);
		}

		public virtual void Started()
		{
		}

		public virtual void Finished()
		{
			foreach (var segment in Segments)
				segment.Reset();
		}
	}
}
