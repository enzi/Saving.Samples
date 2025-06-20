// <copyright project="Saving.Sample" file="MetaDataWriterSystem.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>


using AOT;
using NZCore;
using NZCore.Saving;
#if UNITY_EDITOR
using NZCore.Saving.Authoring.Editor;
#endif
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

namespace Saving.Sample
{
    [BurstCompile]
    [CreateBefore(typeof(SaveGameSystem))]
    [CreateAfter(typeof(DestructionSystem))]
    public partial struct MetaDataWriterTestSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            var metaDataMethod = BurstCompiler.CompileFunctionPointer<OnSerializeMetaData>(OnSerializeMetaData);
            var requests = SystemAPI.GetSingleton<SaveSystemRequestSingleton>();

            requests.RegisterMetaDataSerializer(new MetaDataSerializer()
            {
                OnSerializeMetaData = metaDataMethod
            });

            var filePath = SaveFileSystem.GetDefaultSavePath("SavableSubScene");
            if (SaveFileSystem.TryReadMetaData(filePath, out TestMetaData data))
            {
                Debug.Log($"reading meta data: {data.Text}");
            }
        }

        [BurstCompile, MonoPInvokeCallback(typeof(OnSerializeMetaData))]
        public static void OnSerializeMetaData(ref SystemState state, ref ByteSerializer serializer)
        {
            Debug.Log("Writing meta data");
            serializer.Add(new TestMetaData()
            {
                Text = "test text"
            });
        }

        public struct TestMetaData
        {
            public FixedString128Bytes Text;
        }
    }
    
#if UNITY_EDITOR
    public class TestMetaDataViewer : IMetaDataDebugView
    {
        public void DebugView(VisualElement root, ByteDeserializer deserializer)
        {
            var data = deserializer.Read<MetaDataWriterTestSystem.TestMetaData>();
            
            root.Add(new Label(data.Text.ToString()));
        }
    }
#endif
}