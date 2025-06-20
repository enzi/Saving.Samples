// <copyright project="Saving.Sample" file="GlobalObjectCreatorSystem.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using System;
using NZCore;
using NZCore.Interfaces;
using NZCore.Saving;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public struct GlobalObjectComponent : IComponentData, ISavable
    {
        public int Value;
        public float FloatValue;
    }

    public struct GlobalObjectComponent2 : IComponentData, IEnableableComponent
    {
        public int Value2;
    }
    
    public unsafe struct GlobalTestHashMap : IInitSingleton, IEnableableComponent, IDisposable
    {
        private UnsafeHashMap<int, char>* hashMap;
        
        public ref UnsafeHashMap<int, char> HashMap => ref *hashMap;

        public void Init()
        {
            hashMap = UnsafeCreateHelper.CreateHashMap<int, char>(0, Allocator.Persistent);
        }

        public void Dispose()
        {
            hashMap->Dispose();
            AllocatorManager.Free(Allocator.Persistent, hashMap);
        }
    }
    
    public partial struct GlobalObjectCreatorSystem : ISystem, ISystemStartStop
    {
        private Entity globalObjectEntity;
        private Entity globalHashMapEntity;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ActivatorGlobalObjects>();
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnStartRunning(ref SystemState state)
        {
            var archetype = state.EntityManager.CreateArchetype(stackalloc ComponentType[]
            {
                ComponentType.ReadOnly<SavableObject>(),
                ComponentType.ReadOnly<SaveStateLoaded>(),
                ComponentType.ReadOnly<GlobalObjectComponent>(),
                ComponentType.ReadOnly<GlobalObjectComponent2>(),
            });
            
            globalObjectEntity = state.EntityManager.CreateEntity(archetype);
            
            state.EntityManager.SetComponentEnabled<SaveStateLoaded>(globalObjectEntity, false);
            
            state.EntityManager.SetComponentData(globalObjectEntity, new SavableObject()
            {
                SaveId = 101
            });
            
            state.EntityManager.SetComponentData(globalObjectEntity, new GlobalObjectComponent()
            {
                Value = 2,
                FloatValue = 2
            });
            
            state.EntityManager.SetComponentData(globalObjectEntity, new GlobalObjectComponent2()
            {
                Value2 = 12
            });
            
            var archetypeTestHashMap = state.EntityManager.CreateArchetype(stackalloc ComponentType[]
            {
                ComponentType.ReadOnly<SavableObject>(),
                ComponentType.ReadOnly<SaveStateLoaded>(),
                ComponentType.ReadOnly<GlobalTestHashMap>()
            });
            
            globalHashMapEntity = state.EntityManager.CreateEntity(archetypeTestHashMap);

            state.EntityManager.SetComponentEnabled<SaveStateLoaded>(globalHashMapEntity, false);
            
            state.EntityManager.SetComponentData(globalHashMapEntity, new SavableObject()
            {
                SaveId = 301
            });

            var globalTestHashMap = new GlobalTestHashMap();
            globalTestHashMap.Init();
            
            globalTestHashMap.HashMap.Add(100, 'c');
            globalTestHashMap.HashMap.Add(200, 'd');
            globalTestHashMap.HashMap.Add(300, 'e');
            
            state.EntityManager.SetComponentData(globalHashMapEntity, globalTestHashMap);
            
            Debug.Log("Components GlobalTestHashMap and GlobalObjectComponent have been created. Hit save and view GlobalObjects.sav in the viewer!");
        }

        public void OnStopRunning(ref SystemState state)
        {
            var globalTestHashMap = SystemAPI.GetComponent<GlobalTestHashMap>(globalHashMapEntity);
            globalTestHashMap.Dispose();
            
            state.EntityManager.DestroyEntity(globalHashMapEntity);
            state.EntityManager.DestroyEntity(globalObjectEntity);
        }
        
        public void OnUpdate(ref SystemState state)
        {
        }
    }
}