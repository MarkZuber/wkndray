﻿// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WkndRay;
using WkndRay.Executors;
using WkndRay.Scenes;

namespace RayCon
{
  public static class Program
  {
    private const string OutputDirectory = @"c:\repos\wkndray\images";
    private const int Width = 200;
    private const int Height = 100;

    public static void Main(string[] args)
    {
      int numThreads = 1; // Environment.ProcessorCount;
      const int RayTraceDepth = 50;
      const int NumSamples = 10;
      var renderConfig = new RenderConfig(numThreads, RayTraceDepth, NumSamples);

      var scene = new ManySpheresScene();
      var renderer = new Renderer();
      var pixelBuffer = new PixelBuffer(Width, Height);

      string name = scene.GetType().Name.ToLowerInvariant();
      Console.WriteLine($"Executing {name} at resolution ({pixelBuffer.Width},{pixelBuffer.Height})");
      Console.WriteLine($"  Num Threads   = {numThreads}");
      Console.WriteLine($"  RayTraceDepth = {RayTraceDepth}");
      Console.WriteLine($"  Num Samples   = {NumSamples}");

      var sw = Stopwatch.StartNew();
      renderer.Render(
        pixelBuffer,
        scene.GetCamera(pixelBuffer.Width, pixelBuffer.Height),
        scene.GetWorld(),
        renderConfig);
      sw.Stop();
      Console.WriteLine();
      Console.WriteLine($"Render complete: {sw.ElapsedMilliseconds}ms");

      string outputPath = Path.Combine(OutputDirectory, $"{name}.png");
      Console.WriteLine($"Saving image to {outputPath}");
      pixelBuffer.SaveAsFile(outputPath);

      // RunExecutors();
    }

    public static void RunExecutors()
    {
      const int NumSamples = 100;
      var executors = new List<IExecutor>
      {
        new ManySpheresGenerator(NumSamples),
        //new BetterCameraGenerator(NumSamples),
        //new MaterialGenerator(NumSamples),
        //new DiffuseGenerator(NumSamples),
        // new FirstCameraTracer(NumSamples),
        //new FirstHitableTracer(),
        //new ShadedRaySphereGenerator(),
        //new SimpleRaySphereGenerator(),
        //new SimpleRayImageGenerator(),
        //new SimplePatternGenerator(),
      };

      foreach (var executor in executors)
      {
        string name = executor.GetType().Name.ToLowerInvariant();
        Console.WriteLine($"Executing {name}");
        var pixelBuffer = executor.Execute(Width, Height);
        pixelBuffer.SaveAsFile(Path.Combine(OutputDirectory, $"{name}.png"));
      }
    }
  }
}