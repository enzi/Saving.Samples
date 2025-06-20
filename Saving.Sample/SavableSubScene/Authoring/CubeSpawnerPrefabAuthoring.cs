// <copyright project="Saving.Sample" file="CubePrefabAuthoring.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class CubeSpawnerPrefabAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        
        public class CubeSpawnerPrefabBaker : Baker<CubeSpawnerPrefabAuthoring>
        {
            public override void Bake(CubeSpawnerPrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new CubeSpawnerPrefab()
                {
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}