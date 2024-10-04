// <copyright project="NZCore" file="EnableableMenuSystem.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore.UIToolkit;
using Saving.Sample.Data;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Saving.Sample
{
    public partial struct EnableableMenuSystem : ISystem, ISystemStartStop
    {
        private UIHelper<EnableableSampleMenuViewModel, EnableableSampleMenuViewModel.Data> ui;
        private EntityQuery cubesQuery;

        public void OnCreate(ref SystemState state)
        {
            cubesQuery = SystemAPI.QueryBuilder()
                .WithAll<EnableableCubeTag>()
                .Build();
            
            state.RequireForUpdate<UIAssetsLoaded>();
            state.RequireForUpdate<ActivatorEnableableComponents>();
        }

        public void OnStartRunning(ref SystemState state)
        {
            ui = new UIHelper<EnableableSampleMenuViewModel, EnableableSampleMenuViewModel.Data>("enableable-menu", "enableable-menu");
            ui.LoadPanel();
        }

        public void OnStopRunning(ref SystemState state)
        {
            ui.Unload();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (ui.Model.Toggle)
            {
                Debug.Log("Toggle");
                
                var cubes = cubesQuery.ToEntityArray(Allocator.Temp);
                Random random = new Random(234234 + (uint) SystemAPI.Time.ElapsedTime);

                foreach (var cube in cubes)
                {
                    if (random.NextBool())
                    {
                        continue;
                    }
                    
                    var isEnabled = SystemAPI.IsComponentEnabled<SavableEnableableComp>(cube);
                    SystemAPI.SetComponentEnabled<SavableEnableableComp>(cube, !isEnabled);
                }
            }
            
            ui.Model.Clear();
        }
    }
}