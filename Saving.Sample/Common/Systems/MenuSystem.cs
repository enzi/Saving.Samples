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
    public partial struct MenuSystem : ISystem, ISystemStartStop
    {
        private UIHelper<MenuViewModel, MenuViewModel.Data> ui;
        private SubSceneUtility subSceneUtility;
        
        // prevents that save is executed when the scene is closed
        // while this is possible, a savegame with no open subscenes will be stored
        // so when loaded, no subscene will be loaded
        // for a prototype like this demo scene, this behaviour is not wanted
        private bool isClosed;
        
        public void OnCreate(ref SystemState state)
        {
            isClosed = false;
            
            subSceneUtility = new SubSceneUtility(ref state);
            state.RequireForUpdate<UIAssetsLoaded>();
            state.RequireForUpdate<ActivatorLoadSaveInterface>();
        }

        public void OnStartRunning(ref SystemState state)
        {
            ui = new UIHelper<MenuViewModel, MenuViewModel.Data>("menu", "menu");
            ui.LoadPanel();
        }

        public void OnStopRunning(ref SystemState state)
        {
            ui.Unload();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            if ((ui.Model.ManualSave || Input.GetKey(KeyCode.S)) && !isClosed)
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

                isClosed = false;
            }

            if (ui.Model.ManualClose || Input.GetKey(KeyCode.C))
            {
                //Debug.Log("Manual Close");

                isClosed = true;
                var openScenes = SubSceneHelper.GetAllOpenSubScenesWithIgnoreFilter(ref state, subSceneUtility);

                foreach (var openScene in openScenes)
                {
                    state.EntityManager.DestroyAndUnloadSubscene(openScene, SceneSystem.UnloadParameters.Default);    
                }
            }
            
            ui.Model.Clear();
        }
    }
}