// <copyright project="Saving.Sample" file="MultipleSaveGamesSystem.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using NZCore.Saving;
using NZCore.UIToolkit;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Saving.Sample
{
    public partial class MultipleSaveGamesSystem : SystemBase
    {
        private UIHelper<MultipleSaveGamesViewModel, MultipleSaveGamesViewModel.Data> ui;
        private CharacterItem currentSelection;

        protected override void OnCreate()
        {
            RequireForUpdate<ActivatorMultipleSaveGames>();
            RequireForUpdate<UIAssetsLoaded>();
        }

        protected override void OnStartRunning()
        {
            ui = new UIHelper<MultipleSaveGamesViewModel, MultipleSaveGamesViewModel.Data>("main-menu", "main-menu");
            var element = ui.LoadPanel();

            // shows how to get the savefile with the newest date 
            // var newestFile = SaveFileSystem.GetNewestSaveFile();
            // if (newestFile != null)
            // {
            //     Debug.Log($"Newest file is {newestFile}");
            // }

            List<CharacterItem> characterList = new List<CharacterItem>(); 
            var allSaves = SaveFileSystem.GetAllSaveFiles();
            
            foreach (var save in allSaves)
            {
                if (save.Contains("AutoSave"))
                {
                    // skip loading AutoSave, however, this might be the easiest way to support a "Continue" option in the main menu
                    continue; 
                }

                if (!SaveFileSystem.TryLoadManaged(save, out var bytes))
                {
                    continue;
                }

                // create the container where the linear bytes are split into header and sections
                SaveDataContainerSingleton saveDataContainer = new SaveDataContainerSingleton();
                saveDataContainer.Init();
                    
                // process the linear binary file into sections
                SaveManager.DeserializeSaveDataContainer(bytes, ref saveDataContainer);

                // query the saveDataContainer to find a specific type that was saved
                var levels = SaveManager.FindData<SimpleCharacterLevel>(ref saveDataContainer);
                var names = SaveManager.FindData<SimpleCharacterName>(ref saveDataContainer);

                if (levels.Length > 0 && levels.Length == names.Length)
                {
                    characterList.Add(new CharacterItem()
                    {
                        Level = levels[0],
                        Name = names[0],
                        SavePath = save
                    });
                }
                    
                saveDataContainer.Dispose();
            }
            
            var listView = element.Q<ListView>();
            var assets = SystemAPI.ManagedAPI.GetSingleton<UIAssetsSingleton>();
            
            listView.makeItem += () =>
            {
                var item = assets.VisualTreeAssets["character-list-item"].CloneTree();

                return item;
            };
            
            listView.bindItem += (visualElement, i) =>
            {
                var data = (CharacterItem) listView.itemsSource[i];
                
                var value1Field = visualElement.Q<Label>("character-name");
                var value2Field = visualElement.Q<Label>("character-level");
                
                value1Field.text = $"Name: {data.Name.CharacterName.ToString()}";
                value2Field.text = $"Level: {data.Level.Level.ToString()}";
            };

            listView.itemsChosen += ListViewOnItemsChosen;
            listView.selectionChanged += ListViewOnSelectionChanged;

            listView.itemsSource = characterList;

            var loadButton = element.Q<Button>("btn-load");
            loadButton.clicked += ClickLoadButton;
        }
        
        protected override void OnStopRunning()
        {
            ui.Unload();
        }
        
        private void ListViewOnSelectionChanged(IEnumerable<object> obj)
        {
            var element = obj.FirstOrDefault();
            
            if (element is CharacterItem item)
            {
                currentSelection = item;
            }
        }

        private void ListViewOnItemsChosen(IEnumerable<object> obj)
        {
            var element = obj.FirstOrDefault();
            
            if (element is CharacterItem item)
            {
                LoadSave(item.SavePath);    
            }
        }

        public void ClickLoadButton()
        {
            if (currentSelection != null)
            {
                LoadSave(currentSelection.SavePath);
            }
        }

        private void LoadSave(string savePath)
        {
            // unload the "main menu"
            var activatorEntity = SystemAPI.GetSingletonEntity<ActivatorMultipleSaveGames>();
            EntityManager.DestroyEntity(activatorEntity);

            // set the current path, this will not just be our load path
            // but also serves to set the path when a new save is written
            // if this is not set and no explicit path was given when saving
            // it will default to SaveGames/AutoSave!
            SystemAPI.SetSingleton(new CurrentPathSingleton()
            {
                CurrentPath = savePath
            });
            
            var requests = SystemAPI.GetSingleton<SaveFileSystemRequestSingleton>();
            requests.AddLoadRequest(new LoadRequest());
            
            // that's it
            // we don't need to load the game subscene because
            // it's stored in the savegame and will be automatically loaded
        }

        protected override void OnUpdate()
        {
            if (ui.Model.CreateCharacter)
            {
                Debug.Log($"Create character with name {ui.Model.CharacterName}");

                // get a save path defined by the character name (this would need some error checking)
                // we could use another filename just as easily, even a random guid
                var savePath = SaveFileSystem.GetDefaultSavePath(ui.Model.CharacterName);
                
                // set the CurrentPathSingleton which controls globally where the file
                // is saved. this can still be overriden by setting paths or filenames in requests.
                SystemAPI.SetSingleton(new CurrentPathSingleton()
                {
                    CurrentPath = savePath
                });
                
                // unload the "main menu"
                var activatorEntity = SystemAPI.GetSingletonEntity<ActivatorMultipleSaveGames>();
                EntityManager.DestroyEntity(activatorEntity);
                
                // setup character init data in a very simple way
                var createEntity = EntityManager.CreateEntity();
                EntityManager.AddComponentData(createEntity, new CharacterCreateData()
                {
                    Level = new SimpleCharacterLevel() { Level = 1 },
                    Name = new SimpleCharacterName() { CharacterName = ui.Model.CharacterName }
                });

                // load the "game"
                var sceneReference = SystemAPI.GetSingleton<GameSceneReference>();
                SceneSystem.LoadSceneAsync(World.Unmanaged, sceneReference.SceneReference);
            }
            
            ui.Model.Clear();
        }
    }

    public class CharacterItem
    {
        public SimpleCharacterLevel Level;
        public SimpleCharacterName Name;
        public string SavePath;
    }
}