// <copyright project="Saving.Sample" file="ReferencedPrefabAuthoring.cs">
// Copyright © 2025 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class ReferencedPrefabAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        
        public class ReferencedPrefabAuthoringBaker : Baker<ReferencedPrefabAuthoring>
        {
            public override void Bake(ReferencedPrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new ReferencedPrefabLookup()
                {
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}