using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections;
using System.Collections.Generic;
using Spellpath.Areas;

namespace Spellpath.Actors
{
    public class Actor
    {
        protected List<Coroutine> coroutines = new List<Coroutine>();

        public Transform Transform;
        public Collider Collider;
        public AnimatedSprite Sprite;

        public Vector2 ExtraTextureScale = Vector2.Zero;

        public GameArea CurrentArea = null;

        public Actor()
        {
            Transform = new Transform();
            Sprite = new AnimatedSprite();
            SetCollider(new Collider());
        }

        public virtual void Update(GameTime time)
        {
            UpdateCoroutines(time);
        }

        public virtual void Draw(GameTime time, SpriteBatch b)
        {
            Sprite?.Draw(
                b,
                Transform.Position,
                Transform.Origin,
                Transform.Position.Y / 10000f,
                0, 0,
                Color.Red,
                Transform.Scale * ExtraTextureScale,
                Transform.RotationRad,
                false
            );
        }

        public virtual void DrawUI(GameTime time, SpriteBatch b)
        {
        }

        public virtual void Move(Vector2 amount)
        {
            if (Collider != null && CurrentArea != null)
            {
                float maxStep = System.MathF.Min(
                    Collider.GetWorldBounds().Width,
                    Collider.GetWorldBounds().Height
                ) / 4f;

                float distance = amount.Length();
                Vector2 normal = amount.NormalizedCopy();

                while (distance > 0)
                {
                    float stepDist = System.MathF.Min(distance, maxStep);
                    Vector2 step = normal * stepDist;
                    distance -= System.MathF.Abs(stepDist);

                    Transform.Position += step;
                    CurrentArea.ResolveCollidingPosition(Collider, this, step);
                }
            }
            else
            {
                Transform.Position += amount;
            }
        }

        protected void SetCollider(Collider collider)
        {
            Collider = collider;
            Collider.Actor = this;
        }

        public Coroutine StartCoroutine(IEnumerator routine)
		{
            var cr = new Coroutine(routine);
            coroutines.Add(cr);
            return cr;
		}

        private void UpdateCoroutines(GameTime time)
		{
            coroutines.RemoveAll(c => c.IsFinished);
            foreach (var coroutine in coroutines)
                coroutine.Update(time);
		}
    }
}
