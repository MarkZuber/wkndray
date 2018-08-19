// -----------------------------------------------------------------------
// <copyright file="ImageTextureScene.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;
using WkndRay.Textures;

namespace WkndRay.Scenes
{
  public class ImageTextureScene : IScene
  {
    private readonly string _globeImagePath;

    public ImageTextureScene(string globeImagePath)
    {
      _globeImagePath = globeImagePath;
    }

    /// <inheritdoc />
    public Camera GetCamera(int imageWidth, int imageHeight)
    {
      var lookFrom = new PosVector(13.0, 2.0, 3.0);
      var lookAt = PosVector.Zero;
      double distToFocus = 10.0;
      double aperture = 0.0;
      return new Camera(
        lookFrom,
        lookAt,
        PosVector.UnitY,
        40.0,
        Convert.ToDouble(imageWidth) / Convert.ToDouble(imageHeight),
        aperture,
        distToFocus);
    }

    /// <inheritdoc />
    public IHitable GetWorld()
    {
      var globe = PixelBuffer.FromFile(_globeImagePath);
      var list = new HitableList()
      {
        new Sphere(
          new PosVector(0.0, -1000.0, 0.0),
          1000.0,
          new LambertianMaterial(new VectorNoiseTexture(VectorNoiseMode.Soft, 3.0))),
        new Sphere(
          new PosVector(0.0, 2.0, 0.0),
          2.0,
          new LambertianMaterial(new ImageTexture(globe)))
      };

      return new BvhNode(list, 0.0, 1.0);
    }
  }
}