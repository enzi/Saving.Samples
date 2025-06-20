// <copyright project="Saving.Sample" file="SpawnCubeViewModel.cs">
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
    public class ChangeCubeViewModel : IViewModelBindingNotify<ChangeCubeViewModel.Data>
    {
        private Data data;
        public ref Data Value => ref data;

        [CreateProperty]
        public bool ChangeCube
        {
            get => data.ChangeCube;
            set => data.ChangeCube = value;
        }

        public struct Data : IModelBindingNotify
        {
            public bool ChangeCube;

            public void Clear()
            {
                ChangeCube = false;
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