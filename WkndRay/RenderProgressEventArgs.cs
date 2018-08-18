using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay
{
  public class RenderProgressEventArgs : EventArgs
  {
    public RenderProgressEventArgs(double percentComplete)
    {
      PercentComplete = percentComplete;
    }

    public double PercentComplete { get; }
  }
}
