// <copyright project="NZCore" file="EnableableCubeAuthoring.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Saving.Sample.Data;
using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class EnableableCubeAuthoring : MonoBehaviour
    {
        private class EnableableCubeAuthoringBaker : Baker<EnableableCubeAuthoring>
        {
            public override void Bake(EnableableCubeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new EnableableCubeTag());
            }
        }
    }
}