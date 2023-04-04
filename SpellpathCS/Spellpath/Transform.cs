using Microsoft.Xna.Framework;
using System;

namespace Spellpath
{
    public class Transform
    {
        private Matrix m_matrix = Matrix.Identity;
        private Vector2 m_origin = Vector2.Zero;
        private Vector2 m_position = Vector2.Zero;
        private Vector2 m_scale = Vector2.One;
        private float m_rotation = 0f;

        public Action OnTransformed = null;

        public Vector2 Origin
        {
            get => m_origin;
            set
            {
                if (value == m_origin)
                    return;

                m_origin = value;

                if (OnTransformed != null)
                    OnTransformed();
            }
        }

        public Vector2 Position
        {
            get => m_position;
            set
            {
                if (value == m_position)
                    return;

                m_position = value;

                if (OnTransformed != null)
                    OnTransformed();
            }
        }

        public Vector2 PositionWithOrigin => Position - Origin;

        public Vector2 Scale
        {
            get => m_scale;
            set
            {
                if (value == m_scale)
                    return;

                m_scale = value;

                if (OnTransformed != null)
                    OnTransformed();
            }
        }

        public float RotationDeg
        {
            get => MathHelper.ToDegrees(m_rotation);
            set
            {
                var tr = MathHelper.ToRadians(value);

                if (value == tr)
                    return;

                m_rotation = tr;

                if (OnTransformed != null)
                    OnTransformed();
            }
        }

        public float RotationRad
        {
            get => m_rotation;
            set
            {
                if (value == m_rotation)
                    return;

                m_rotation = value;

                if (OnTransformed != null)
                    OnTransformed();
            }
        }

        public Transform()
        {
        }

        public Transform(Transform other)
        {
            this.m_matrix = other.m_matrix;
            this.m_origin = other.m_origin;
            this.m_position = other.m_position;
            this.m_scale = other.m_scale;
            this.m_rotation = other.m_rotation;
        }

        public void MoveX(float x) => Position += new Vector2(x, 0);
        public void MoveY(float y) => Position += new Vector2(0, y);

        public Matrix GetMatrix()
        {
            Matrix result = Matrix.Identity;

            if (m_origin.X != 0f || m_origin.Y != 0f)
                result = Matrix.CreateTranslation(-m_origin.X, -m_origin.Y, 0);

            if (m_scale.X != 0f || m_scale.Y != 0f)
                result *= Matrix.CreateScale(m_scale.X, m_scale.Y, 1f);

            if (m_rotation != 0f)
                result *= Matrix.CreateRotationZ(m_rotation);

            if (m_position.X != 0f || Position.Y != 0f)
                result *= Matrix.CreateTranslation(m_position.X, m_position.Y, 0f);

            return result;
        }
    }
}
