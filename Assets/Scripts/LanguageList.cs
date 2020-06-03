using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefaultLanguageList", menuName = "LanguageList")]
public class LanguageList : ScriptableObject
{
    public List<LanguageObject> languageList;
}