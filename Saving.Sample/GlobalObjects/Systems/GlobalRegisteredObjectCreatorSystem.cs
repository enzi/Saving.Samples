// <copyright project="NZCore" file="GlobalRegisteredObjectCreatorSystem.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using AOT;
using NZCore;
using NZCore.Saving;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public struct GlobalRegisteredObjectComponent : IComponentData, ISavableObject
    {
        public double Time;
        
        public void Init() { }
        public void Dispose() { }
    }
    
    public struct GlobalRegisteredObjectComponent2 : IComponentData, ISavableObject
    {
        public double AnotherTime;
        
        public void Init() { }
        public void Dispose() { }
    }
    
    [BurstCompile]
    [CreateBefore(typeof(SaveGameSystem))]
    [CreateAfter(typeof(DestructionSystem))]
    public partial struct GlobalRegisteredObjectCreatorSystem : ISystem
    {
        private Entity globalObjectEntity;
        private double lastTime;
        
        public void OnCreate(ref SystemState state)
        {
            lastTime = -1.0;
            
            var updateMethod = BurstCompiler.CompileFunctionPointer<OnUpdateSaveState>(SaveRegisteredObject);
            var register = SystemAPI.GetSingleton<RegisterSaveObjectsSingleton>();
            
            // simpler version
            // {
            //     var (entity, tmpQuery) = register.CreateAndRegisterGlobalEntity<GlobalRegisteredObjectComponent>(ref state, "time", updateMethod);
            //     SaveFileSystem.CreateLoadRequest<GlobalRegisteredObjectComponent>(ref state, entity, "time");
            //     state.RequireForUpdate(tmpQuery);
            // }

            {
                NativeList<ComponentType> comps = new NativeList<ComponentType>(2, state.WorldUpdateAllocator);
                comps.Add(ComponentType.ReadOnly<GlobalRegisteredObjectComponent>());
                comps.Add(ComponentType.ReadOnly<GlobalRegisteredObjectComponent2>());

                var (entity, tmpQuery) = register.CreateGlobalEntity(ref state, comps.AsArray());

                register.RegisterGlobalEntity<GlobalRegisteredObjectComponent>(ref state, "time", updateMethod, entity);
                register.RegisterGlobalEntity<GlobalRegisteredObjectComponent2>(ref state, "time", updateMethod, entity);
                
                SaveFileSystem.CreateIndividualLoadRequest(ref state, entity, "time");

                state.RequireForUpdate(tmpQuery);
            }
            
            state.RequireForUpdate<ActivatorGlobalObjects>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            var obj = SystemAPI.GetSingleton<GlobalRegisteredObjectComponent>();
            var obj2 = SystemAPI.GetSingleton<GlobalRegisteredObjectComponent2>();

            if (lastTime < obj.Time)
            {
                Debug.Log($"GlobalRegisteredObjectCreatorSystem time {obj.Time} {obj2.AnotherTime}");
                lastTime = obj.Time;
            }
        }

        // IL2CPP also needs a MonoPInvokeCallback
        [BurstCompile, MonoPInvokeCallback(typeof(OnUpdateSaveState))]
        public static void SaveRegisteredObject(ref SystemState state)
        {
            // this is called before the data is serialized
            // and is useful to not being forced to update the save data all the time
            // as save data and runtime data can be very different
            
            // one such usecase is to write back interface data that lives in its own memory space
            // but is only written back to the save container at this stage

            state.EntityManager.SetSingleton(new GlobalRegisteredObjectComponent()
            {
                Time = state.WorldUnmanaged.Time.ElapsedTime 
            });
            
            state.EntityManager.SetSingleton(new GlobalRegisteredObjectComponent2()
            {
                AnotherTime = state.WorldUnmanaged.Time.ElapsedTime 
            });
        }
    }
}