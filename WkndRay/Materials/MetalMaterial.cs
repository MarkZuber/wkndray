// -----------------------------------------------------------------------
// <copyright file="MetalMaterial.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Materials
{
  public class MetalMaterial : AbstractMaterial
  {
    public MetalMaterial(ColorVector albedo, double fuzz)
    {
      Albedo = albedo;
      Fuzz = fuzz;
    }

    public ColorVector Albedo { get; }
    public double Fuzz { get; }

    /// <inheritdoc />
    public override ScatterResult Scatter(Ray rayIn, HitRecord hitRecord)
    {
      var reflected = rayIn.Direction.ToUnitVector().Reflect(hitRecord.Normal);
      var specularRay = new Ray(hitRecord.P, reflected + Fuzz * PosVector.GetRandomInUnitSphere());
      var attenuation = Albedo;
      return new ScatterResult(true, attenuation, specularRay, null);
    }
  }
}