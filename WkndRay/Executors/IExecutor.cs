using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Executors
{
  public interface IExecutor
  {
    PixelBuffer Execute(int width, int height);
  }
}
