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

namespace WkndRay
{
  public class Renderer
  {
    public event EventHandler<RenderProgressEventArgs> Progress;

    public void Render(
      IRandomService randomService,
      IPixelBuffer pixelArray,
      Camera camera,
      IHitable world,
      RenderConfig renderConfig)
    {
      Progress?.Invoke(this, new RenderProgressEventArgs(0.0));

      RenderMultiThreaded(randomService, pixelArray, camera, world, renderConfig);
    }

    private void RenderMultiThreaded(
      IRandomService randomService,
      IPixelBuffer pixelArray,
      Camera camera,
      IHitable world,
      RenderConfig renderConfig)
    {
      var rayTracer = new RayTracer(randomService, camera, world, renderConfig, pixelArray.Width, pixelArray.Height);
      ThreadPool.SetMinThreads(renderConfig.NumThreads * 3, renderConfig.NumThreads * 3);

      var queueDataAvailableEvent = new AutoResetEvent(false);
      var rowQueue = new ConcurrentQueue<int>();
      var resultQueue = new ConcurrentQueue<RenderLineResult>();

      for (var y = 0; y < pixelArray.Height; y++)
      {
        rowQueue.Enqueue(y);
      }

      var tasks = new List<Task>();

      try
      {
        for (var thid = 0; thid < renderConfig.NumThreads; thid++)
        {
          tasks.Add(
            Task.Run(() => RenderFunc(rayTracer, pixelArray.Width, rowQueue, resultQueue, queueDataAvailableEvent)));
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
      int pixelWidth,
      ConcurrentQueue<int> rowQueue,
      ConcurrentQueue<RenderLineResult> resultQueue,
      AutoResetEvent queueDataAvailableEvent)
    {
      while (rowQueue.TryDequeue(out int y))
      {
        var rowPixels = new List<ColorVector>();
        for (var x = 0; x < pixelWidth; x++)
        {
          rowPixels.Add(rayTracer.GetPixelColor(x, y));
        }

        resultQueue.Enqueue(new RenderLineResult(y, rowPixels));
        queueDataAvailableEvent.Set();
      }
    }

    private void ResultFunc(
      IPixelBuffer pixelBuffer,
      ConcurrentQueue<RenderLineResult> resultQueue,
      AutoResetEvent queueDataAvailableEvent)
    {
      var incompleteRows = new HashSet<int>();
      for (var y = 0; y < pixelBuffer.Height; y++)
      {
        incompleteRows.Add(y);
      }

      while (incompleteRows.Count > 0)
      {
        queueDataAvailableEvent.WaitOne(TimeSpan.FromMilliseconds(1000));

        while (resultQueue.TryDequeue(out var renderLineResult))
        {
          // assert pixelArray.Width == renderLineResult.Count
          pixelBuffer.SetPixelRowColors(renderLineResult.Y, renderLineResult.RowPixels);
          incompleteRows.Remove(renderLineResult.Y);

          var totalRows = Convert.ToDouble(pixelBuffer.Height);
          var completeRows = Convert.ToDouble(pixelBuffer.Height - incompleteRows.Count);
          var percentComplete = completeRows / totalRows * 100.0;
          Console.WriteLine($"Percent Complete: {percentComplete:F}%");
          Progress?.Invoke(this, new RenderProgressEventArgs(percentComplete));
        }
      }
    }

    private class RenderLineResult
    {
      public RenderLineResult(int y, List<ColorVector> rowPixels)
      {
        Y = y;
        RowPixels = rowPixels;
      }

      public List<ColorVector> RowPixels { get; }
      public int Y { get; }
    }
  }
}