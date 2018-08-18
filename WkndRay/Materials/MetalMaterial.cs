// -----------------------------------------------------------------------
// <copyright file="MetalMaterial.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Materials
{
  public class MetalMaterial : IMaterial
  {
    public MetalMaterial(ColorVector albedo, double fuzz)
    {
      Albedo = albedo;
      Fuzz = fuzz;
    }

    public ColorVector Albedo { get; }
    public double Fuzz { get; }

    /// <inheritdoc />
    public ScatterResult Scatter(Ray rayIn, HitRecord hitRecord)
    {
      var reflected = rayIn.Direction.ToUnitVector().Reflect(hitRecord.Normal);
      var scattered = new Ray(hitRecord.P, reflected + Fuzz * PosVector.GetRandomInUnitSphere());
      var attenuation = Albedo;
      bool isScattered = scattered.Direction.Dot(hitRecord.Normal) > 0.0;
      return new ScatterResult(isScattered, attenuation, scattered);
    }
  }
}