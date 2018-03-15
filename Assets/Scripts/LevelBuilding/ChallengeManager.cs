using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using UnityEngine;
using System.IO;
using System.Text;

public class ChallengeManager : MonoBehaviour {

    public static ChallengeManager current;
    
    public int currentDifficulty = 0;
    
    public FloorDataBeforeScore[] floorData;
        
    public float startTime;
    public float getHardTimeRemain;
    public float widthDecreasePerHundredScore;
    float m_LastWidthDecreaseScore = 0;

    public static void SetFloorTypeData(FloorTypeData data)
    {
        if(data != null && current != null)
        {
            current.GetDifficulty(); // refresh difficulty

            FloorBuilder.current.m_UpcomingFloorDatas.Enqueue(data);
        }
    }

    // Use this for initialization
    void Start () {
        current = this;
		ItemManager itemManager = new ItemManager ();
    }

    public bool ShouldGenerateNewFloorType()
    {
        //print(FloorBuilder.current.m_UpcomingFloorDatas.Count);
        return FloorBuilder.current != null && FloorBuilder.current.m_UpcomingFloorDatas.Count < FloorBuilder.m_MaxFloorData;
    }

    public int GetDifficulty()
    {
        if (currentDifficulty < floorData.Length - 1 && GameManager.current.scoreForDifficulty > floorData[currentDifficulty + 1].activateScore)
        {
            currentDifficulty++;
            GameManager.current.m_DiffMultiplier = floorData[currentDifficulty].multiplier;
            GameManager.current.AddNormalTimeScale(floorData[currentDifficulty].m_Timescale - 1.0f);

            if (ChallengeManager.current.floorData[currentDifficulty].useGlideTransition)
                FloorBuilder.current.AddGlidingAtEnd(); 
        }

        return currentDifficulty;
    }

    public void AddDifficultyByLevel()
    {
        currentDifficulty++;
    }

    public bool ShouldRedCoin()
    {
        return Random.value < floorData[currentDifficulty].redCoinChance;
    }

    void addDifficultyByTime()
    {
        if (FloorBuilder.current.m_GlobalWidth > FloorBuilder.current.m_MinWidth)
        {
            //FloorBuilder.current.m_GlobalWidth -= 1.0f;
            Time.timeScale += 0.1f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.current && GameManager.current.state == GameManager.GameState.Running)
        {
            if(GameManager.current.scoreForDifficulty - 100.0f > m_LastWidthDecreaseScore)
            {
                FloorBuilder.current.m_GlobalWidth = FloorBuilder.current.m_InitWidth - (widthDecreasePerHundredScore/100.0f * (GameManager.current.scoreForDifficulty));
                if (FloorBuilder.current.m_GlobalWidth < FloorBuilder.current.m_MinWidth)
                    FloorBuilder.current.m_GlobalWidth = FloorBuilder.current.m_MinWidth;
                m_LastWidthDecreaseScore = GameManager.current.scoreForDifficulty;
            }
            
            /*getHardTimeRemain -= Time.deltaTime;
            if (getHardTimeRemain <= 0)
            {
                addDifficultyByTime();
                getHardTimeRemain = 40.0f;
            }*/
        }
    }


    /*public void WriteToXML()
    {
        print("Write to xml");
        StringWriter Output = new StringWriter(new StringBuilder());
        XmlSerializer xs = new XmlSerializer(floorData.GetType());
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        //ns.Add("Difficulty");
        xs.Serialize(Output, floorData, ns);
        File.WriteAllText("Assets/Resources/XML/Difficulty.xml", Output.ToString(), Encoding.UTF8);
    }

    public void ReadFromXML()
    {
        print("Read from xml");
        FloorDataConfig config = new FloorDataConfig();
        config.Init();
        floorData = FloorDataConfig.data;
    }*/
    /*
	public FloorType randomByPresetChance() {
		float randomRange = 100;
		float randomNumber = Random.Range(0,randomRange);
		float baseValue = 0;

        //float[] challengeChances = floorData[currentFloorData].challengeChance;

        for (int i = 0; i < floorData[currentDifficulty].data.Length; i++)
        {
            float chance = baseValue + floorData[currentDifficulty].data[i].chance * randomRange;
            if (randomNumber <= chance) {
				return (FloorType)i;
			}
			baseValue = baseValue + (floorData[currentDifficulty].data[i].chance * randomRange);
		}
		//end

		int k=0;
		FloorType res = (FloorType)k;
		return res;
	}
    */
    /*public int randomIntByPresetChance()
    {
        float randomRange = 100;
        float randomNumber = Random.Range(0, randomRange);
        float baseValue = 0;

        //float[] challengeChances = floorData[currentFloorData].challengeChance;

        for (int i = 0; i < floorData[currentDifficulty].data.Length; i++)
        {
            float chance = floorData[currentDifficulty].data[i].chance * randomRange;
            if (randomNumber <= chance)
            {
                return i;
            }
            //baseValue = baseValue + (floorData[currentFloorData].data[i].chance * randomRange);
        }
        //end

        return 0;
    }*/
    /*public FloorTypeData refreshFloorType()
    {
        if(currentDifficulty < floorData.Length -1 &&  GameManager.current.gameScore > floorData[currentDifficulty+1].activateScore)
        {
            currentDifficulty++;
            GameManager.current.m_GlobalMultiplier = floorData[currentDifficulty].multiplier;
        }
        FloorTypeData data = new FloorTypeData();
        if (remainingChallenges <= 0)
        {
            //Rest by straight
            remainingChallenges = challengesPerRest;

            data.floorCount = 20;
            data.coinCount = Random.Range(0, data.floorCount);
            data.floorTurningAngle = 0;
            data.floorWidth = 15;
            //data.obstacleCount = 0;
            return data;
        }

        //Generate new floor type
        int floorType = randomIntByPresetChance();
        data.obstacles = new ArrayList();
        ObstacleData obstacleData = new ObstacleData();

        FloorDataInEditor editorData = floorData[currentDifficulty].data[floorType];
        data.floorCount = Random.Range(editorData.minFloorCount, editorData.maxFloorCount);
		data.coinCount = Random.Range(editorData.minCoinCount, editorData.maxCoinCount);
        data.coinStartIndex = Random.Range(0, data.floorCount - data.coinCount);
        data.floorTurningAngle = (Random.Range(0.0f, 1.0f) > 0.5f ? 1 : -1) * Random.Range(editorData.minFloorTurningAngle, editorData.maxFloorTurningAngle);
        data.floorWidth = editorData.floorWidth;

        if(editorData.obstacleType == ObstacleType.Jump)
        {
            obstacleData.obstacleType = ObstacleType.Jump;
            obstacleData.obstacleCount = 1;
            obstacleData.obstacleStartIndex = 1;//Random.Range(1, data.floorCount - obstacleData.obstacleCount - 1);
            data.obstacles.Add(obstacleData);
            remainingChallenges -= 2;
        }
        else
        {
            obstacleData.obstacleType = ObstacleType.Cube;
            obstacleData.obstacleCount = Random.Range(editorData.minObstacleCount, editorData.maxObstacleCount);
            obstacleData.obstacleStartIndex = Random.Range(0, data.floorCount - obstacleData.obstacleCount);
            data.obstacles.Add(obstacleData);
            remainingChallenges -= obstacleData.obstacleCount / 3;
        }

        return data;
    }*/
}
