using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizedText))]
public class LocalizedTextEditor : Editor
{
    private LocalizedText locScript;
    public LocalizationManager locManagerObj;

    void OnEnable()
    {
        locManagerObj = (LocalizationManager)FindObjectOfType(typeof(LocalizationManager));
    }

    public override void OnInspectorGUI()
    {
        locScript = (LocalizedText)target;
        locScript.key = EditorGUILayout.TextField("Key", locScript.key as string);

        if (GUILayout.Button("Search Key", GUILayout.ExpandWidth(false)))
        {
            SearchKey editorWindow = EditorWindow.GetWindow<SearchKey>();
            editorWindow.inst = locScript;
            editorWindow.languageListObj = locManagerObj.languageList;
            editorWindow.viewIndex = locManagerObj.currentLanguage;
        }
    }
}

