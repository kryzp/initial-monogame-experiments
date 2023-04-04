using System;
using CollisionTest2;
using CollisionTest2.Entities.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ARPG.Meta
{
    public class Camera
    {
	    private Vector2 targetPosition { get; set; }
	    
	    private Viewport viewport;

	    private bool targetOverride = false;
	    
		private Vector2 origin { get; set; }
	    private Vector2 position { get; set; }
	    private float scale { get; set; }
	    private float rotation { get; set; }
	    
		public Vector2 Origin
		{
			get => origin;
			set => origin = value;
		}
		
	    public Vector2 Position
	    {
		    get => position;
		    set
		    {
			    position = value;
			    targetPosition = value;
		    }
	    }

	    public float Scale
	    {
		    get => scale;
		    set => scale = value;
	    }

	    public float Rotation
	    {
		    get => rotation;
		    set => rotation = value;
	    }

	    public Matrix Transform { get; set; }

        public Vector2 TargetOverride
        {
	        get => targetPosition;
	        set
	        {
		        targetOverride = true;
		        targetPosition = value;
	        }
        }

        public Sprite TargetSprite { get; set; }

        public Camera(Viewport view)
        {
	        viewport = view;

	        targetPosition = Position;
	        
	        Origin = new Vector2(Game1.ScreenWidth / 2f, Game1.ScreenHeight / 2f);
	        Scale = 4f;
        }

        public void Update(float deltaTime)
        {
	        /*
	         * Target Override means we can have a cutscene where I se the target to something like a boss and then
	         * when finished lerping to the boss it gets set back to the player or whatever the target sprite is
	         *
	         * In the future I plan to make this more robust when making cutscenes.
	         */
	        
	        if(targetOverride)
	        {
		        var pos = Position;
		        pos.X = MathHelper.Lerp(pos.X, targetPosition.X, 0.15f);
		        pos.Y = MathHelper.Lerp(pos.Y, targetPosition.Y, 0.15f);
		        position = pos;
	        }
	        else if(TargetSprite != null)
	        {
		        var pos = Position;
		        pos.X = MathHelper.Lerp(pos.X, TargetSprite.Position.X, 0.15f);
		        pos.Y = MathHelper.Lerp(pos.Y, TargetSprite.Position.Y, 0.15f);
		        position = pos;
	        }

	        if(targetPosition == Position)
	        {
		        targetOverride = false;
	        }

	        Transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
	                    Matrix.CreateRotationZ(Rotation) *
	                    Matrix.CreateScale(new Vector3(Scale, Scale, 0)) *
	                    Matrix.CreateTranslation(Origin.X, Origin.Y, 0);
        }

        public void Reset()
        {
	        TargetSprite = null;
	        targetOverride = false;
	        Origin = new Vector2(Game1.WorldWidth / 2f, Game1.WorldWidth / 2f);
	        Scale = 4f;
	        Rotation = 0f;
        }
    }
}