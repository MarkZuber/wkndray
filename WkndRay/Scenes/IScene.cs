// -----------------------------------------------------------------------
// <copyright file="IScene.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Scenes
{
  public interface IScene
  {
    Camera GetCamera(int imageWidth, int imageHeight);
    IHitable GetWorld();
  }
}