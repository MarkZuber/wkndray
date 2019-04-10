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

        public float GetValue(PosVector direction)
        {
            float cosine = direction.ToUnitVector().Dot(Uvw.W);
            return cosine > 0.0f ? cosine / MathF.PI : 1.0f;  // todo: book has this as 1.0f, but that causes NaN due to div by zero
        }

        public PosVector Generate()
        {
            return Uvw.Local(RandomService.GetRandomCosineDirection());
        }
    }
}
