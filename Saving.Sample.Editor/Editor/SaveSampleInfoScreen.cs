// <copyright project="NZCore.Saving" file="SampleInfoScreen.cs" version="1.0">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore.Editor;
using UnityEditor;
using UnityEngine;

namespace Saving.Sample.Editor
{
    public class SaveSampleInfoScreen : EditorWindow
    {
        private const string PREF_KEY = "SaveSampleInfoScreen";
        
        [InitializeOnLoadMethod]
        private static void ShowWelcomeScreen()
        {
            if (!EditorPrefs.GetBool(PREF_KEY, false))
            {
                EditorApplication.delayCall += () =>
                {
                    SaveSampleInfoScreen window = GetWindow<SaveSampleInfoScreen>("Welcome");
                    window.minSize = new Vector2(500, 450);
                    window.Show();
                };
            }
        }
        
        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Welcome to the Samples for \"Saving System for DOTS\"!", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Important Information:", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Before starting any scene, it is important to generate the `Settings` files.");
            EditorGUILayout.LabelField("Otherwise no data will be saved!", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("To do so:");
            EditorGUILayout.LabelField("- navigate to `SaveSystem Samples\\Save Formats`");
            EditorGUILayout.LabelField("- click on any of the scriptable objects");
            EditorGUILayout.LabelField("- click `Update every JSON setting`");
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("OR for convenience");

            if (GUILayout.Button("Generate Settings"))
            {
                ChangeProcessorEditorElement.Click_CodeGenAll();
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("The crucial part out of the way.");
            EditorGUILayout.LabelField("Thank you very much for buying this asset!", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("I hope you like it and if you have any feedback for me, please send me an email.");
            EditorGUILayout.LabelField("Also consider writing a review after some time. Thanks!");
            EditorGUILayout.Space(10);
            
            EditorGUILayout.LabelField("Useful Resources:", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);
            
            if (GUILayout.Button("Open Documentation"))
            {
                Application.OpenURL("https://saving.enzenebner.com/docs");
            }
            if (GUILayout.Button("Visit Discord"))
            {
                Application.OpenURL("https://discord.gg/rdDXJQBGnc");
            }

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Close and Don't Show Again"))
            {
                EditorPrefs.SetBool(PREF_KEY, true);
                Close();
            }
        }
    }
}