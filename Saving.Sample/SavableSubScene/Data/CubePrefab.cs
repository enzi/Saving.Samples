// <copyright project="NZCore" file="CubePrefab.cs" version="0.1">
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