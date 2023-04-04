using Microsoft.Xna.Framework;
using System.Collections;

namespace DarkWreath
{
    public abstract class WaitYieldInstruction
    {
        protected float timeRemaining;
        public bool IsFinished => timeRemaining <= 0f;
        public abstract void Update(GameTime time);
    }

    public class WaitForMilliseconds : WaitYieldInstruction
    {
        public WaitForMilliseconds(float ms) => timeRemaining = ms;
        public override void Update(GameTime time) => timeRemaining -= (float)time.ElapsedGameTime.TotalMilliseconds;
    }

    public class WaitForSeconds : WaitYieldInstruction
    {
        public WaitForSeconds(float sec) => timeRemaining = sec;
        public override void Update(GameTime time) => timeRemaining -= (float)time.ElapsedGameTime.TotalSeconds;
    }

    public class Coroutine
    {
        private IEnumerator routine;
        private WaitYieldInstruction wait;
        
        public bool IsFinished { get; private set; }
        public void Stop() => IsFinished = true;

        public Coroutine(IEnumerator routine)
        {
            this.routine = routine;
        }

        public void Update(GameTime time)
        {
            if (IsFinished)
                return;
            
            if (wait != null)
            {
                wait.Update(time);

                if (!wait.IsFinished)
                    return;
                
                wait = null;
            }

            if (!routine.MoveNext())
                IsFinished = true;
            
            wait = (WaitYieldInstruction)routine.Current;
        }
    }
}
