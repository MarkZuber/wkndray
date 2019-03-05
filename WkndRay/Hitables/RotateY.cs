using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Hitables
{
    public class RotateY : AbstractHitable
    {
        public RotateY(IHitable hitable, double angle)
        {
            Hitable = hitable;
            Angle = angle;

            double radians = Math.PI / 180.0 * angle;
            SinTheta = Math.Sin(radians);
            CosTheta = Math.Cos(radians);
            var box = Hitable.GetBoundingBox(0.0, 1.0);
            var min = new PosVector(double.MaxValue, double.MaxValue, double.MaxValue).ToDoubleArray();
            var max = new PosVector(-double.MaxValue, -double.MaxValue, -double.MaxValue).ToDoubleArray();

            for (int i = 0; i < 2; i++)
            {
                double dubi = Convert.ToDouble(i);
                for (int j = 0; j < 2; j++)
                {
                    double dubj = Convert.ToDouble(j);
                    for (int k = 0; k < 2; k++)
                    {
                        double dubk = Convert.ToDouble(k);
                        double x = (dubi * box.Max.X) + ((1.0 - dubi) * box.Min.X);
                        double y = (dubj * box.Max.Y) + ((1.0 - dubj) * box.Min.Y);
                        double z = (dubk * box.Max.Z) + ((1.0 - dubk) * box.Min.Z);
                        double newx = (CosTheta * x) + (SinTheta * z);
                        double newz = (-SinTheta * x) + (CosTheta * z);
                        var tester = new PosVector(newx, y, newz).ToDoubleArray();
                        for (int c = 0; c < 3; c++)
                        {
                            if (tester[c] > max[c])
                            {
                                max[c] = tester[c];
                            }

                            if (tester[c] < min[c])
                            {
                                min[c] = tester[c];
                            }
                        }
                    }
                }
            }

            BoundingBox = new AABB(new PosVector(min), new PosVector(max));
        }

        public IHitable Hitable { get; }
        public double Angle { get; }
        public double SinTheta { get; }
        public double CosTheta { get; }
        public AABB BoundingBox { get; }

        public override HitRecord Hit(Ray ray, double tMin, double tMax)
        {
            var origin = ray.Origin.ToDoubleArray();
            var dir = ray.Direction.ToDoubleArray();
            origin[0] = (CosTheta * ray.Origin.X) - (SinTheta * ray.Origin.Z);
            origin[2] = (SinTheta * ray.Origin.X) + (CosTheta * ray.Origin.Z);
            dir[0] = (CosTheta * ray.Direction.X) - (SinTheta * ray.Direction.Z);
            dir[2] = (SinTheta * ray.Direction.X) + (CosTheta * ray.Direction.Z);
            var rotatedRay = new Ray(new PosVector(origin), new PosVector(dir));
            var hitRecord = Hitable.Hit(rotatedRay, tMin, tMax);
            if (hitRecord == null)
            {
                return null;
            }

            var p = hitRecord.P.ToDoubleArray();
            var normal = hitRecord.Normal.ToDoubleArray();
            p[0] = (CosTheta * hitRecord.P.X) + (SinTheta * hitRecord.P.Z);
            p[2] = (-SinTheta * hitRecord.P.X) + (CosTheta * hitRecord.P.Z);
            normal[0] = (CosTheta * hitRecord.Normal.X) + (SinTheta * hitRecord.Normal.Z);
            normal[2] = (-SinTheta * hitRecord.Normal.X) + (CosTheta * hitRecord.Normal.Z);
            return new HitRecord(hitRecord.T, new PosVector(p), new PosVector(normal), hitRecord.UvCoords, hitRecord.Material);
        }

        public override AABB GetBoundingBox(double t0, double t1)
        {
            return BoundingBox;
        }
    }
}
