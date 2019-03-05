// -----------------------------------------------------------------------
// <copyright file="ImageTextureScene.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Hitables;
using WkndRay.Materials;
using WkndRay.Textures;

namespace WkndRay.Scenes
{
    public class LightsScene : IScene
    {
        private readonly string _globeImagePath;

        public LightsScene(string globeImagePath)
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
          new LambertianMaterial(new VectorNoiseTexture(VectorNoiseMode.Marble, 3.0))),
        new Sphere(
          new PosVector(0.0, 2.0, 0.0),
          2.0,
          new LambertianMaterial(new ImageTexture(globe))),
        new Sphere(new PosVector(0.0, 7.0, 0.0), 2.0, new DiffuseLight(new ColorTexture(4.0, 4.0, 4.0))),
        new XyRect(3.0, 5.0, 1.0, 3.0, -2.0, new DiffuseLight(new ColorTexture(4.0, 4.0, 4.0)))
      };

            return new BvhNode(list, 0.0, 1.0);
        }

        /// <inheritdoc />
        public IHitable GetLightHitable()
        {
            return new HitableList();
        }

        /// <inheritdoc />
        public Func<Ray, ColorVector> GetBackgroundFunc()
        {
            return ray => ColorVector.One * 0.05;
        }
    }
}