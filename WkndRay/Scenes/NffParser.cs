// -----------------------------------------------------------------------
// <copyright file="NffParser.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using WkndRay.Hitables;
using WkndRay.Materials;
using WkndRay.Textures;

namespace WkndRay.Scenes
{
  public class NffParser
  {
    public static IScene ParseFile(string filePath, int imageWidth, int imageHeight)
    {
      var background = new Func<Ray, ColorVector>(ray => new ColorVector(0.0, 0.0, 0.0));
      var lights = new HitableList();
      var world = new HitableList();
      var cameraAt = new PosVector(0.0, 0.0, 0.0);
      var cameraFrom = new PosVector(0.0, 0.0, 0.0);
      var cameraUp = new PosVector(0.0, 0.0, 0.0);

      var lookingFor = LookingFor.Instruction;

      IMaterial currentMaterial = new LambertianMaterial(new ColorTexture(0.0, 0.0, 0.0));

      var polyVectors = new List<PosVector>();
      var currentItemCounter = 0;

      var lines = File.ReadAllLines(filePath);

      foreach (var line in lines)
      {
        var split = line.Split(' ', '\t');

        switch (lookingFor)
        {
        case LookingFor.Instruction:
        {
          var instruction = split[0];

          if (instruction == "b")
          {
            // background color
            background = ray => new ColorVector(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]));
          }
          else if (instruction == "v")
          {
            // viewpoint location
            lookingFor = LookingFor.ViewpointFrom;
          }
          else if (instruction == "l")
          {
            // positional light
            //var colorVector = split.Length == 7
            //                    ? new ColorVector(
            //                      double.Parse(split[4]),
            //                      double.Parse(split[5]),
            //                      double.Parse(split[6]))
            //                    : new ColorVector(7.0, 7.0, 7.0);
            var colorVector = new ColorVector(7.0, 7.0, 7.0);
            var sphere = new Sphere(
              new PosVector(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3])),
              1.5,
              new DiffuseLight(new ColorTexture(colorVector)));
            lights.Add(sphere);
            world.Add(sphere);
          }
          else if (instruction == "f")
          {
            // println!("reading f: {}", num);
            // object material properties
            // "f" red green blue Kd Ks Shine T index_of_refraction
            // Kd Diffuse component
            // Ks Specular
            // Shine Phong cosine power for highlights
            // T Transmittance (fraction of contribution of the transmitting ray).
            // Usually, 0 <= Kd <= 1 and 0 <= Ks <= 1, though it is not required that Kd + Ks = 1. Note that transmitting objects (T > 0) are considered to have two sides for algorithms that need these (normally, objects have one side).
            // todo: i don't think i'm assigning the correct values into my solidmaterial yet
            //currentMaterial = new SolidMaterial(
            //  0.0, // kAmbient
            //  double.Parse(split[4]),  // kDiffuse
            //  double.Parse(split[5]),  // kSpecular
            //  double.Parse(split[7]),  // kReflection
            //  double.Parse(split[8]),  // kTransparent
            //  double.Parse(split[8]),  // refraction    -- todo: which is which here?
            //  double.Parse(split[6]),  // gloss
            //  new ColorVector(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]))
            //);
            // todo: need to look at diffuse/specular/reflection/transparent and pick an appropriate material for it...
            //currentMaterial = new LambertianMaterial(
            //  new ColorTexture(
            //    new ColorVector(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]))));
            if (double.Parse(split[5]) > 0.01)
            {
              currentMaterial = new MetalMaterial(
                new ColorVector(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3])),
                1.5);
            }
            else
            {
              currentMaterial = new LambertianMaterial(
                new ColorTexture(
                  new ColorVector(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]))));
            }
          }
          else if (instruction == "c")
          {
            // cone or cylinder
          }
          else if (instruction == "s")
          {
            // sphere
            world.Add(
              new Sphere(
                new PosVector(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3])),
                double.Parse(split[4]),
                currentMaterial));
          }
          else if (instruction == "p")
          {
            // polygon
            currentItemCounter = int.Parse(split[1]);
            polyVectors = new List<PosVector>();
            lookingFor = LookingFor.Polygon;
          }
          else if (instruction == "pp")
          {
            // polygon patch
          }
          else if (instruction == "#")
          {
            // comment
          }
        }
          break;
        case LookingFor.Polygon:
        {
          if (currentItemCounter > 0)
          {
            currentItemCounter--;
            polyVectors.Add(new PosVector(double.Parse(split[0]), double.Parse(split[1]), double.Parse(split[2])));
          }

          if (currentItemCounter == 0)
          {
            if (polyVectors.Count >= 3)
            {
              var firstVert = polyVectors[0];
              var prevVert = polyVectors[1];
              var thisVert = polyVectors[2];
              world.Add(
                new Triangle(
                  new List<PosVector>
                  {
                    firstVert,
                    prevVert,
                    thisVert
                  },
                  currentMaterial));

              for (var i = 3; i < polyVectors.Count; i++)
              {
                prevVert = thisVert;
                thisVert = polyVectors[i];
                    world.Add(
                      new Triangle(
                        new List<PosVector>
                        {
                      firstVert,
                      prevVert,
                      thisVert
                        },
                        currentMaterial));
                  }
            }

            lookingFor = LookingFor.Instruction;
          }
        }
          break;
        case LookingFor.ViewpointFrom:
        {
          cameraFrom = new PosVector(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]));
          lookingFor = LookingFor.ViewpointAt;
        }
          break;
        case LookingFor.ViewpointAt:
        {
          cameraAt = new PosVector(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]));
          lookingFor = LookingFor.ViewpointUp;
        }
          break;
        case LookingFor.ViewpointUp:
        {
          cameraUp = new PosVector(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]));
          lookingFor = LookingFor.ViewpointAngle;
        }
          break;
        case LookingFor.ViewpointAngle:
        {
          // todo: implement
          lookingFor = LookingFor.ViewpointHither;
        }
          break;
        case LookingFor.ViewpointHither:
        {
          // todo: implement
          lookingFor = LookingFor.ViewpointResolution;
        }
          break;
        case LookingFor.ViewpointResolution:
        {
          //resolutionX = int.Parse(split[1]);
          //resolutionY = int.Parse(split[2]);

          lookingFor = LookingFor.Instruction;
        }
          break;
        }
      }

      return new NffParserScene(
        new Camera(
          cameraFrom,
          cameraAt,
          cameraUp,
          50.0,
          Convert.ToDouble(imageWidth) / Convert.ToDouble(imageHeight),
          0.0,
          10.0),
        new BvhNode(world, 0.0, 1.0),
        lights,
        background);
    }

    private enum LookingFor
    {
      Instruction,
      ViewpointFrom,
      ViewpointAt,
      ViewpointUp,
      ViewpointAngle,
      ViewpointHither,
      ViewpointResolution,
      Polygon
    }

    private class NffParserScene : IScene
    {
      private readonly Func<Ray, ColorVector> _backgroundFunc;

      private readonly Camera _camera;
      private readonly IHitable _lightHitable;
      private readonly IHitable _world;

      public NffParserScene(Camera camera, IHitable world, IHitable lightHitable, Func<Ray, ColorVector> backgroundFunc)
      {
        _camera = camera;
        _world = world;
        _lightHitable = lightHitable;
        _backgroundFunc = backgroundFunc;
      }

      public Camera GetCamera(int imageWidth, int imageHeight)
      {
        return _camera;
      }

      public IHitable GetWorld()
      {
        return _world;
      }

      public IHitable GetLightHitable()
      {
        return _lightHitable;
      }

      public Func<Ray, ColorVector> GetBackgroundFunc()
      {
        return _backgroundFunc;
      }
    }
  }
}