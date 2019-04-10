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
            var lookFrom = new PosVector(13.0f, 2.0f, 3.0f);
            var lookAt = PosVector.Zero;
            float distToFocus = 10.0f;
            float aperture = 0.0f;
            return new Camera(
              lookFrom,
              lookAt,
              PosVector.UnitY,
              40.0f,
              Convert.ToSingle(imageWidth) / Convert.ToSingle(imageHeight),
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
          new PosVector(0.0f, -1000.0f, 0.0f),
          1000.0f,
          new LambertianMaterial(new VectorNoiseTexture(VectorNoiseMode.Soft, 3.0f))),
        new Sphere(
          new PosVector(0.0f, 2.0f, 0.0f),
          2.0f,
          new LambertianMaterial(new ImageTexture(globe)))
      };

            return new BvhNode(list, 0.0f, 1.0f);
        }

        /// <inheritdoc />
        public IHitable GetLightHitable()
        {
            return new HitableList();
        }

        /// <inheritdoc />
        public Func<Ray, ColorVector> GetBackgroundFunc()
        {
            return ray =>
            {
                var unitDirection = ray.Direction.ToUnitVector();
                float t = 0.5f * (unitDirection.Y + 1.0f);
                return ((1.0f - t) * ColorVector.One) + (t * new ColorVector(0.5f, 0.7f, 1.0f));
            };
        }
    }
}
