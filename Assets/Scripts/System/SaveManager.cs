using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class CarSave
{
    public List<CarSaveData> m_Cars;
    
    public CarSave()
    {
        m_Cars = new List<CarSaveData>();
    }
}

[System.Serializable]
public class TrailSave
{
    public List<string> m_TrailNames;

    public TrailSave()
    {
        m_TrailNames = new List<string>();
    }
}

[System.Serializable]
public class GameSave
{
    public int coin;
    public int star;
    public int highScore;
    public CarSave m_Cars;
    public TrailSave m_Trails;
    public string version;

    public GameSave()
    {
        coin = 0;
        star = 0;
        highScore = 0;

        // init first car save
        m_Cars = new CarSave();
        m_Trails = new TrailSave();
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

    // Purchased cars
    Dictionary<string, CarSaveData> m_CarDict;

    public CarSelectData CarStorer
    {
        get
        {
            if (m_CarStorer == null)
            {
                m_CarStorer = (CarSelectData)Resources.Load(m_DataPath);
            }
            return m_CarStorer;
        }
        set
        { }
    }
    CarSelectData m_CarStorer;

    const string m_DataPath = "ScriptableObjects/CarSelectData";

    public SaveManager()
    {
        m_CarDict = new Dictionary<string, CarSaveData>();
    }

    public void Save()
    {
        Debug.Log("save");
        BinaryFormatter bf = new BinaryFormatter();
        
        System.IO.FileInfo filepath = new System.IO.FileInfo(Application.persistentDataPath);
        filepath.Directory.Create();

		string path = Application.persistentDataPath + "/savedGames.gd";
		FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");

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
                Debug.LogError(e);
                m_Data = new GameSave();
			}
			file.Close();
            MaybeBackfillDefaultSceneCarSaveData();

            if (m_Data.m_Cars == null)
            {
                m_Data.m_Cars = new CarSave();
                AcquireCar(0, 0);
            }
            else
            {
                foreach (var car in m_Data.m_Cars.m_Cars)
                {
                    if(!m_CarDict.ContainsKey(car.m_Name))
                    {
                        m_CarDict.Add(car.m_Name, car);
                    }
                }
            }

            if (m_Data.m_Trails == null)
            {
                m_Data.m_Trails = new TrailSave();
                AcquireTrail("Line");
                AcquireTrail("LineSky");
            }
        }
        else
        {
            Debug.Log("Load new ");
            m_Data = new GameSave();
            GetDefaultCarAndTrails();
        }
    }

    private void MaybeBackfillDefaultSceneCarSaveData() {
        var carData = m_Data.m_Cars;
        var carIndexesToBackfill = new List<int>();
        foreach(var car in carData.m_Cars) {
            if(car.m_SceneIndex != 0) {
                bool shouldBackfill = true;
                foreach(var c in carData.m_Cars) {
                    if(c.m_CarIndex == car.m_CarIndex  && c.m_SceneIndex == 0) {
                        shouldBackfill = false;
                        break;
                    }
                }
                if(shouldBackfill) {
                    carIndexesToBackfill.Add(car.m_CarIndex);
                }
            }
        }

        foreach(var carIndex in carIndexesToBackfill) {
            AcquireCar(carIndex, 0);
        }
        if(carIndexesToBackfill.Count > 0) {
            Debug.Log("Backfill failed");
        }
    }

    public void GetDefaultCarAndTrails()
    {
        AcquireCar(0, 0);
        AcquireCar(0, 1);
        AcquireTrail("LINE");
        AcquireTrail("CLOUD");
        AcquireTrail("NONE");
    }

    public int GetCarPriceCoin(SingleCarSelectData carData) {
        if (m_Data == null) {
            Debug.LogError("Get price failed: save data is null");
        }
        var coinPrice = carData.coinPrice;

        // If not first scene and doesn't have car
        if (carData.sceneIndex > 0 && !HasCar(carData.carIndex, 0)) {
            var firstSceneCarData = CarSelectDataReader.Instance.GetCarData(carData.carIndex, 0);
            coinPrice += firstSceneCarData.coinPrice;
        }
        return coinPrice;
    }

    public int GetCarPriceStar(SingleCarSelectData carData) {
        if (m_Data == null) {
            Debug.LogError("Get price failed: save data is null");
        }
        var starPrice = carData.starPrice;

        // If not first scene and doesn't have car
        if (carData.sceneIndex > 0 && !HasCar(carData.carIndex, 0)) {
            var firstSceneCarData = CarSelectDataReader.Instance.GetCarData(carData.carIndex, 0);
            starPrice += firstSceneCarData.starPrice;
        }
        return starPrice;
    }

    public int GetCarPriceCoin(int carIndex, int sceneIndex) {
        return GetCarPriceCoin(CarSelectDataReader.Instance.GetCarData(carIndex, sceneIndex));
    }

    public int GetCarPriceStar(int carIndex, int sceneIndex) {
        return GetCarPriceStar(CarSelectDataReader.Instance.GetCarData(carIndex, sceneIndex));
    }

    public bool BuyCarWithCoin(int carIndex, int sceneIndex)
    {
        int price = GetCarPriceCoin(carIndex, sceneIndex);
        if (GameManager.current.gameCoins >= price)
        {
            GameManager.current.AddCoin(-price);
            AcquireCar(carIndex, sceneIndex);

            // get default scene car
            if (!HasCar(carIndex, 0)) {
                AcquireCar(carIndex, 0);
            }

            return true;
        }
        return false;
    }


    public bool BuyCarWithStar(int carIndex, int sceneIndex)
    {
        int price = GetCarPriceStar(carIndex, sceneIndex);
        if (GameManager.current.gameStars >= price)
        {
            GameManager.current.AddStar(-price);
            AcquireCar(carIndex, sceneIndex);

            // get default scene car
            if(!HasCar(carIndex, 0)) {
                AcquireCar(carIndex, 0);
            }

            return true;
        }
        return false;
    }


    public void AcquireCar(int carIndex, int sceneIndex)
    {
        CarSaveData first = new CarSaveData();
        first.m_CarIndex = carIndex;
        first.m_SceneIndex = sceneIndex;
        first.m_Name = CarStorer.GetCarData(carIndex, sceneIndex).name;

        if (HasCar(first.m_Name)) return;

        m_Data.m_Cars.m_Cars.Add(first);
        m_CarDict.Add(first.m_Name, first);

        Save();
    }
    
    public bool BuyTrail(string name)
    {
        int price = CarSelectDataReader.Instance.GetTrailSelectData(name).price;
        if (GameManager.current.gameStars >= price)
        {
            if (!m_Data.m_Trails.m_TrailNames.Contains(name))
            {
                GameManager.current.AddStar(-price);
                m_Data.m_Trails.m_TrailNames.Add(name);

                Save();
            }
            return true;
        }
        return false;
    }

    public void AcquireTrail(string name)
    {
        if (!m_Data.m_Trails.m_TrailNames.Contains(name))
        {
            m_Data.m_Trails.m_TrailNames.Add(name);

            Save();
        }
    }

    public bool BuyCarUpgrade(string carName, CarUpgradeCatagory type)
    {
        if (HasCar(carName))
        {
            CarSaveData realData = GetSavedCarDataForLevel(carName);
            SingleCarSelectData carData = CarSelectDataReader.Instance.GetCarData(carName);
            bool success = false;

            switch (type)
            {
                case CarUpgradeCatagory.Accelerate:
                    if (realData.m_AccLevel < carData.maxUpgradeLevel &&
                        GameManager.current.gameCoins >= carData.GetUpgradePrice(realData.m_AccLevel, type))
                    {
                        GameManager.current.AddCoin(-carData.GetUpgradePrice(realData.m_AccLevel, type));
                        realData.m_AccLevel++;
                        success = true;
                    }
                    break;
                case CarUpgradeCatagory.Boost:
                    if (realData.m_BoostLevel < carData.maxUpgradeLevel &&
                        GameManager.current.gameCoins >= carData.GetUpgradePrice(realData.m_BoostLevel, type))
                    {
                        GameManager.current.AddCoin(-carData.GetUpgradePrice(realData.m_BoostLevel, type));
                        realData.m_BoostLevel++;
                        success = true;
                    }
                    break;
                case CarUpgradeCatagory.Handling:
                    if (realData.m_HandlingLevel < carData.maxUpgradeLevel &&
                        GameManager.current.gameCoins >= carData.GetUpgradePrice(realData.m_HandlingLevel, type))
                    {
                        GameManager.current.AddCoin(-carData.GetUpgradePrice(realData.m_HandlingLevel, type));
                        realData.m_HandlingLevel++;
                        success = true;
                    }
                    break;
            }
            if (success)
            {
                Save();
                QuestManager.UpdateQuestsStatic(QuestAction.UpgradeCar);
                if ((type == CarUpgradeCatagory.Accelerate && realData.m_AccLevel == carData.maxUpgradeLevel)
                    || (type == CarUpgradeCatagory.Boost && realData.m_BoostLevel == carData.maxUpgradeLevel)
                    || (type == CarUpgradeCatagory.Handling && realData.m_HandlingLevel == carData.maxUpgradeLevel))
                {
                    QuestManager.UpdateQuestsStatic(QuestAction.UpgradeCarToMax);
                }
                return true;
            }
        }
        return false;
    }

    public bool HasCar(string name)
    {
        return m_CarDict.ContainsKey(name);
    }

    public bool HasCar(int carIndex, int sceneIndex) {
        foreach(var car in m_Data.m_Cars.m_Cars) {
            if(car.m_CarIndex == carIndex && car.m_SceneIndex == sceneIndex) {
                return true;
            }
        }
        return false;
    }

    public bool HasTrail(string name)
    {
        return m_Data.m_Trails.m_TrailNames.Contains(name);
    }

    public CarSaveData GetSavedCarData(string name)
    {
        if (m_CarDict.ContainsKey(name))
            return m_CarDict[name];
        else
            return null;
    }

    public CarSaveData GetSavedCarDataForLevel(string name) {
        if (m_CarDict.ContainsKey(name)) {
            var data = m_CarDict[name];
            return GetSavedCarDataForLevel(data.m_CarIndex);
        }
        else
            return null;
    }

    // Gets saved scar data in first scene
    public CarSaveData GetSavedCarDataForLevel(int carIndex) {
        foreach(var car in m_Data.m_Cars.m_Cars) {
            if(car.m_CarIndex == carIndex && car.m_SceneIndex == 0) {
                return car;
            }
        }
        return null;
    }
}
