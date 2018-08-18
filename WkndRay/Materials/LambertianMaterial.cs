// -----------------------------------------------------------------------
// <copyright file="LambertianMaterial.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Materials
{
  public class LambertianMaterial : IMaterial
  {
    public LambertianMaterial(ColorVector albedo)
    {
      Albedo = albedo;
    }

    public ColorVector Albedo { get; }

    /// <inheritdoc />
    public ScatterResult Scatter(Ray rayIn, HitRecord hitRecord)
    {
      var target = hitRecord.P + hitRecord.Normal + PosVector.GetRandomInUnitSphere();
      var scatteredRay = new Ray(hitRecord.P, target - hitRecord.P);
      var attenuation = Albedo;
      return new ScatterResult(true, attenuation, scatteredRay);
    }
  }
}