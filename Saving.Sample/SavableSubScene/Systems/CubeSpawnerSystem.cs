// <copyright project="NZCore" file="CubeSpawnerSystem.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore.Saving;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace Saving.Sample
{
    public partial struct CubeSpawnerSystem : ISystem
    {
        private EntityQuery spawnerQuery;
        private double lastTime;
        
        public void OnCreate(ref SystemState state)
        {
            spawnerQuery = SystemAPI.QueryBuilder()
                .WithAll<CubeSpawner>()
                .Build();
            
            state.RequireForUpdate<CubePrefab>();
            state.RequireForUpdate<ActivatorSavableSubScene>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (spawnerQuery.IsEmpty)
            {
                return;
            }

            var elapsedTime = SystemAPI.Time.ElapsedTime;
            var prefab = SystemAPI.GetSingleton<CubePrefab>();
            var spawnerEntities = spawnerQuery.ToEntityArray(Allocator.Temp);
            Random random = new Random(123 + (uint) elapsedTime);
            
            foreach (var spawnerEntity in spawnerEntities)
            {
                var spawner = SystemAPI.GetComponent<CubeSpawner>(spawnerEntity);

                if (elapsedTime > lastTime + spawner.Interval)
                {
                    lastTime = elapsedTime;
                }
                else
                {
                    continue;
                }
                
                var array = new NativeArray<Entity>(spawner.Amount, Allocator.Temp);
            
                // spawn the prefab Amount times
                state.EntityManager.Instantiate(prefab.Prefab, array);
            
                for (var i = 0; i < array.Length; i++)
                {
                    var instancedEntity = array[i];
                    
                    // give it some random position
                    var randomPos = SphericalPos(random.NextFloat(), random.NextFloat());
                    randomPos *= random.NextFloat(1, 10);
                    randomPos.y += 10;
                    
                    var transform = LocalTransform.FromPosition(randomPos);
                    state.EntityManager.SetComponentData(instancedEntity, transform);
                }
                
                //state.EntityManager.DestroyEntity(spawnerEntity);
                
                // request a save (not needed because interface handles it)
                //var requests = SystemAPI.GetSingleton<SaveSystemRequestSingleton>();
                //requests.AddManualSave();
            }
        }
        
        public static float3 SphericalPos(float azimuth, float inclination)
        {
            azimuth *= math.PI;
            inclination *= math.PI;
            float si = math.sin (inclination);
            float ci = math.cos (inclination);
            float sa = math.sin (azimuth);
            float ca = math.cos (azimuth);
            return new float3(si * sa, -ci, si * ca);
        }
    }
}