// <copyright project="Saving.Sample" file="CubeSpawnerAuthoring.cs">
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
                
                // a test case for a additional savable entity
                //
                // var ent = CreateAdditionalEntity(TransformUsageFlags.None, false, "test entity 1");
                // AddComponent(ent, new SavableEntity());
                // AddComponent(ent, new AdditionalTestData());
                // AddComponent(ent, new DestroyEntity());
                // AddComponent(ent, new SavableAdditionalIndex(0));
                // SetComponentEnabled<DestroyEntity>(ent, false);
                //
                // var ent2 = CreateAdditionalEntity(TransformUsageFlags.None, false, "test entity 2");
                // AddComponent(ent2, new SavableEntity());
                // AddComponent(ent2, new AdditionalTestData());
                // AddComponent(ent2, new DestroyEntity());
                // AddComponent(ent2, new SavableAdditionalIndex(1));
                // SetComponentEnabled<DestroyEntity>(ent2, false);
                
                // AddComponent(entity, new ToBeSplitTest()
                // {
                //     Value1 = 10,
                //     Value2 = 20,
                //     Value3 = 30,
                //     Value4 = 40
                // });
            }
        }
    }

    public struct AdditionalTestData : IComponentData, ISavable
    {
        public int Value;
    }
}