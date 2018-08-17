// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO;
using WkndRay;

namespace RayCon
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      const string OutputDirectory = @"c:\repos\wkndray\images";
      var pixelBuffer = new SimplePatternGenerator().Execute(200, 100);
      pixelBuffer.SaveAsFile(Path.Combine(OutputDirectory, "simplepattern.png"));
    }
  }
}