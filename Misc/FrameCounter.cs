using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class FrameCounter
    {
        public const int MaxSamples = 100;
        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }

        private Queue<float> _frameBuffer;

        public FrameCounter()
        {
            _frameBuffer = new Queue<float>(); 
        }

        public bool Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            _frameBuffer.Enqueue(CurrentFramesPerSecond);

            if (_frameBuffer.Count > MaxSamples)
            {
                _frameBuffer.Dequeue();
                AverageFramesPerSecond = _frameBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
            return true;
        }
    }
}
