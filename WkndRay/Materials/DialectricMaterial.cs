using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Materials
{
  public class DialectricMaterial : IMaterial
  {
    private readonly IRandomService _randomService;
    public DialectricMaterial(IRandomService randomService, double refractionIndex)
    {
      _randomService = randomService;
      RefractionIndex = refractionIndex;
    }

    public double RefractionIndex { get; }

    /// <inheritdoc />
    public ScatterResult Scatter(Ray rayIn, HitRecord hitRecord)
    {
      var reflected = rayIn.Direction.Reflect(hitRecord.Normal);
      var attenuation = new ColorVector(1.0, 1.0, 1.0);
      double niOverNt;
      PosVector outwardNormal;
      double cosine;
      if (rayIn.Direction.Dot(hitRecord.Normal) > 0.0)
      {
        outwardNormal = -hitRecord.Normal;
        niOverNt = RefractionIndex;
        cosine = RefractionIndex * rayIn.Direction.Dot(hitRecord.Normal) / rayIn.Direction.Magnitude();
      }
      else
      {
        outwardNormal = hitRecord.Normal;
        niOverNt = 1.0 / RefractionIndex;
        cosine = -rayIn.Direction.Dot(hitRecord.Normal) / rayIn.Direction.Magnitude();
      }

      double reflectProbability;
      Ray scattered;
      var refracted = rayIn.Direction.Refract(outwardNormal, niOverNt);
      if (refracted != null)
      {
        reflectProbability = CalculateSchlickApproximation(cosine, RefractionIndex);
      }
      else
      {
        scattered = new Ray(hitRecord.P, reflected);
        reflectProbability = 1.0;
      }

      if (_randomService.NextDouble() < reflectProbability)
      {
        scattered = new Ray(hitRecord.P, reflected);
      }
      else
      {
        scattered = new Ray(hitRecord.P, refracted);
      }

      return new ScatterResult(true, attenuation, scattered);
    }

    private double CalculateSchlickApproximation(double cosine, double refractionIndex)
    {
      double r0 = (1.0 - refractionIndex) / (1.0 + refractionIndex);
      r0 = r0 * r0;
      return r0 + (1.0 - r0) * Math.Pow(1.0 - cosine, 5.0);
    }
  }
}
