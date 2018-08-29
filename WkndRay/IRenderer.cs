using System;
using WkndRay.Scenes;

namespace WkndRay
{
  public interface IRenderer
  {
    event EventHandler<RenderProgressEventArgs> Progress;
    void Render(IPixelBuffer pixelArray, IScene scene, RenderConfig renderConfig);
    void Render(IPixelBuffer pixelArray, Camera camera, IHitable world, IHitable lightHitable, RenderConfig renderConfig, Func<Ray, ColorVector> backgroundFunc);
  }
}