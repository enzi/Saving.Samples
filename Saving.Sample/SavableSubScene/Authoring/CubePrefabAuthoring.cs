// <copyright project="Saving.Sample" file="CubePrefabAuthoring.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class CubePrefabAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        
        public class CubePrefabBaker : Baker<CubePrefabAuthoring>
        {
            public override void Bake(CubePrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new CubePrefab()
                {
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}