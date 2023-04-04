using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using DarkWreath.Areas;

namespace DarkWreath.Actors
{
    public abstract class Actor
    {
        private int transformStamp;
        private Collider collider;
        
        protected List<Coroutine> pCoroutines = new List<Coroutine>();

        public int TransformStamp => transformStamp;
        
        public Collider Collider
        {
            get
            {
                return collider;
            }

            set
            {
                collider = value;
            
                if (value != null)
                    value.Actor = this;
            }
        }
        
        public Transform Transform;
        public AnimatedSprite Sprite;
        public GameArea CurrentArea = null;

        public Actor()
        {
            Transform = new Transform();
            Transform.OnTransformed = () => transformStamp++;
            
            Sprite = new AnimatedSprite();
            Collider = null;
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateCoroutines(gameTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch b)
        {
            Sprite?.Draw(
                b,
                Transform.Position,
                Transform.Origin,
                Transform.Position.Y / 10000f,
                0, 0,
                Color.White,
                Transform.Scale,
                Transform.RotationRad,
                false
            );
        }

        public virtual void DrawUI(GameTime gameTime, SpriteBatch b)
        {
        }

        public void Destroy()
        {
            CurrentArea?.RemoveActor(this);
        }
        
        public virtual void Move(Vector2 amount, float precision = 6f)
        {
            if (Collider != null && CurrentArea != null)
            {
                float distance = amount.Length();
                Vector2 normal = amount.Normalized();

                float maxStep = distance / precision;
                
                /*
                float maxStep = MathF.Min(
                    Collider.WorldBounds.Width,
                    Collider.WorldBounds.Height
                ) / 4f;
                */
                
                var hits1 = GetCollidingColliders(Collider);
                foreach (Hit hit in hits1)
                    OnCollisionHit(hit);
                    
                CurrentArea.ResolveCollidingPosition(Collider);

                while (distance > 0f)
                {
                    float stepDist = MathF.Min(distance, maxStep);
                    Vector2 step = normal * stepDist;
                    distance -= MathF.Abs(stepDist);
                    
                    Transform.Position += step;
                    
                    var hits = GetCollidingColliders(Collider);
                    foreach (Hit hit in hits)
                        OnCollisionHit(hit);
                    
                    CurrentArea.ResolveCollidingPosition(Collider);
                }
            }
            else
            {
                Transform.Position += amount;
            }
        }

        public Coroutine StartCoroutine(IEnumerator routine)
		{
            var cr = new Coroutine(routine);
            pCoroutines.Add(cr);
            return cr;
		}

        private void UpdateCoroutines(GameTime gameTime)
		{
            pCoroutines.RemoveAll(c => c.IsFinished);
            foreach (var coroutine in pCoroutines)
                coroutine.Update(gameTime);
		}
        
        public virtual List<Hit> GetCollidingColliders(Collider col)
        {
            if (CurrentArea == null)
                return null;
            
            return CurrentArea.GetCollidingColliders(col);
        }

        public virtual void OnCollisionHit(Hit hit)
        {
        }
    }
}
