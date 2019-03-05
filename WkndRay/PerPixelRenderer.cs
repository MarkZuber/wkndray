// -----------------------------------------------------------------------
// <copyright file="Renderer.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WkndRay.Scenes;

namespace WkndRay
{
  public class PerPixelRenderer : IRenderer
  {
    public event EventHandler<RenderProgressEventArgs> Progress;

    public void Render(IPixelBuffer pixelArray, IScene scene, RenderConfig renderConfig)
    {
      Render(pixelArray, scene.GetCamera(pixelArray.Width, pixelArray.Height), scene.GetWorld(), scene.GetLightHitable(), renderConfig, scene.GetBackgroundFunc());
    }

    public void Render(IPixelBuffer pixelArray, Camera camera, IHitable world, IHitable lightHitable, RenderConfig renderConfig, Func<Ray, ColorVector> backgroundFunc)
    {
      Progress?.Invoke(this, new RenderProgressEventArgs(0.0));

      if (renderConfig.TwoPhase)
      {
        // do a much lighter weight first pass first.
        RenderMultiThreaded(
          pixelArray,
          camera,
          world,
          lightHitable,
          new RenderConfig(renderConfig.NumThreads, 5, 1),
          backgroundFunc);
      }

      RenderMultiThreaded(pixelArray, camera, world, lightHitable, renderConfig, backgroundFunc);
    }

    private void RenderMultiThreaded(IPixelBuffer pixelArray, Camera camera, IHitable world, IHitable lightHitable, RenderConfig renderConfig, Func<Ray, ColorVector> backgroundFunc)
    {
      var rayTracer = new RayTracer(camera, world, lightHitable, renderConfig, pixelArray.Width, pixelArray.Height, backgroundFunc);
      ThreadPool.SetMinThreads(renderConfig.NumThreads * 3, renderConfig.NumThreads * 3);

      var queueDataAvailableEvent = new AutoResetEvent(false);
      var inputQueue = new ConcurrentQueue<RenderInput>();
      var resultQueue = new ConcurrentQueue<RenderResult>();

      for (var y = 0; y < pixelArray.Height; y++)
      {
        for (var x = 0; x < pixelArray.Width; x++)
        {
          inputQueue.Enqueue(new RenderInput(x, y));
        }
      }

      var tasks = new List<Task>();

      try
      {
        for (var thid = 0; thid < renderConfig.NumThreads; thid++)
        {
          tasks.Add(
            Task.Run(() => RenderFunc(rayTracer, inputQueue, resultQueue, queueDataAvailableEvent)));
        }

        tasks.Add(Task.Run(() => ResultFunc(pixelArray, resultQueue, queueDataAvailableEvent)));

        Task.WaitAll(tasks.ToArray());
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
    }

    private static void RenderFunc(
      IRayTracer rayTracer,
      ConcurrentQueue<RenderInput> inputQueue,
      ConcurrentQueue<RenderResult> resultQueue,
      AutoResetEvent queueDataAvailableEvent)
    {
      try
      {
        while (inputQueue.TryDequeue(out RenderInput input))
        {
          var color = rayTracer.GetPixelColor(input.X, input.Y);
          resultQueue.Enqueue(new RenderResult(input.X, input.Y, color));
          queueDataAvailableEvent.Set();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
    }

    private void ResultFunc(
      IPixelBuffer pixelBuffer,
      ConcurrentQueue<RenderResult> resultQueue,
      AutoResetEvent queueDataAvailableEvent)
    {
      try
      {
        var incompletePixels = new HashSet<Tuple<int, int>>();
        for (var y = 0; y < pixelBuffer.Height; y++)
        {
          for (int x = 0; x < pixelBuffer.Width; x++)
          {
            incompletePixels.Add(Tuple.Create(x, y));
          }
        }

        var totalPixels = Convert.ToDouble(pixelBuffer.Height * pixelBuffer.Width);

        int previousPercent = 0;

        while (incompletePixels.Count > 0)
        {
          queueDataAvailableEvent.WaitOne(TimeSpan.FromMilliseconds(1000));

          while (resultQueue.TryDequeue(out var renderResult))
          {
            // assert pixelArray.Width == renderLineResult.Count
            pixelBuffer.SetPixelColor(renderResult.X, renderResult.Y, renderResult.Color);
            incompletePixels.Remove(Tuple.Create(renderResult.X, renderResult.Y));

            var completePixels = Convert.ToDouble(totalPixels - incompletePixels.Count);
            var percentComplete = completePixels / totalPixels * 100.0;
            int intPercent = Convert.ToInt32(percentComplete);
            if (intPercent > previousPercent)
            {
              previousPercent = intPercent;
              Console.WriteLine($"Percent Complete: {percentComplete:F}%");
              Progress?.Invoke(this, new RenderProgressEventArgs(percentComplete));
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
    }

    private class RenderInput
    {
      public RenderInput(int x, int y)
      {
        X = x;
        Y = y;
      }

      public int X { get; }
      public int Y { get; }
    }

    private class RenderResult
    {
      public RenderResult(int x, int y, ColorVector color)
      {
        X = x;
        Y = y;
        Color = color;
      }

      public int X { get; }
      public int Y { get; }
      public ColorVector Color { get; }
    }
  }
}