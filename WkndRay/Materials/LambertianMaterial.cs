// -----------------------------------------------------------------------
// <copyright file="LambertianMaterial.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Pdfs;
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
      //var uvw = OrthoNormalBase.FromW(hitRecord.Normal);
      //PosVector direction = uvw.Local(RandomService.GetRandomCosineDirection());

      //var scatteredRay = new Ray(hitRecord.P, direction.ToUnitVector());
      var attenuation = Albedo.GetValue(hitRecord.UvCoords, hitRecord.P);
      //var pdf = uvw.W.Dot(scatteredRay.Direction) / Math.PI;
      return new ScatterResult(true, attenuation, null, new CosinePdf(hitRecord.Normal));
    }

    public override double ScatteringPdf(Ray rayIn, HitRecord hitRecord, Ray scattered)
    {
      double cosine = hitRecord.Normal.Dot(scattered.Direction.ToUnitVector());
      if (cosine < 0.0)
      {
        cosine = 0.0;
      }

      return cosine / Math.PI;
    }
  }
}