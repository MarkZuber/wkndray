// -----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using WkndRay;
using WkndRay.Scenes;

namespace RayWpf
{
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string OutputDirectory = @"c:\repos\wkndray\images";
        private const string NffDirectory = @"c:\repos\wkndray\nff";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RenderButton_OnClick(object sender, RoutedEventArgs e)
        {
            int numThreads = Environment.ProcessorCount;
            const int ImageWidth = 1000; 
            const int ImageHeight = 1000;
            const int RayTraceDepth = 50;
            const int NumSamples = 10;
            var renderConfig = new RenderConfig(numThreads, RayTraceDepth, NumSamples)
            {
                TwoPhase = false
            };

            var scene = new CornellBoxScene();
            // var scene = NffParser.ParseFile(Path.Combine(NffDirectory, "balls5.nff"), ImageWidth, ImageHeight);

            // var pixelBuffer = new PixelBuffer(ImageWidth, ImageHeight);
            var pixelBuffer = new WpfPixelBuffer(Dispatcher, ImageWidth, ImageHeight);
            RenderImage.Source = pixelBuffer.WriteableBitmap;
            IRenderer renderer = new PerLineRenderer();

            string name = scene.GetType().Name.ToLowerInvariant();
            string outputPath = Path.Combine(OutputDirectory, $"{name}.png");

            renderer.Progress += (_, args) => { Dispatcher.Invoke(() => { RenderProgress.Value = args.PercentComplete; }); };
            Task.Run(() =>
            {
                Dispatcher.Invoke(() => RenderButton.IsEnabled = false);
                var sw = Stopwatch.StartNew();
                var rendererData = renderer.Render(pixelBuffer, scene, renderConfig);
                sw.Stop();
                Dispatcher.Invoke(() =>
                {
                    RenderTime.Text = $"{sw.ElapsedMilliseconds}ms";
                    AvgGetColorMs.Text = $"{rendererData.GetAveragePixelColorMilliseconds()}ms";
                    RenderButton.IsEnabled = true;
                });
                pixelBuffer.SaveAsFile(outputPath);
            });
        }
    }
}
