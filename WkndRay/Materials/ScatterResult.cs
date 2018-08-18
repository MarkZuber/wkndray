using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Materials
{
  public class ScatterResult
  {
    public ScatterResult(bool isScattered, ColorVector attenuation, Ray scatteredRay)
    {
      IsScattered = isScattered;
      Attenuation = attenuation;
      ScatteredRay = scatteredRay;
    }
      
    public bool IsScattered { get; }
    public ColorVector Attenuation { get; }
    public Ray ScatteredRay { get; }
  }
}
