using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Scenes
{
  public interface IScene
  {
    Camera GetCamera(int imageWidth, int imageHeight);
    IHitable GetWorld();
  }
}
