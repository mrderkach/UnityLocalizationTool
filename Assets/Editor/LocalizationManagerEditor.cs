using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizationManager))]
public class LocalizationManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        LocalizationManager locScript = (LocalizationManager)target;
        locScript.languageList = (LanguageList)EditorGUILayout.ObjectField("Language List",
            locScript.languageList, typeof(LanguageList), false);
        int n = locScript.languageList.languageList.Count;
        string[] options = new string[n];
        for (int i = 0; i < n; ++i)
        {
            options[i] = locScript.languageList.languageList[i].itemName;
        }
        locScript.currentLanguage = EditorGUILayout.Popup("Current Language", 
            locScript.currentLanguage, options);
    }
}

