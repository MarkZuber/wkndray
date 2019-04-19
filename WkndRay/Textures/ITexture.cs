using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace WkndRay.Textures
{
    public interface ITexture
    {
        ColorVector GetValue(Point2D uvCoords, Vector3 p);
    }
}
