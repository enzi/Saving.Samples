// <copyright project="NZCore" file="EnableableSampleMenuViewModel.cs" version="0.1">
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
    public class EnableableSampleMenuViewModel: IViewModelBindingNotify<EnableableSampleMenuViewModel.Data>
    {
        private Data data;
        public ref Data Value => ref data;

        [CreateProperty]
        public bool Toggle
        {
            get => data.Toggle;
            set => data.Toggle = value;
        }

        public struct Data : IModelBindingNotify
        {
            public bool Toggle;
            
            public void Clear()
            {
                Toggle = false;
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