// -----------------------------------------------------------------------
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
        private const int Width = 1000;
        private const int Height = 1000;

        public static void Main(string[] args)
        {
            int numThreads = Environment.ProcessorCount;
            const int RayTraceDepth = 50;
            const int NumSamples = 1000;
            var renderConfig = new RenderConfig(numThreads, RayTraceDepth, NumSamples);

            string globeImagePath = Path.Combine(OutputDirectory, "globetex.jpg");
            // var scene = new ManySpheresScene();
            // var scene = new NoiseSpheresScene();
            // var scene = new ImageTextureScene(globeImagePath);
            // var scene = new LightsScene(globeImagePath);
            var scene = new CornellBoxScene();
            // var scene = new CornellBoxWithSmokeScene();
            IRenderer renderer = new PerPixelRenderer();
            var pixelBuffer = new PixelBuffer(Width, Height);

            string name = scene.GetType().Name.ToLowerInvariant();
            Console.WriteLine($"Executing {name} at resolution ({pixelBuffer.Width},{pixelBuffer.Height})");
            Console.WriteLine($"  Num Threads   = {numThreads}");
            Console.WriteLine($"  RayTraceDepth = {RayTraceDepth}");
            Console.WriteLine($"  Num Samples   = {NumSamples}");

            var sw = Stopwatch.StartNew();
            var rendererData = renderer.Render(pixelBuffer, scene, renderConfig);
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine($"Render complete: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Total Pixel Color Time: {rendererData.GetTotalPixelColorMilliseconds()}ms");
            Console.WriteLine($"Per Pixel Avg Time:     {rendererData.GetAveragePixelColorMilliseconds()}ms");

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
