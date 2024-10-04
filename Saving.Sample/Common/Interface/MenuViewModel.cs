// <copyright project="NZCore" file="MenuViewModel.cs" version="0.1">
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
    public class MenuViewModel : IViewModelBindingNotify<MenuViewModel.Data>
    {
        private Data data;
        public ref Data Value => ref data;
        
        [CreateProperty]
        public bool ManualSave
        {
            get => data.ManualSave;
            set => data.ManualSave = value;
        }
        
        [CreateProperty]
        public bool ManualLoad
        {
            get => data.ManualLoad;
            set => data.ManualLoad = value;
        }
        
        [CreateProperty]
        public bool ManualClose
        {
            get => data.ManualClose;
            set => data.ManualClose = value;
        }

        public struct Data : IModelBindingNotify
        {
            public bool ManualSave;
            public bool ManualLoad;
            public bool ManualClose;

            public void Clear()
            {
                ManualLoad = false;
                ManualSave = false;
                ManualClose = false;
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