// <copyright project="NZCore" file="MenuSystem.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore;
using NZCore.Saving;
using NZCore.UIToolkit;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

namespace Saving.Sample
{
    public partial struct GameSaveLoadMenuSystem : ISystem, ISystemStartStop
    {
        private UIHelper<GameSaveLoadViewModel, GameSaveLoadViewModel.Data> ui;
        private SubSceneUtility subSceneUtility;
        
        public void OnCreate(ref SystemState state)
        {
            subSceneUtility = new SubSceneUtility(ref state);
            state.RequireForUpdate<UIAssetsLoaded>();
            state.RequireForUpdate<ActivatorGame>();
        }

        public void OnStartRunning(ref SystemState state)
        {
            ui = new UIHelper<GameSaveLoadViewModel, GameSaveLoadViewModel.Data>("game-save-load-menu", "game-save-load-menu");
            ui.LoadPanel();
        }

        public void OnStopRunning(ref SystemState state)
        {
            ui.Unload();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            if (ui.Model.ManualSave || Input.GetKey(KeyCode.S))
            {
                //Debug.Log("Manual Save");
                var requests = SystemAPI.GetSingleton<SaveSystemRequestSingleton>();
                requests.AddManualSave();
            }

            if (ui.Model.ManualLoad || Input.GetKey(KeyCode.L))
            {
                //Debug.Log("Manual Load");
                var fileRequests = SystemAPI.GetSingleton<SaveFileSystemRequestSingleton>();
                fileRequests.AddLoadRequest(new LoadRequest());
            }

            if (ui.Model.GotoMainMenu || Input.GetKey(KeyCode.C))
            {
                //Debug.Log("Manual Close");
                
                // unload the "game"
                var sceneReference = SystemAPI.GetSingleton<GameSceneReference>();
                SceneSystem.UnloadScene(state.WorldUnmanaged, sceneReference.SceneHash);

                // reactivate the main menu
                var menuEntity = state.EntityManager.CreateEntity();
                state.EntityManager.AddComponentData(menuEntity, new ActivatorMultipleSaveGames());
            }
            
            ui.Model.Clear();
        }
    }
}