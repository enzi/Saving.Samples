// <copyright project="Saving.Sample" file="SimpleGameSystem.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore.Saving;
using NZCore.UIToolkit;
using Unity.Collections;
using Unity.Entities;

namespace Saving.Sample
{
    public partial struct SimpleGameSystem: ISystem, ISystemStartStop
    {
        private UIHelper<SimpleGameViewModel, SimpleGameViewModel.Data> ui;

        private EntityQuery createQuery;
        private EntityQuery saveStateLoadedQuery;

        public void OnCreate(ref SystemState state)
        {
            createQuery = SystemAPI.QueryBuilder()
                .WithAll<CharacterCreateData>()
                .Build();
            
            // query to update the interface when save data has been applied
            saveStateLoadedQuery = SystemAPI.QueryBuilder()
                .WithAll<SimpleCharacterName, SimpleCharacterLevel>()
                .WithPresent<SaveStateLoaded>()
                .Build();
            
            saveStateLoadedQuery.AddChangedVersionFilter(ComponentType.ReadOnly<SaveStateLoaded>());
            
            state.RequireForUpdate<ActivatorGame>();
            state.RequireForUpdate<UIAssetsLoaded>();
        }
        
        public void OnStartRunning(ref SystemState state)
        {
            ui = new UIHelper<SimpleGameViewModel, SimpleGameViewModel.Data>("simple-game", "simple-game");
            ui.LoadPanel();

            if (!createQuery.IsEmpty)
            {
                var createEntities = createQuery.ToEntityArray(Allocator.Temp);

                foreach (var createEntity in createEntities)
                {
                    var createData = SystemAPI.GetComponent<CharacterCreateData>(createEntity);
                    ui.Model.CharacterName = createData.Name.CharacterName;
                    ui.Model.CharacterLevel = createData.Level.Level;
                    
                    SystemAPI.SetSingleton(createData.Level);
                    SystemAPI.SetSingleton(createData.Name);
                }
                
                state.EntityManager.DestroyEntity(createQuery);
            }
        }

        public void OnStopRunning(ref SystemState state)
        {
            ui.Unload();
        }

        public void OnUpdate(ref SystemState state)
        {
            // for some reason SystemAPI.Query doesn't work. need to investigate why
            //foreach (var (levelRO, nameRO) in SystemAPI.Query<RefRO<SimpleCharacterLevel>, RefRO<SimpleCharacterName>>().WithChangeFilter<SaveStateLoaded>())
            if (!saveStateLoadedQuery.IsEmpty)
            {
                var name = SystemAPI.GetSingleton<SimpleCharacterName>();
                var level = SystemAPI.GetSingleton<SimpleCharacterLevel>();
                
                ui.Model.CharacterName = name.CharacterName;
                ui.Model.CharacterLevel = level.Level;
            }
            
            if (ui.Model.GainLevelButton)
            {
                var level = SystemAPI.GetSingleton<SimpleCharacterLevel>();
                level.Level += 1;
                
                SystemAPI.SetSingleton(level);
                ui.Model.CharacterLevel = level.Level;
            }

            ui.Model.Clear();
        }
    }
}