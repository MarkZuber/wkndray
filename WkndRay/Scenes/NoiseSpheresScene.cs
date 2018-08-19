using System;
using System.Collections.Generic;
using System.Text;
using WkndRay.Materials;
using WkndRay.Textures;

namespace WkndRay.Scenes
{
  public class NoiseSpheresScene : IScene
  {
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
        20.0,
        Convert.ToDouble(imageWidth) / Convert.ToDouble(imageHeight),
        aperture,
        distToFocus);
    }

    /// <inheritdoc />
    public IHitable GetWorld()
    {
      //var perlinTexture = new NoiseTexture(true, 3.0);
      var list = new HitableList()
      {
        new Sphere(new PosVector(0.0, -1000.0, 0.0), 1000.0, new LambertianMaterial(new VectorNoiseTexture(VectorNoiseMode.Soft, 3.0))),
        new Sphere(new PosVector(0.0, 2.0, 0.0), 2.0, new LambertianMaterial(new VectorNoiseTexture(VectorNoiseMode.Marble, 3.0)))
      };

      return new BvhNode(list, 0.0, 1.0);
    }
  }
}
