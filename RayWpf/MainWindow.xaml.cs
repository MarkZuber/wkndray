// -----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
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
      const int ImageWidth = 500;
      const int ImageHeight = 500;
      const int RayTraceDepth = 50;
      const int NumSamples = 1000;
      var renderConfig = new RenderConfig(numThreads, RayTraceDepth, NumSamples)
      {
        TwoPhase = false
      };

      var scene = new CornellBoxScene();
      // var scene = NffParser.ParseFile(Path.Combine(NffDirectory, "balls5.nff"), ImageWidth, ImageHeight);

      var pixelBuffer = new WpfPixelBuffer(Dispatcher, ImageWidth, ImageHeight);
      RenderImage.Source = pixelBuffer.WriteableBitmap;
      IRenderer renderer = new PerPixelRenderer();

      string name = scene.GetType().Name.ToLowerInvariant();
      string outputPath = System.IO.Path.Combine(OutputDirectory, $"{name}.png");

      renderer.Progress += (_, args) => { Dispatcher.Invoke(() => { RenderProgress.Value = args.PercentComplete; }); };
      Task.Run(
        () =>
        {
          Dispatcher.Invoke(() => RenderButton.IsEnabled = false);
          renderer.Render(pixelBuffer, scene, renderConfig);
          pixelBuffer.SaveAsFile(outputPath);
          Dispatcher.Invoke(() => RenderButton.IsEnabled = true);
        });
    }
  }
}