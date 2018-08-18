// -----------------------------------------------------------------------
// <copyright file="RenderProgressEventArgs.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

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