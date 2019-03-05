using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay
{
    public class OrthoNormalBase
    {
        public static OrthoNormalBase FromW(PosVector n)
        {
            PosVector w = n.ToUnitVector();
            PosVector a = Math.Abs(w.X) > 0.9 ? PosVector.UnitY : PosVector.UnitX;
            PosVector v = w.Cross(a).ToUnitVector();
            PosVector u = w.Cross(v);
            return new OrthoNormalBase(u, v, w);
        }

        public PosVector Local(double a, double b, double c)
        {
            return (a * U) + (b * V) + (c * W);
        }

        public PosVector Local(PosVector a)
        {
            return (a.X * U) + (a.Y * V) + (a.Z * W);
        }

        private OrthoNormalBase(PosVector u, PosVector v, PosVector w)
        {
            U = u;
            V = v;
            W = w;
        }

        public PosVector U { get; }
        public PosVector V { get; }
        public PosVector W { get; }
    }
}
