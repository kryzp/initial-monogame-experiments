using Microsoft.Xna.Framework;
using System;

namespace DarkWreath
{
    public class Transform
    {
        private Matrix matrix = Matrix.Identity;
        private Vector2 origin = Vector2.Zero;
        private Vector2 position = Vector2.Zero;
        private Vector2 scale = Vector2.One;
        private float rotation = 0f;

        public Action OnTransformed = null;

        public Vector2 Origin
        {
            get => origin;
            set
            {
                if (value == origin)
                    return;

                origin = value;

                if (OnTransformed != null)
                    OnTransformed();
            }
        }

        public Vector2 Position
        {
            get => position;
            set
            {
                if (value == position)
                    return;

                position = value;

                if (OnTransformed != null)
                    OnTransformed();
            }
        }

        public Vector2 PositionWithOrigin => Position - Origin;

        public Vector2 Scale
        {
            get => scale;
            set
            {
                if (value == scale)
                    return;

                scale = value;

                if (OnTransformed != null)
                    OnTransformed();
            }
        }

        public float RotationDeg
        {
            get => MathHelper.ToDegrees(rotation);
            set
            {
                var tr = MathHelper.ToRadians(value);

                if (value == tr)
                    return;

                rotation = tr;

                if (OnTransformed != null)
                    OnTransformed();
            }
        }

        public float RotationRad
        {
            get => rotation;
            set
            {
                if (value == rotation)
                    return;

                rotation = value;

                if (OnTransformed != null)
                    OnTransformed();
            }
        }

        public Matrix Matrix
        {
            get
            {
                Matrix result = Matrix.Identity;

                if (origin.X != 0f || origin.Y != 0f)
                    result = Matrix.CreateTranslation(-origin.X, -origin.Y, 0);

                if (scale.X != 0f || scale.Y != 0f)
                    result *= Matrix.CreateScale(scale.X, scale.Y, 1f);

                if (rotation != 0f)
                    result *= Matrix.CreateRotationZ(rotation);

                if (position.X != 0f || Position.Y != 0f)
                    result *= Matrix.CreateTranslation(position.X, position.Y, 0f);

                return result;
            }
        }

        public Transform()
        {
        }

        public Transform(Transform other)
        {
            this.matrix = other.matrix;
            this.origin = other.origin;
            this.position = other.position;
            this.scale = other.scale;
            this.rotation = other.rotation;
        }

        public void MoveX(float x) => Position += new Vector2(x, 0);
        public void MoveY(float y) => Position += new Vector2(0, y);
    }
}
