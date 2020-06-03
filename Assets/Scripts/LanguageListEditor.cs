using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class LanguageEditor : EditorWindow
{

    public LanguageList languageListObj;
    private AutocompleteSearchField autocompleteSearchField;
    private int mode = 0;
    private int viewIndex = 1;
    private bool confirmDelete = false;
    private bool confirmDeleteE = false;
    private Vector2 scrollPosition = Vector2.zero,
        scrollPosition0 = Vector2.zero;
    private string new_key = "", new_value = "";

    [MenuItem("Window/Language Editor %#e")]
    static void Init()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(LanguageEditor));
        window.Show();
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            languageListObj = AssetDatabase.LoadAssetAtPath(objectPath, typeof(LanguageList)) as LanguageList;
        }
        if (autocompleteSearchField == null) autocompleteSearchField = new AutocompleteSearchField();
        autocompleteSearchField.onInputChanged = OnInputChanged;
        autocompleteSearchField.onConfirm = OnConfirm;

    }

    void OnGUI()
    {
        scrollPosition0 = GUILayout.BeginScrollView(
scrollPosition0);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Languages Editor", EditorStyles.boldLabel);
        if (languageListObj != null)
        {
            if (GUILayout.Button("Show Language List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = languageListObj;
            }
        }
        if (GUILayout.Button("Open Language List"))
        {
            OpenItemList();
        }
        GUILayout.EndHorizontal();

        if (languageListObj == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Open Existing Item List", GUILayout.ExpandWidth(false)))
            {
                OpenItemList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (languageListObj != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                confirmDelete = false;
                confirmDeleteE = false;
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                confirmDelete = false;
                confirmDeleteE = false;
                if (viewIndex < languageListObj.languageList.Count)
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Language", GUILayout.ExpandWidth(false)))
            {
                AddItem();
                confirmDelete = false;
                confirmDeleteE = false;
            }
            if (confirmDelete)
            {
                if (GUILayout.Button("Confirm delete", GUILayout.ExpandWidth(false)))
                {
                    confirmDelete = false;
                    confirmDeleteE = false;
                    DeleteItem(viewIndex - 1);
                }
            }
            else
            {
                if (GUILayout.Button("Delete Language", GUILayout.ExpandWidth(false)))
                {
                    confirmDelete = true;
                    confirmDeleteE = false;
                }
            }
            GUILayout.EndHorizontal();

            if (languageListObj.languageList == null)
                Debug.Log("Gone wrong");
            if (languageListObj.languageList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", viewIndex, GUILayout.ExpandWidth(false)), 1, languageListObj.languageList.Count);
                EditorGUILayout.LabelField("of   " + languageListObj.languageList.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();
                GUILayout.Space(10);

                languageListObj.languageList[viewIndex - 1].itemName = EditorGUILayout.TextField("Language Name", languageListObj.languageList[viewIndex - 1].itemName as string);

                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Inspect", GUILayout.ExpandWidth(false)))
                {
                    mode = 1;
                    confirmDeleteE = false;
                    confirmDelete = false;
                }
                if (GUILayout.Button("Edit", GUILayout.ExpandWidth(false)))
                {
                    mode = 0;
                    confirmDeleteE = false;
                    confirmDelete = false;
                }
                GUILayout.EndHorizontal();

                // Sync Languages
                //for (int i = 1; i < languageListObj.languageList.Count; ++i)
                //{
                //    foreach (var entity in languageListObj.languageList[0].words)
                //    {
                //        try
                //        {
                //            languageListObj.languageList[i].words.Add(entity.Key, "");
                //        }
                //        catch { }
                //    }
                //}

                if (mode == 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Change entities:", EditorStyles.boldLabel);
                    if (GUILayout.Button("+", GUILayout.ExpandWidth(false)) && new_key != null)
                    {
                        confirmDelete = false;
                        confirmDeleteE = false;
                        for (int i = 0; i < languageListObj.languageList.Count; ++i)
                        {
                            try
                            {
                                languageListObj.languageList[i].words.Add(new_key, new_value);
                            }
                            catch
                            {
                                languageListObj.languageList[viewIndex - 1].words[new_key] = new_value;
                            }
                        }
                    }
                    if (confirmDeleteE)
                    {
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button(
                            "Yes, I want to delete this key from ALL languages permanently", 
                            GUILayout.ExpandWidth(true)))
                        {
                            confirmDeleteE = false;
                            confirmDelete = false;
                            for (int i = 0; i < languageListObj.languageList.Count; ++i)
                            {
                                try
                                {
                                    languageListObj.languageList[i].words.Remove(new_key);
                                }
                                catch { }
                            }
                            new_key = "";
                            new_value = "";
                            autocompleteSearchField.searchString = "";
                        }
                    } else
                    {
                        if (GUILayout.Button("x", GUILayout.ExpandWidth(false)))
                        {
                            confirmDeleteE = true;
                            confirmDelete = false;
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Key:");
                    new_key = EditorGUILayout.TextArea(new_key as string, GUILayout.Width(200));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Value:");
                    new_value = EditorGUILayout.TextArea(new_value as string, GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    GUILayout.Label("Search key");
                    autocompleteSearchField.OnGUI();

                    GUILayout.Space(20);
                }
                else if (mode == 1)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Language dictionary", EditorStyles.boldLabel);
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    scrollPosition = GUILayout.BeginScrollView(
                    scrollPosition);
                    if (languageListObj.languageList[viewIndex - 1].words.Count == 0)
                    {
                        GUILayout.Label("No words");
                    }
                    else
                    {

                        List<string> keys = new List<string>();
                        foreach (KeyValuePair<string, string> p in languageListObj.languageList[viewIndex - 1].words)
                        {
                            keys.Add(p.Key);
                        }

                        keys.Sort();
                        foreach (string key in keys)
                        {
                            string value = languageListObj.languageList[viewIndex - 1].words[key];
                            GUILayout.Label(string.Format("{0}: {1}", key, value));
                        }
                    }
                    GUILayout.EndScrollView();

                    GUILayout.Space(10);
                }
            }
            else
            {
                GUILayout.Label("This Language List is Empty.");
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(languageListObj);
        }
        GUILayout.EndScrollView();
    }

    void OpenItemList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Language List", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            languageListObj = AssetDatabase.LoadAssetAtPath(relPath, typeof(LanguageList)) as LanguageList;
            if (languageListObj.languageList == null)
                languageListObj.languageList = new List<LanguageObject>();
            if (languageListObj)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddItem()
    {
        LanguageObject newItem = new LanguageObject();
        languageListObj.languageList.Add(newItem);
        viewIndex = languageListObj.languageList.Count;

        bool repeats = true;
        int k = 0;
        while (repeats)
        {
            for (int i = 0; i < languageListObj.languageList.Count - 1; ++i)
            {
                if (languageListObj.languageList[i].itemName ==
                    languageListObj.languageList[viewIndex - 1].itemName + k.ToString())
                {
                    ++k;
                    continue;
                }
            }
            languageListObj.languageList[viewIndex - 1].itemName =
                languageListObj.languageList[viewIndex - 1].itemName + k.ToString();
            repeats = false;
        }

        foreach (var entity in languageListObj.languageList[0].words)
        {
            languageListObj.languageList[viewIndex-1].words.Add(entity.Key, "");
        }
    }

    void DeleteItem(int index)
    {
        languageListObj.languageList.RemoveAt(index);
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

    void OnConfirm(string result)
    {
        new_key = result;
        new_value =
            languageListObj.languageList[viewIndex - 1].words[result];
    }
}