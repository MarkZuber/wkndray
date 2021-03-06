﻿// -----------------------------------------------------------------------
// <copyright file="IScene.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay.Scenes
{
    public interface IScene
    {
        Camera GetCamera(int imageWidth, int imageHeight);
        IHitable GetWorld();
        IHitable GetLightHitable();
        Func<Ray, ColorVector> GetBackgroundFunc();
    }
}