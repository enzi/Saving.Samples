// <copyright project="Saving.Sample" file="ChangeCubeMenuSystem.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore.UIToolkit;
using Saving.Sample;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace SaveSystem_Samples.Saving.Sample.SharedComponents.Systems
{
    public partial struct ChangeCubeMenuSystem : ISystem, ISystemStartStop
    {
        private EntityQuery cubesQuery;
        
        private UIHelper<ChangeCubeViewModel, ChangeCubeViewModel.Data> ui;

        public void OnCreate(ref SystemState state)
        {
            cubesQuery = SystemAPI.QueryBuilder()
                .WithAll<ColoredCube>()
                .Build();
            
            state.RequireForUpdate<UIAssetsLoaded>();
            state.RequireForUpdate<ActivatorSharedComponents>();
        }

        public void OnStartRunning(ref SystemState state)
        {
            ui = new UIHelper<ChangeCubeViewModel, ChangeCubeViewModel.Data>("changecube", "changecube");
            ui.LoadPanel();
        }

        public void OnStopRunning(ref SystemState state)
        {
            ui.Unload();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (ui.Model.ChangeCube)
            {
                var cubes = cubesQuery.ToEntityArray(Allocator.Temp);
                Random random = new Random(234234 + (uint) SystemAPI.Time.ElapsedTime);

                foreach (var cube in cubes)
                {
                    var newGroupId = random.NextInt(0, 3);
                   
                    state.EntityManager.SetSharedComponent(cube, new TestSharedComponent()
                    {
                        GroupIndex = newGroupId
                    });

                    var lt = SystemAPI.GetComponent<LocalTransform>(cube);
                    lt.Position.x = newGroupId * 5 - 5;
                    
                    SystemAPI.SetComponent(cube, lt);
                }
            }
            
            ui.Model.Clear();
        }
    }
}