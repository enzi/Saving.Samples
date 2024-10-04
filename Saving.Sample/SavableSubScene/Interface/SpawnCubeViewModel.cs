// <copyright project="NZCore" file="SpawnCubeViewModel.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using System;
using BovineLabs.Core.UI;
using Unity.Burst;
using Unity.Collections;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Saving.Sample
{
    public class SpawnCubeViewModel : IViewModelBindingNotify<SpawnCubeViewModel.Data>
    {
        private Data data;
        public ref Data Value => ref data;

        [CreateProperty]
        public bool SpawnCube
        {
            get => data.SpawnCube;
            set => data.SpawnCube = value;
        }
        
        [CreateProperty]
        public bool CreateSpawner
        {
            get => data.CreateSpawner;
            set => data.CreateSpawner = value;
        }
        
        [CreateProperty]
        public bool DestroySpawner
        {
            get => data.DestroySpawner;
            set => data.DestroySpawner = value;
            
        }
        
        [CreateProperty]
        public int SpawnedObjects
        {
            get => data.SpawnedObjects;
            set => data.SpawnedObjects = value;
        }
        
        [CreateProperty]
        public int SpawnerAmount
        {
            get => data.SpawnerAmount;
            set => data.SpawnerAmount = value;
        }

        public struct Data : IModelBindingNotify
        {
            public bool SpawnCube;
            public bool CreateSpawner;
            public bool DestroySpawner;
            private int spawnedObjects;
            private int spawnerAmount;

            public int SpawnedObjects
            {
                get => spawnedObjects;
                set
                {
                    if (spawnedObjects == value)
                        return;
                    
                    spawnedObjects = value;
                    this.Notify();
                }
            }
            
            public int SpawnerAmount
            {
                get => spawnerAmount;
                set
                {
                    if (spawnerAmount == value)
                        return;
                    
                    spawnerAmount = value;
                    this.Notify();
                }
            }

            public void Clear()
            {
                SpawnCube = false;
                CreateSpawner = false;
                DestroySpawner = false;
            }
            
            public FunctionPointer<OnPropertyChangedDelegate> Notify { get; set; }
        }
        
        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;
        public void OnPropertyChanged(in FixedString64Bytes property)
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property.ToString()));
        }
    }
}