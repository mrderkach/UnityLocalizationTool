using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class SearchKey : EditorWindow
{
    private AutocompleteSearchField autocompleteSearchField;
    public LanguageList languageListObj;
    public int viewIndex = 1;
    public float width;
    public string new_key;
    public LocalizedText inst;

    void OnEnable()
    {
        if (autocompleteSearchField == null) autocompleteSearchField = new AutocompleteSearchField();
        autocompleteSearchField.onInputChanged = OnInputChanged;
        autocompleteSearchField.onConfirm = OnConfirm;

    }

    void OnGUI()
    {
        GUILayout.Label("Search key");
        autocompleteSearchField.OnGUI();
    }

    void OnInputChanged(string searchString)
    {
        autocompleteSearchField.ClearResults();
        if (searchString != null)
        {
            foreach (var entity in languageListObj.languageList[viewIndex - 1].words)
            {
                if (entity.Key.StartsWith(searchString))
                {
                    autocompleteSearchField.AddResult(entity.Key);
                }
            }
        }
    }

    void OnConfirm(string searchString)
    {
        inst.key = searchString;
        this.Close();
    }
}