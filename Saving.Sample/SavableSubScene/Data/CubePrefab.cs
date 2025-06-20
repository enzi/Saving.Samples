// <copyright project="Saving.Sample" file="CubePrefab.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;

namespace Saving.Sample
{
    public struct CubePrefab : IComponentData
    {
        public Entity Prefab;
    }
}