using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class RecordManager {

    const string DateFormat = "dd-MM-yyyy";

	public static void Record(string key)
	{
		PlayerPrefs.SetInt(key, 1);
		PlayerPrefs.Save();
	}

	public static bool HasRecord(string key)
	{
		return PlayerPrefs.HasKey (key);
	}

    public static void RecordInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
    public static int GetRecordInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public static void RecordDate(string key, DateTime date)
    {
        string dateString = date.ToString(DateFormat);

        PlayerPrefs.SetString(key, dateString);
        PlayerPrefs.Save();
    }

    public static bool HasRecordDate(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static DateTime GetRecordDate(string key)
    {
        Debug.Assert(PlayerPrefs.HasKey(key));
        DateTime date = DateTime.ParseExact(PlayerPrefs.GetString(key), DateFormat, CultureInfo.InvariantCulture);
        return date;
    }
}
