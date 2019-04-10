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
        float GetValue(PosVector direction);
        PosVector Generate();
    }
}