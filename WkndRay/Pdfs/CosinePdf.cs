// -----------------------------------------------------------------------
// <copyright file="CosinePdf.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay.Pdfs
{
  public class CosinePdf : IPdf
  {
    public CosinePdf(PosVector w)
    {
      Uvw = OrthoNormalBase.FromW(w);
    }

    public OrthoNormalBase Uvw { get; }

    public double GetValue(PosVector direction)
    {
      double cosine = direction.ToUnitVector().Dot(Uvw.W);
      return cosine > 0.0 ? cosine / Math.PI : 0.0;  // todo: book has this as 1.0, but that causes NaN due to div by zero
    }

    public PosVector Generate()
    {
      return Uvw.Local(RandomService.GetRandomCosineDirection());
    }
  }
}