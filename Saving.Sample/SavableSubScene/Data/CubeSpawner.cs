// <copyright project="Saving.Sample" file="CubeSpawner.cs">
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
    
    public struct CubeSpawnerPrefab : IComponentData
    {
        public Entity Prefab;
    }
}