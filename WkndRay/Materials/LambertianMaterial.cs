// -----------------------------------------------------------------------
// <copyright file="LambertianMaterial.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Textures;

namespace WkndRay.Materials
{
  public class LambertianMaterial : AbstractMaterial
  {
    public LambertianMaterial(ITexture albedo)
    {
      Albedo = albedo;
    }

    public ITexture Albedo { get; }

    /// <inheritdoc />
    public override ScatterResult Scatter(Ray rayIn, HitRecord hitRecord)
    {
      var target = hitRecord.P + hitRecord.Normal + PosVector.GetRandomInUnitSphere();
      var scatteredRay = new Ray(hitRecord.P, target - hitRecord.P);
      var attenuation = Albedo.GetValue(hitRecord.UvCoords, hitRecord.P);
      return new ScatterResult(true, attenuation, scatteredRay);
    }
  }
}