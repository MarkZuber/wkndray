using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay
{
    public class PixelData
    {
        public PixelData(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
        public ColorVector Color { get; set; }

        public long PixelColorMilliseconds { get; private set; }
        public void SetPixelColorMilliseconds(long ms) => PixelColorMilliseconds = ms;

        public long AverageSampleMilliseconds { get; set; }

        public int MaxDepthReached { get; private set; } = 0;

        internal void SetDepth(int depth)
        {
            if (depth > MaxDepthReached)
            {
                MaxDepthReached = depth;
            }
        }
    }
}
