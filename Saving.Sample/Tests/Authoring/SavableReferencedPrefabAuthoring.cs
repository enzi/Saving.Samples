// <copyright project="Saving.Sample" file="SavableReferencedPrefabAuthoring.cs">
// Copyright © 2025 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class SavableReferencedPrefabAuthoring : MonoBehaviour
    {
        public class SavableReferencedPrefabAuthoringBaker : Baker<SavableReferencedPrefabAuthoring>
        {
            public override void Bake(SavableReferencedPrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<SavableReferencedPrefab>(entity);
            }
        }
    }
}