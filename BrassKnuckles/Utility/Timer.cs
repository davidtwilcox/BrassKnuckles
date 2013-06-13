using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BrassKnuckles.Utility
{
    public abstract class Timer
    {
        #region Private fields

        private float _accumulatedDelta;

        #endregion

        #region Properties

        public float Delay { get; set; }

        public bool Repeat { get; private set; }

        public bool IsDone { get; private set; }

        public bool IsRunning
        {
            get { return (!IsDone && (_accumulatedDelta < Delay) && !IsStopped); }
        }

        public bool IsStopped { get; private set; }

        public float PercentRemaining
        {
            get
            {
                if (IsDone)
                {
                    return 1.0f;
                }
                else if (IsStopped)
                {
                    return 0.0f;
                }
                else
                {
                    return 1.0f - ((Delay - _accumulatedDelta) / Delay);
                }
            }
        }

        #endregion

        #region Constructors

        public Timer(float delay, bool repeat)
        {
            Delay = delay;
            Repeat = repeat; 
        }

        #endregion

        #region Public methods

        public abstract void Execute();

        public void Update(GameTime gameTime)
        {
            if (!IsDone && !IsStopped)
            {
                _accumulatedDelta += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_accumulatedDelta >= Delay)
                {
                    _accumulatedDelta -= Delay;

                    if (Repeat)
                    {
                        Reset();
                    }
                    else
                    {
                        IsDone = true;
                    }

                    Execute();
                }
            }
        }

        public void Reset()
        {
            IsStopped = false;
            IsDone = false;
            _accumulatedDelta = 0.0f;
        }

        public void Stop()
        {
            IsStopped = true;
        }

        #endregion
    }
}
