namespace montecarlovisualized
{
    public struct Particle
    {
        public float X;
        public float Y;
        public float VX;
        public float VY;

        public Particle(float x, float y, float vx, float vy)
        {
            this.X = x;
            this.Y = y;
            this.VX = vx;
            this.VY = vy;
        }

        public Particle(Particle other)
        {
            this.X = other.X;
            this.Y = other.Y;
            this.VX = other.VX;
            this.VY = other.VY;
        }

        // describes the particle as a single value, used for ranking the particles and finding their standard deviation.
        public float GrossValue()
        {
            return X*Y + VX*VY;
            //return X * Y * VX * VY;
        }

        public override string ToString()
        {
            return $"POSITION = ({X}, {Y})\nVELOCITY = ({VX}, {VY})";
        }
    }
}
