// -----------------------------------------------------------------------
// <copyright file="IExecutor.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Executors
{
  public interface IExecutor
  {
    PixelBuffer Execute(int width, int height);
  }
}