﻿// -----------------------------------------------------------------------
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
    /// <inheritdoc />
    public Camera GetCamera(int imageWidth, int imageHeight)
    {
      var lookFrom = new PosVector(278.0, 278.0, -800.0);
      var lookAt = new PosVector(278.0, 278.0, 0.0);
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
      var red = new LambertianMaterial(new ColorTexture(0.65, 0.05, 0.05));
      var white = new LambertianMaterial(new ColorTexture(0.73, 0.73, 0.73));
      var green = new LambertianMaterial(new ColorTexture(0.12, 0.45, 0.15));
      var light = new DiffuseLight(new ColorTexture(15.0, 15.0, 15.0));

      var list = new HitableList()
      {
        new FlipNormals(new YzRect(0.0, 555.0, 0.0, 555.0, 555.0, green)),
        new YzRect(0.0, 555.0, 0.0, 555.0, 0.0, red),
        new XzRect(213.0, 343.0, 227.0, 332.0, 554.0, light),
        new FlipNormals(new XzRect(0.0, 555.0, 0.0, 555.0, 555.0, white)),
        new XzRect(0.0, 555.0, 0.0, 555.0, 0.0, white),
        new FlipNormals(new XyRect(0.0, 555.0, 0.0, 555.0, 555.0, white)),

        new Translate(new RotateY(new Box(new PosVector(0.0, 0.0, 0.0), new PosVector(165.0, 165.0, 165.0), white), -18.0), new PosVector(130.0, 0.0, 65.0)), 
        new Translate(new RotateY(new Box(new PosVector(0.0, 0.0, 0.0), new PosVector(165.0, 330.0, 165.0), white), 15.0), new PosVector(265.0, 0.0, 295.0))
      };

      return new BvhNode(list, 0.0, 1.0);
    }

    /// <inheritdoc />
    public Func<Ray, ColorVector> GetBackgroundFunc()
    {
      return ray => ColorVector.One * 0.1;
    }
  }
}