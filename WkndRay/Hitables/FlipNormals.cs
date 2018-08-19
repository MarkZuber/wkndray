// -----------------------------------------------------------------------
// <copyright file="FlipNormals.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Hitables
{
  public class FlipNormals : IHitable
  {
    public FlipNormals(IHitable hitable)
    {
      Hitable = hitable;
    }

    public IHitable Hitable { get; }

    /// <inheritdoc />
    public HitRecord Hit(Ray ray, double tMin, double tMax)
    {
      var hitrec = Hitable.Hit(ray, tMin, tMax);
      if (hitrec != null)
      {
        // invert the normal...
        hitrec = new HitRecord(hitrec.T, hitrec.P, -hitrec.Normal, hitrec.UvCoords, hitrec.Material);
        return hitrec;
      }
      else
      {
        return null;
      }
    }

    /// <inheritdoc />
    public AABB GetBoundingBox(double t0, double t1)
    {
      return Hitable.GetBoundingBox(t0, t1);
    }
  }
}