using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class TextDataParsed
{
    public string Original;
    public Dictionary<SystemLanguage, string> m_Dict;
}

[System.Serializable]
public class TextData
{
    public string Original;
    public string English;
    public string ChineseSimplified;
    public string ChineseTraditional;
    public string German;
    public string French;
    public string Japanese;
    public string Korean;
    public string Spanish;
    public string Italian;
}


[System.Serializable]
public class FontData
{
    public Font English;
    public Font ChineseSimplified;
    public Font ChineseTraditional;
    public Font German;
    public Font French;
    public Font Japanese;
    public Font Korean;
    public Font Spanish;
    public Font Italian;
}


[CreateAssetMenu(fileName = "LocalizationData", menuName = "Custom/LocalizationData", order = 1)]
public class LocalizationObject : ScriptableObject {
    public TextData[] data;
    public FontData fontData;
    public SystemLanguage debugLang;

    public Dictionary<string, TextDataParsed> m_TextDict;

    public void ParseLocalizationData()
    {
        var comparer = StringComparer.OrdinalIgnoreCase;
        m_TextDict = new Dictionary<string, TextDataParsed>(comparer);
        foreach (TextData d in data)
        {
            TextDataParsed parse = new TextDataParsed();
            parse.m_Dict = new Dictionary<SystemLanguage, string>();

            var fields = d.GetType().GetFields();
            //print(fields.Length);

            foreach (var f in fields)
            {
                if(f.Name != "Original")
                {
                    SystemLanguage lang = (SystemLanguage)System.Enum.Parse(typeof(SystemLanguage), f.Name);
                    parse.m_Dict.Add(lang, (string)f.GetValue(d));
                }
            }
            m_TextDict.Add(d.Original, parse);
        }

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LocalizationObject))]
public class LocalizationObjectEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LocalizationObject gd = (LocalizationObject)target;
        if (GUILayout.Button("Parse"))
        {
            gd.ParseLocalizationData();
        }
    }
}

#endif
