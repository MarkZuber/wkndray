using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Materials
{
  public class MetalMaterial : IMaterial
  {
    private readonly IRandomService _randomService;

    public MetalMaterial(IRandomService randomService, ColorVector albedo, double fuzz)
    {
      _randomService = randomService;
      Albedo = albedo;
      Fuzz = fuzz;
    }

    public ColorVector Albedo { get; }
    public double Fuzz { get; }

    /// <inheritdoc />
    public ScatterResult Scatter(Ray rayIn, HitRecord hitRecord)
    {
      var reflected = rayIn.Direction.ToUnitVector().Reflect(hitRecord.Normal);
      var scattered = new Ray(hitRecord.P, reflected + Fuzz * PosVector.GetRandomInUnitSphere(_randomService));
      var attenuation = Albedo;
      bool isScattered = scattered.Direction.Dot(hitRecord.Normal) > 0.0;
      return new ScatterResult(isScattered, attenuation, scattered);
    }
  }
}
