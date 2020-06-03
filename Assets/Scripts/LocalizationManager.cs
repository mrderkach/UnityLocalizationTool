using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{

    public static LocalizationManager instance;
    public int currentLanguage = 0;

    public LanguageList languageList;

    private readonly string missingTextString = "Localized text not found";

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeLanguage(int language)
    {
        currentLanguage = language;
        LocalizedText[] all_texts = FindObjectsOfType(typeof(LocalizedText)) as LocalizedText[];
        foreach (LocalizedText obj in all_texts)
        {
            obj.UpdateText();
        }
        return;
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        try { 
            result = languageList.languageList[currentLanguage].words[key];
        } catch
        {
            Debug.Log(missingTextString);
        }

        return result;

    }
}