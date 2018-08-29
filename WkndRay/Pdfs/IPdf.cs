// -----------------------------------------------------------------------
// <copyright file="IPdf.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Pdfs
{
  /// <summary>
  ///   Probability Distribution Function
  /// </summary>
  public interface IPdf
  {
    double GetValue(PosVector direction);
    PosVector Generate();
  }
}