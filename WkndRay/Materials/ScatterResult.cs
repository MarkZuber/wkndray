// -----------------------------------------------------------------------
// <copyright file="ScatterResult.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using WkndRay.Pdfs;

namespace WkndRay.Materials
{
    public class ScatterResult
    {
        public ScatterResult(bool isScattered, ColorVector attenuation, Ray specularRay, IPdf pdf)
        {
            IsScattered = isScattered;
            Attenuation = attenuation;
            SpecularRay = specularRay;
            Pdf = pdf;
        }

        public static ScatterResult False()
        {
            return new ScatterResult(false, new ColorVector(0.1f, 0.1f, 0.1f) /*ColorVector.Zero*/, null, null);
        }

        public bool IsScattered { get; }
        public Ray SpecularRay { get; }
        public bool IsSpecular => SpecularRay != null;
        public ColorVector Attenuation { get; }
        public IPdf Pdf { get; }  // probability distribution function
    }
}
