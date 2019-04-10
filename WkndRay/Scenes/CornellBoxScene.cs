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
    public class CornellBoxScene : IScene
    {
        private readonly IHitable _light;
        private readonly IHitable _glassSphere;

        public CornellBoxScene()
        {
            var light = new DiffuseLight(new ColorTexture(15.0f, 15.0f, 15.0f));
            var glass = new DialectricMaterial(1.5f);

            _light = new XzRect(213.0f, 343.0f, 227.0f, 332.0f, 554.0f, light);
            _glassSphere = new Sphere(new PosVector(190.0f, 90.0f, 190.0f), 90.0f, glass);
        }

        /// <inheritdoc />
        public Camera GetCamera(int imageWidth, int imageHeight)
        {
            var lookFrom = new PosVector(278.0f, 278.0f, -800.0f);
            var lookAt = new PosVector(278.0f, 278.0f, 0.0f);
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
            var red = new LambertianMaterial(new ColorTexture(0.65f, 0.05f, 0.05f));
            var white = new LambertianMaterial(new ColorTexture(0.73f, 0.73f, 0.73f));
            var green = new LambertianMaterial(new ColorTexture(0.12f, 0.45f, 0.15f));
            var aluminum = new MetalMaterial(new ColorVector(0.8f, 0.85f, 0.88f), 0.0f);

            var list = new HitableList()
            {
                new FlipNormals(new YzRect(0.0f, 555.0f, 0.0f, 555.0f, 555.0f, green)),
                new YzRect(0.0f, 555.0f, 0.0f, 555.0f, 0.0f, red),
                new FlipNormals(_light),
                new FlipNormals(new XzRect(0.0f, 555.0f, 0.0f, 555.0f, 555.0f, white)),
                new XzRect(0.0f, 555.0f, 0.0f, 555.0f, 0.0f, white),
                new FlipNormals(new XyRect(0.0f, 555.0f, 0.0f, 555.0f, 555.0f, white)),

                // new Translate(new RotateY(new Box(new PosVector(0.0f, 0.0f, 0.0f), new PosVector(165.0f, 165.0f, 165.0f), white), -18.0f), new PosVector(130.0f, 0.0f, 65.0f)), 
                new Translate(new RotateY(new Box(new PosVector(0.0f, 0.0f, 0.0f), new PosVector(165.0f, 330.0f, 165.0f), white), 15.0f), new PosVector(265.0f, 0.0f, 295.0f)),
                _glassSphere,
            };

            return new BvhNode(list, 0.0f, 1.0f);
        }

        /// <inheritdoc />
        public IHitable GetLightHitable()
        {
            return new HitableList
      {
        _light,
        _glassSphere
      };
        }

        /// <inheritdoc />
        public Func<Ray, ColorVector> GetBackgroundFunc()
        {
            return ray => new ColorVector(0.12f, 0.34f, 0.56f); // ColorVector.1; // * 0.1;
        }
    }
}
