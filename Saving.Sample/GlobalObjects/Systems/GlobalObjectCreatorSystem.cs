// <copyright project="NZCore" file="GlobalObjectCreatorSystem.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore;
using NZCore.Saving;
using Unity.Entities;

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
    
    public partial struct GlobalObjectCreatorSystem : ISystem
    {
        private Entity globalObjectEntity;
        
        public void OnCreate(ref SystemState state)
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
        }
        
        public void OnUpdate(ref SystemState state)
        {
        }
    }
}