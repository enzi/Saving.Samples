// <copyright project="Saving.Sample" file="SpawnCubeMenuSystem.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore.Saving;
using NZCore.UIToolkit;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Saving.Sample
{
    public partial struct SpawnCubeMenuSystem : ISystem, ISystemStartStop
    {
        private UIHelper<SpawnCubeViewModel, SpawnCubeViewModel.Data> ui;

        private EntityQuery savableObjectsQuery;
        private EntityQuery spawnerQuery;
        
        public void OnCreate(ref SystemState state)
        {
            savableObjectsQuery = SystemAPI.QueryBuilder()
                .WithAll<SavableEntity>()
                .WithNone<Parent>()
                .Build();
            
            spawnerQuery = SystemAPI.QueryBuilder()
                .WithAll<CubeSpawner>()
                .Build();
            
            state.RequireForUpdate<UIAssetsLoaded>();
            state.RequireForUpdate<ActivatorSavableSubScene>();
            state.RequireForUpdate<CubeSpawnerPrefab>();
        }

        public void OnStartRunning(ref SystemState state)
        {
            ui = new UIHelper<SpawnCubeViewModel, SpawnCubeViewModel.Data>("spawncube", "spawncube");
            ui.LoadPanel();
        }

        public void OnStopRunning(ref SystemState state)
        {
            ui.Unload();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            var spawnerAmount = spawnerQuery.CalculateEntityCount();
            ui.Model.SpawnerAmount = spawnerAmount;
            ui.Model.SpawnedObjects = savableObjectsQuery.CalculateEntityCount() - spawnerAmount;
            
            if (ui.Model.CreateSpawner)
            {
                //Debug.Log("CreateSpawner");
                // 
                // create a new spawner, this time though it won't be a subscene entity
                // because we don't add any subScene section data, making this entity global
                // for the saving system it doesn't make a difference other than 
                // it's located in Global in the SaveGame viewer
                var spawnerPrefab = SystemAPI.GetSingleton<CubeSpawnerPrefab>();
                state.EntityManager.Instantiate(spawnerPrefab.Prefab);
            }

            if (ui.Model.DestroySpawner)
            {
                var spawners = spawnerQuery.ToEntityArray(Allocator.Temp);
                state.EntityManager.DestroyEntity(spawners);
                //state.EntityManager.DestroyEntity(spawnerQuery);
            }
            
            if (ui.Model.SpawnCube)
            {
                var prefab = SystemAPI.GetSingleton<CubePrefab>();

                int amount = 10;
                var array = new NativeArray<Entity>(amount, Allocator.Temp);
            
                state.EntityManager.Instantiate(prefab.Prefab, array);
            
                Random random = new Random(1234 + (uint) SystemAPI.Time.ElapsedTime);
            
                for (var i = 0; i < array.Length; i++)
                {
                    var instancedEntity = array[i];
                    
                    var randomPos = SphericalPos(random.NextFloat(), random.NextFloat());
                    randomPos *= random.NextFloat(1, 10);
                    randomPos.y += 10;
                    
                    var transform = LocalTransform.FromPosition(randomPos);
                    state.EntityManager.SetComponentData(instancedEntity, transform);
                }
            }
            
            ui.Model.Clear();
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