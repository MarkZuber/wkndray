// -----------------------------------------------------------------------
// <copyright file="LambertianMaterial.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using WkndRay.Textures;

namespace WkndRay.Materials
{
  public class LambertianMaterial : IMaterial
  {
    public LambertianMaterial(ITexture albedo)
    {
      Albedo = albedo;
    }

    public ITexture Albedo { get; }

    /// <inheritdoc />
    public ScatterResult Scatter(Ray rayIn, HitRecord hitRecord)
    {
      var target = hitRecord.P + hitRecord.Normal + PosVector.GetRandomInUnitSphere();
      var scatteredRay = new Ray(hitRecord.P, target - hitRecord.P);
      var attenuation = Albedo.GetValue(0.0, 0.0, hitRecord.P);
      return new ScatterResult(true, attenuation, scatteredRay);
    }
  }
}