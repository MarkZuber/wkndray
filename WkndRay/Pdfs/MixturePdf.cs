// -----------------------------------------------------------------------
// <copyright file="MixturePdf.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Pdfs
{
    public class MixturePdf : IPdf
    {
        public MixturePdf(IPdf p0, IPdf p1)
        {
            P0 = p0;
            P1 = p1;
        }

        public IPdf P0 { get; }
        public IPdf P1 { get; }

        public double GetValue(PosVector direction)
        {
            return (0.5 * P0.GetValue(direction)) + (0.5 * P1.GetValue(direction));
        }

        public PosVector Generate()
        {
            return RandomService.NextDouble() < 0.5 ? P0.Generate() : P1.Generate();
        }
    }
}