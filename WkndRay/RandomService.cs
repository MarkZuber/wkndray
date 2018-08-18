using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay
{
  public class RandomService : IRandomService
  {
    private readonly Random _random = new Random();
    private readonly object _lock = new object();

    /// <inheritdoc />
    public double NextDouble()
    {
      lock (_lock)
      {
        return _random.NextDouble();
      }
    }
  }
}
