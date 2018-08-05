using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager current;

    public LocalizationObject m_Storer;
    public bool m_UseDebugLang = false;
    const string m_ObjectLocation = "ScriptableObjects/LocalizationData";
    const string languageKey = "LangKey";

    public Dictionary<string, TextDataParsed> m_TextDict;


    SystemLanguage m_CurrentLanguage = SystemLanguage.English;
    public SystemLanguage CurrentLanguage
    {
        set
        {
            m_CurrentLanguage = value;
            RecordManager.RecordInt(languageKey, (int)m_CurrentLanguage);
            print(m_CurrentLanguage);
        }
        get
        {
            return m_CurrentLanguage;
        }
    }

    // Use this for initialization
    void Awake ()
    {
        current = this;
        m_Storer = (LocalizationObject)Resources.Load(m_ObjectLocation);

        if (m_UseDebugLang)
            CurrentLanguage = m_Storer.debugLang;
        else
        {
            if(RecordManager.HasRecord(languageKey))
            {
                CurrentLanguage = (SystemLanguage)RecordManager.GetRecordInt(languageKey);
            }
            else
            {
                CurrentLanguage = Application.systemLanguage;
            }
        }
        ParseLocalizationData();
    }

    public void ParseLocalizationData()
    {
        /*var data = m_Storer.data;
        var comparer = StringComparer.OrdinalIgnoreCase;
        m_TextDict = new Dictionary<string, TextDataParsed>(comparer);
        foreach (TextData d in data)
        {
            TextDataParsed parse = new TextDataParsed();
            parse.m_Dict = new Dictionary<SystemLanguage, string>();
            
            var fields = d.GetType().GetFields();

            foreach (var f in fields)
            {
                if (f.Name != "Original")
                {
                    SystemLanguage lang = (SystemLanguage)System.Enum.Parse(typeof(SystemLanguage), f.Name);
                    parse.m_Dict.Add(lang, (string)f.GetValue(d));
                }
            }

            m_TextDict.Add(d.Original, parse);
        }*/

        var comparer = StringComparer.OrdinalIgnoreCase;
        m_TextDict = new Dictionary<string, TextDataParsed>(comparer);
        string text = m_Storer.textFile.text;
        string[] line = text.Split('\n');

        List<SystemLanguage> languages = new List<SystemLanguage>();

        string[] line1 = line[0].Split(',');
        for (int i = 1; i < line1.Length; ++i)
        {
            string lang = line1[i];
            SystemLanguage language = (SystemLanguage)System.Enum.Parse(typeof(SystemLanguage), lang);
            languages.Add(language);
        }

        for (int i = 1; i < line.Length; ++i)
        {
            string[] lineWord = line[i].Split(',');
            if (m_TextDict.ContainsKey(lineWord[0]))
            {
                Debug.LogWarning(lineWord[0] + ": Localization key duplicated.");
                continue;
            }

            TextDataParsed parse = new TextDataParsed();
            parse.m_Dict = new Dictionary<SystemLanguage, string>();
            parse.Original = lineWord[0];

            for (int j = 1; j < lineWord.Length; ++j)
            {
                parse.m_Dict[languages[j - 1]] = lineWord[j];
            }

            m_TextDict.Add(parse.Original, parse);
        }
    }

    public static string tr(string original)
    {
        return current.GetLocalString(original);
    }

    public string GetLocalString(string original)
    {
        if(m_TextDict.ContainsKey(original))
        {
            TextDataParsed data = m_TextDict[original];
            if(data.m_Dict.ContainsKey(CurrentLanguage))
            {
                return data.m_Dict[CurrentLanguage];
            }
        }

        return original;
    }

    public Font GetFont()
    {
        string lang = CurrentLanguage.ToString();
        Font font = (Font) m_Storer.fontData.GetType().GetField(lang).GetValue(m_Storer.fontData);
        return font;
    }
}
