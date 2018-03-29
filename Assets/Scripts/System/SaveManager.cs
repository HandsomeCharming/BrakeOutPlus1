using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class CarSave
{
    List<CarSaveData> m_Cars;
    Dictionary<string, CarSaveData> m_CarDict;

    public CarSelectData Storer
    {
        get
        {
            if(m_Storer == null)
            {
                m_Storer = (CarSelectData)Resources.Load(m_DataPath);
            }
            return m_Storer;
        }
        set
        {}
    }
    CarSelectData m_Storer;

    const string m_DataPath = "ScriptableObjects/CarSelectData";

    public CarSave()
    {
        m_Cars = new List<CarSaveData>();
        m_CarDict = new Dictionary<string, CarSaveData>();
    }

    public void BuyCar(int carIndex, int sceneIndex)
    {
        CarSaveData first = new CarSaveData();
        first.m_CarIndex = carIndex;
        first.m_SceneIndex = sceneIndex;
        first.m_Name = Storer.GetCarData(carIndex, sceneIndex).name;

        m_Cars.Add(first);
        m_CarDict.Add(first.m_Name, first);
    }

    public bool HasCar(string name)
    {
        return m_CarDict.ContainsKey(name);
    }
}

[System.Serializable]
public class GameSave
{
    public int coin;
    public int star;
    public int highScore;
    public CarSave m_Cars;
    public string version;

    public GameSave()
    {
        coin = 0;
        star = 0;
        highScore = 0;

        // init first car save
        m_Cars = new CarSave();
        m_Cars.BuyCar(0, 0);
    }
}

public class SaveManager {
    public static SaveManager instance;

    public static SaveManager GetInstance()
    {
        if(instance == null)
        {
            instance = new SaveManager();
        }
        return instance;
    }

    public GameSave m_Data;

    public void Save()
    {
        Debug.Log("save");
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        System.IO.FileInfo filepath = new System.IO.FileInfo(Application.persistentDataPath);
        filepath.Directory.Create();

		string path = Application.persistentDataPath + "/savedGames.gd";
		FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want

		m_Data.coin = GameManager.current.GetCoinCount();
		m_Data.star = GameManager.current.gameStars;
		m_Data.highScore = GameManager.current.gameHighScore;
		bf.Serialize(file, m_Data);
		file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			string path = Application.persistentDataPath + "/savedGames.gd";
			FileStream file = File.Open(path, FileMode.Open);
            try
			{
                m_Data = (GameSave)bf.Deserialize(file);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                m_Data = new GameSave();
			}
			file.Close();

            if(m_Data.m_Cars == null)
            {
                m_Data.m_Cars = new CarSave();
                m_Data.m_Cars.BuyCar(0, 0);
            }
        }
        else
        {
            Debug.Log("Load new ");
            m_Data = new GameSave();
        }
    }

    public bool HasCar(string name)
    {
        return m_Data.m_Cars.HasCar(name);
    }

    public void BuyCar(string name)
    {

    }
}
