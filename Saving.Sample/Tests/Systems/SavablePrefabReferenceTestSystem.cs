// <copyright project="Saving.Sample" file="SavablePrefabReferenceTestSystem.cs">
// Copyright © 2025 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Saving.Sample
{
    public partial struct SavablePrefabReferenceTestSystem : ISystem
    {
        private EntityQuery testQuery;
        
        public void OnCreate(ref SystemState state)
        {
            testQuery = SystemAPI.QueryBuilder()
                .WithAll<SavableReferencedPrefab>()
                .Build();
            
            state.RequireForUpdate<ActivatorTestScene>();
            state.RequireForUpdate<ReferencedPrefabLookup>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (testQuery.IsEmpty)
            {
                Debug.Log("setting up referenced prefabs");

                var prefab = SystemAPI.GetSingleton<ReferencedPrefabLookup>().Prefab;
                var elapsedTime = SystemAPI.Time.ElapsedTime;
                Random random = new Random(123 + (uint) elapsedTime);

                var prefabContainer = SystemAPI.GetSingletonBuffer<PrefabContainerBuffer>();

                for (int i = 0; i < 10; i++)
                {
                    var randomIndex = random.NextInt(0, prefabContainer.Length);

                    var ent = state.EntityManager.Instantiate(prefab);
                    
                    state.EntityManager.SetComponentData(ent, new SavableReferencedPrefab()
                    {
                        ReferencedPrefab = prefabContainer[randomIndex].Prefab
                    });
                }
            }
        }
    }
}