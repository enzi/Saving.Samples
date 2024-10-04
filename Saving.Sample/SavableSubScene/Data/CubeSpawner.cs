// <copyright project="NZCore" file="CubeSpawner.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;

namespace Saving.Sample
{
    public struct CubeSpawner : IComponentData
    {
        public int Amount;
        public float Interval;
    }
}