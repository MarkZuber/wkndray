using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay
{
  public class RenderConfig
  {
    public RenderConfig(int numThreads, int rayTraceDepth, int numSamples)
    {
      NumThreads = numThreads;
      RayTraceDepth = rayTraceDepth;
      NumSamples = numSamples;
    }

    public int NumThreads { get; }
    public int RayTraceDepth { get; }
    public int NumSamples { get; }
  }
}