// <copyright project="NZCore" file="CubeSpawnerAuthoring.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore;
using NZCore.Saving;
using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class CubeSpawnerAuthoring : MonoBehaviour
    {
        public int Amount = 5;
        public float Interval = 1;
        
        public class CubeSpawnerAuthoringBaker : Baker<CubeSpawnerAuthoring>
        {
            public override void Bake(CubeSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new CubeSpawner
                {
                    Amount = authoring.Amount,
                    Interval = authoring.Interval
                });
                
                var ent = CreateAdditionalEntity(TransformUsageFlags.None, false, "test entity");
                AddComponent(ent, new SavableEntity());
                AddComponent(ent, new AdditionalTestData());
                AddComponent(ent, new DestroyEntity());
                SetComponentEnabled<DestroyEntity>(ent, false);
            }
        }
    }

    public struct AdditionalTestData : IComponentData, ISavable
    {
        public int Value;
    }
}