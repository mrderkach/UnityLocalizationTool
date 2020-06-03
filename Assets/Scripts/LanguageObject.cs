using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyDictionary : SerializableDictionary<string, string> { }

[System.Serializable]
public class LanguageObject
{
    public string itemName = "New Language";
    public MyDictionary words = new MyDictionary();
}