using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace montecarlovisualized
{
    public class Belief
    {
        public const float VELOCITY_BOUNDS = 2f;
        public const float VELOCITY_SCALAR_FLUX  = 0.025f;
        public const float VELOCITY_ANGULAR_FLUX = 0.02f;

        private readonly int PARTICLE_COUNT;

        private List<Particle> prevParticles;
        private List<Particle> particles;

        private Random rng;

        public List<Particle> Particles => particles;

        public Belief(int particleCount, int x, int y)
        {
            this.PARTICLE_COUNT = particleCount;

            rng = new Random();

            particles = new List<Particle>();

            for (int i = 0; i < PARTICLE_COUNT; i++)
            {
                particles.Add(new Particle(
                    rng.NextFloat(x, x+64f),
                    rng.NextFloat(y, y+64f),
                    rng.NextFloat(-VELOCITY_BOUNDS, VELOCITY_BOUNDS),
                    rng.NextFloat(-VELOCITY_BOUNDS, VELOCITY_BOUNDS)
                ));
            }

            this.prevParticles = new List<Particle>(this.particles);
        }

        public void Update(int px, int py)
        {
            this.particles = MonteCarlo(px, py);
            this.prevParticles = new List<Particle>(this.particles);
        }

        // weird function but it works by taking the standard deviation of all particles
        // (via their gross values) then depending on the deviation from the mean, the particles are exluded
        // from the final particle, effectively removing any outliers (e.g: exclude if the gross value is 2 standard deviations higher than regular)
        // +1 mr. statistics teacher!
        public Particle GetStandardParticle()
        {
            float grossSum = 0f;
            foreach (Particle p in particles)
                grossSum += p.GrossValue();

            float grossMean = grossSum/(float)PARTICLE_COUNT;

            float grossDiff = 0f;
            foreach (Particle p in particles)
                grossDiff += MathF.Pow(p.GrossValue()-grossMean, 2f);

            float standardDeviation = MathF.Sqrt(grossDiff/(float)PARTICLE_COUNT);

            float finalXTotal = 0f;
            float finalYTotal = 0f;
            float finalVXTotal = 0f;
            float finalVYTotal = 0f;
            int count = 0;

            foreach (Particle p in particles)
            {
                if (Math.Abs(p.GrossValue()) > (standardDeviation*2f))
                    continue;

                finalXTotal += p.X;
                finalYTotal += p.Y;
                finalVXTotal += p.VX;
                finalVYTotal += p.VY;

                count++;
            }

            return new Particle(
                finalXTotal / count,
                finalYTotal / count,
                finalVXTotal / count,
                finalVYTotal / count
            );
        }

        private int emptyTimer = 0;

        private List<Particle> MonteCarlo(int px, int py)
        {
            List<(Particle, float)> processed = new List<(Particle, float)>();

            for (int i = 0; i < PARTICLE_COUNT; i++)
            {
                Particle p = GetParticle(prevParticles[i]);
                float w = GetWeight(p, px, py);
                processed.Add((p, w));
            }

            List<Particle> filtered = new List<Particle>();

            bool empty = true;
            while (filtered.Count < PARTICLE_COUNT)
            {
                for (int i = 0; i < PARTICLE_COUNT; i++)
                {
                    Particle particle = processed[i].Item1;
                    float weight = processed[i].Item2;

                    if (rng.NextDouble() <= weight)
                    {
                        Particle instance = new Particle(particle);
                        {
                            float theta = (float)Math.Atan2(instance.VY, instance.VX);
                            float length = (float)Math.Sqrt(instance.VX * instance.VX + instance.VY * instance.VY);

                            theta += rng.NextFloat(-VELOCITY_ANGULAR_FLUX, VELOCITY_ANGULAR_FLUX);
                            length *= rng.NextFloat(1f - VELOCITY_SCALAR_FLUX, 1f + VELOCITY_SCALAR_FLUX);

                            instance.VX = (float)Math.Cos(theta) * length;
                            instance.VY = (float)Math.Sin(theta) * length;
                        }

                        if (instance.X >= px && instance.X <= px+64 &&
                            instance.Y >= py && instance.Y <= py+64)
                        {
                            empty = false;
                        }

                        filtered.Add(instance);
                    }
                }
            }

            if (empty)
                emptyTimer++;
            else
                emptyTimer = 0;
            
            if (emptyTimer > 22)
            {
                emptyTimer = 0;
                filtered.Clear();

                for (int j = 0; j < PARTICLE_COUNT; j++)
                {
                    Particle instance = new Particle()
                    {
                        X = rng.NextFloat(px, px+64),
                        Y = rng.NextFloat(py, py+64),
                        VX = rng.NextFloat(-VELOCITY_BOUNDS, VELOCITY_BOUNDS),
                        VY = rng.NextFloat(-VELOCITY_BOUNDS, VELOCITY_BOUNDS)
                    };

                    filtered.Add(instance);
                }
            }

            return filtered;
        }

        private Particle GetParticle(Particle prev)
        {
            Particle result = new Particle(prev);
            {
                result.X += result.VX;
                result.Y += result.VY;
            }

            return result;
        }

        private float GetWeight(Particle particle, int x, int y)
        {
            float padding = 8f;

            if (particle.X >= x && particle.X <= x+64 &&
                particle.Y >= y && particle.Y <= y+64)
            {
                return 0.99f;
            }
            else if (particle.X >= x-padding && particle.X <= x+64+padding &&
                     particle.Y >= y-padding && particle.Y <= y+64+padding)
            {
                // differences in position
                float dx = particle.X - x;
                float dy = particle.Y - y;

                // pythagoras' theorem
                float dd = MathF.Sqrt(
                    MathF.Pow(MathF.Max(0f, MathF.Abs(dx)), 2f) +
                    MathF.Pow(MathF.Max(0f, MathF.Abs(dy)), 2f)
                );

                // normalize distance (0 -> 1)
                dd /= padding;

                return dd;
            }

            return 0.01f;
        }
    }
}
