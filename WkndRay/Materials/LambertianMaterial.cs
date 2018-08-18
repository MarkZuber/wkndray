using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Materials
{
  public class LambertianMaterial : IMaterial
  {
    private readonly IRandomService _randomService;

    public LambertianMaterial(IRandomService randomService, ColorVector albedo)
    {
      _randomService = randomService;
      Albedo = albedo;
    }

    public ColorVector Albedo { get; }

    /// <inheritdoc />
    public ScatterResult Scatter(Ray rayIn, HitRecord hitRecord)
    {
      var target = hitRecord.P + hitRecord.Normal + PosVector.GetRandomInUnitSphere(_randomService);
      var scatteredRay = new Ray(hitRecord.P, target - hitRecord.P);
      var attenuation = Albedo;
      return new ScatterResult(true, attenuation, scatteredRay);
    }
  }
}
