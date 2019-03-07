using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FloorType
{
    Straight = 0,
    SmoothCurve,
    SteepCurve,
    Jump,
    NarrowWidth,//Wait for more
    TotalRandom
}

public enum ObstacleType
{
    Cube,
    Jump,
    BeforeJump,
    HalfRoad,
    Wall,
    BlackHole,
    MovingCube,
    MovingWall,
    WallLeftCube,
    WallRightCube,
    WallMidCube,
    RandomPlacedCube,
    GlidingStart,
    GlidingEnd,
    AutoPilotEnd,
    AutoPilotStopSign
}

public class ObstacleData
{
    public ObstacleType obstacleType;
    public int obstacleCount;
    public int obstacleStartIndex;
}

public class FloorTypeData
{
    public FloorType floorType;
    public int floorCount;
    public int maxFloorCount;
    public float floorTurningAngle;
    public int coinCount;
    public int coinStartIndex;
    public int coinSlot = -1;
    public ArrayList obstacles;
    public float floorWidth;

    public FloorTypeData()
    {
        obstacles = new ArrayList();
        coinSlot = -1;
    }

    public void AddObstacleByType(ObstacleType type, int count = 1, int startIndex = 0)
    {
        ObstacleData data = new ObstacleData();
        data.obstacleType = type;
        data.obstacleCount = count;
        data.obstacleStartIndex = startIndex;

        obstacles.Add(data);
    }

    public void SetFloorCount(int count)
    {
        floorCount = count;
        maxFloorCount = count;
    }
}


public class FloorBuilder : MonoBehaviour {

    public Vector3 DownHillSlope;

    public int floorMeshCount;
    public GameObject floorMeshPrefab;
    public float floorMeshLifeTime;

    public float m_GlobalWidth;
    public float m_ActualWidth;
    public float m_InitWidth;
    public float m_MinWidth;
    
    public float length;

    public int initialStraightLength;
    public int m_BuildBeforeLength = 50;

    public int collidingIndex;

    public int startIndex;
    public int endIndex;
    
    public float collidedTime;

    public int collidedCount;
    
    public FloorType floorType;
    /* Floor Types:
     * 0: Straight
     * 1: Smooth Curve
     * 2: Steep Curve
     * 3: U Turn
     * 4: Width / 2
     * 5: Total Random
     * 
     */

    public int remainingFloorCount;
    public float floorTurningAngle;

    public static FloorBuilder current;

    public FloorMesh[] floorMeshes;
    
    public float m_LastMakeFloorY;

    FloorTypeData floorTypeData;

    //Should null check when its empty 
    public Queue<FloorTypeData> m_UpcomingFloorDatas;
    public static int m_MaxFloorData = 5;

    HashSet<ObstacleType> m_MadeMeshDict;

    // Use this for initialization
    void Start () {
        
        current = this;
        float downHillDegree = 3.0f;

        DownHillSlope = (new Vector3(0, -Mathf.Sin(downHillDegree* Mathf.Deg2Rad), Mathf.Cos(downHillDegree * Mathf.Deg2Rad))).normalized; //(new Vector3(0, -0.05f, 0.9f)).normalized;
        m_UpcomingFloorDatas = new Queue<FloorTypeData>();
        m_InitWidth = m_GlobalWidth;
        m_ActualWidth = m_GlobalWidth;
        /*m_MadeMeshDict = new HashSet<ObstacleType>();
        m_MadeMeshDict.Add(ObstacleType.Jump);
        m_MadeMeshDict.Add(ObstacleType.HalfRoad);*/

        GameManager.current.state = GameManager.GameState.AssembleTrack;

        floorMeshes = new FloorMesh[floorMeshCount];
        for(int a=0;a!=floorMeshCount;++a)
        {
            GameObject floorObject = (GameObject)Instantiate<GameObject>(floorMeshPrefab);
            floorObject.transform.position = Vector3.zero;
            floorMeshes[a] = floorObject.GetComponent<FloorMesh>();
            floorMeshes[a].width = m_ActualWidth; // width;
            floorMeshes[a].length = length;
            floorMeshes[a].index = a;
            floorMeshes[a].gameObject.AddComponent<FloorColor>();
            floorObject.transform.parent = transform;
        }
        //Initial straight track
        floorTypeData = new FloorTypeData();
        floorTypeData.SetFloorCount(initialStraightLength);
        floorTypeData.floorTurningAngle = 0;
        floorTypeData.floorWidth = m_ActualWidth; // width;
        m_UpcomingFloorDatas.Enqueue(floorTypeData);

        ChallengeManager.current.GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>().StartBehaviour();

        floorMeshes[0].prevPos1 = new Vector3(0, 0, 0);
        floorMeshes[0].prevPos2 = new Vector3(5, 0, 0);
        floorMeshes[0].dir = DownHillSlope;
        floorMeshes[0].prevDir = Vector3.forward;
        floorMeshes[0].makeMesh();
        ChangeFloorColor();

        startIndex = 1;
        endIndex = 0;

        for (int a = 1; a != floorMeshCount; ++a)
        {
            makeFloorByType();
        }
    }

    public void FinishedMakingFloorType(bool success)
    {
    }

    public void RebuildFloor()
    {
        m_GlobalWidth = m_InitWidth;
        m_ActualWidth = m_GlobalWidth;
        CoinGenerator.current.ResetAllCoins();
        m_UpcomingFloorDatas.Clear();

        floorTypeData = new FloorTypeData();
        floorTypeData.SetFloorCount(initialStraightLength);
        floorTypeData.maxFloorCount = floorTypeData.floorCount;
        floorTypeData.floorTurningAngle = 0;
        floorTypeData.floorWidth = m_ActualWidth; // width;

        startIndex = 1;
        endIndex = 0;
        collidingIndex = 0;
        for (int a = 0; a != floorMeshCount; ++a)
        {
            floorMeshes[a].ResetMesh();
        }

        floorMeshes[0].prevPos1 = new Vector3(0, 0, 0);
        floorMeshes[0].prevPos2 = new Vector3(5, 0, 0);
        floorMeshes[0].dir = DownHillSlope;
        floorMeshes[0].prevDir = Vector3.forward;
        floorMeshes[0].makeMesh();
        m_UpcomingFloorDatas.Enqueue(floorTypeData);

        ChallengeManager.current.GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>().StartBehaviour();

        for (int a = 1; a != floorMeshCount; ++a)
        {
            floorMeshes[a].ResetMesh();
            makeFloorByType();
        }
    }
	
	// Update is called once per frame
	void Update () {
		GameManager.current.cldIndex = collidingIndex;
        if (GameManager.current.state == GameManager.GameState.Running || GameManager.current.state == GameManager.GameState.Start)
        {
            if(GameManager.current.state == GameManager.GameState.Running)
            {
                if ((collidingIndex > startIndex && collidingIndex - startIndex > m_BuildBeforeLength) || (collidingIndex < startIndex && collidingIndex + floorMeshCount - startIndex > m_BuildBeforeLength))
                {
                    makeFloorByType();
                }
            }
        }

        foreach (FloorMesh floorMesh in floorMeshes)
        {
            if (floorMesh.isActiveAndEnabled)
            {
                floorMesh.InGameUpdate();
            }
        }
    }

    public void ChangeFloorColor()
    {
        if(floorMeshes != null)
        {
            floorMeshes[0].GetComponent<FloorColor>().DrawColor();
        }
    }

    void AddDefaultFloorType()
    {
        FloorTypeData data = new FloorTypeData();
        data.SetFloorCount(initialStraightLength);
        data.floorTurningAngle = 0;
        data.floorWidth = m_GlobalWidth; // width;
        m_UpcomingFloorDatas.Enqueue(data);
    }
    
    public void meshCollided(int index)
    {
		if (Mathf.Abs(index - collidingIndex) < 90 ) 
		{
			//collidedCount += (index - collidingIndex)>0? (index - collidingIndex): collidingIndex - index;
            GameManager.current.AddScore((index - collidingIndex) > 0 ? (index - collidingIndex) : collidingIndex - index);
        }
		//GameManager.current.gameScore = collidedCount;
		collidingIndex = index;
        collidedTime = 0;
    }

    void MakeFloor(bool success = true)
    {
        m_ActualWidth = m_GlobalWidth;
        FloorMesh fm = floorMeshes[startIndex];

        fm.ResetMesh();
        fm.width = m_ActualWidth; //floorTypeData.floorWidth;
        fm.length = length;
        fm.prevPos1 = floorMeshes[endIndex].endPos1;
        fm.prevPos2 = floorMeshes[endIndex].endPos2;
        fm.prevDir = floorMeshes[endIndex].dir;
        fm.dir = Quaternion.Euler(0, floorTypeData.floorTurningAngle, 0) * fm.prevDir;
        fm.floorTypeData = floorTypeData;

        ItemManager im = ItemManager.current;

        bool hasObstacle = false;
        bool hasMadeMesh = false;
        if (floorTypeData.obstacles != null)
        {
            for (int a = 0; a != floorTypeData.obstacles.Count; ++a)
            {
                ObstacleData obsData = (ObstacleData)floorTypeData.obstacles[a];
                obsData.obstacleStartIndex--;
                if (obsData.obstacleStartIndex == 0 && obsData.obstacleType == ObstacleType.Jump)
                {
                    hasMadeMesh = ObstacleBuilder.current.makeObstacleOnMesh(startIndex, ObstacleType.BeforeJump);
                    hasObstacle = true;
                }
                if (obsData.obstacleStartIndex < 0 && -obsData.obstacleStartIndex <= obsData.obstacleCount)
                {
                    hasMadeMesh = ObstacleBuilder.current.makeObstacleOnMesh(startIndex, obsData.obstacleType);
                    hasObstacle = true;
                }
            }
        }

        if(!hasMadeMesh)
            fm.makeMesh();

        m_LastMakeFloorY = fm.endPos1.y;

        if (im != null && !hasObstacle)  // Make item if no obstacle
        {
            if (im.NextFloorReached())
            {
                im.PutItemOnFloor(fm);
            }
        }

        //Coin
        floorTypeData.coinStartIndex--;
        if (floorTypeData.coinStartIndex < 0 && -floorTypeData.coinStartIndex <= floorTypeData.coinCount && !hasObstacle)
        {
            if(fm.floorTypeData.coinSlot == -1)
            {
                fm.floorTypeData.coinSlot = Random.Range(1, fm.GetMaxSlot());
            }
            CoinGenerator.current.putCoin(startIndex);
        }

        //only in sky
        if(GameManager.current.m_CurrentSceneIndex == 1)
        {
            SkyByRoadPrefab obj = BackgroundManager.current.NextObjectByRoad();
            if (obj != null)
            {
                GameObject skyObj = Instantiate(obj.Prefab);
                float dist = Random.Range(obj.m_Distance.min, obj.m_Distance.max) * (Random.value < 0.5f? -1.0f: 1.0f);
                skyObj.transform.position = (fm.prevPos1 + fm.prevPos2)/2.0f + Vector3.Cross(fm.dir, Vector3.up) * dist;
                skyObj.transform.Rotate(0, Random.Range(0, 360.0f), 0);
                float scale = Random.Range(obj.m_Scale.min, obj.m_Scale.max);
                skyObj.transform.localScale = new Vector3(scale, scale, scale);
                skyObj.GetComponent<ItemSuper>().StartAnim();

                fm.destroyOnRemake.Add(skyObj);
            }
        }

        endIndex = startIndex;
        startIndex = (startIndex + 1) % floorMeshCount;
    }

    void makeFloorByType()
    {
        if (m_UpcomingFloorDatas.Count < m_MaxFloorData)
        {
            ChallengeManager.current.GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>().StartBehaviour();
        }
        if (floorTypeData.floorCount <= 0)
        {
            if(m_UpcomingFloorDatas.Count > 0)
            {
                floorTypeData = m_UpcomingFloorDatas.Dequeue();
            }
            else
            {
                print("shit");
            }
        }
        MakeFloor(true);
        floorTypeData.floorCount--;
    }

    FloorTypeData GetStraightFloorData(int length)
    {
        FloorTypeData data = new FloorTypeData();
        data.SetFloorCount(length);
        data.floorTurningAngle = 0;
        data.floorWidth = m_GlobalWidth; // width;

        return data;
    }

    public void AddGlidingAtEnd()
    {
        m_UpcomingFloorDatas.Clear();

        print("Add Glide!");

        //Straight
        m_UpcomingFloorDatas.Enqueue(GetStraightFloorData(10));

        FloorTypeData start = GetStraightFloorData(1);
        start.AddObstacleByType(ObstacleType.GlidingStart);
        m_UpcomingFloorDatas.Enqueue(start);

        FloorTypeData end = GetStraightFloorData(1);
        end.AddObstacleByType(ObstacleType.GlidingEnd);
        m_UpcomingFloorDatas.Enqueue(end);

        m_UpcomingFloorDatas.Enqueue(GetStraightFloorData(22));
    }

    public void AddAutoPilotEnd()
    {
        FloorTypeData end = GetStraightFloorData(1);
        end.AddObstacleByType(ObstacleType.AutoPilotEnd);
        m_UpcomingFloorDatas.Enqueue(end);

        FloorTypeData straight = GetStraightFloorData(15);
        straight.coinCount = Random.Range(4, 10);
        straight.coinStartIndex = Random.Range(4, 15);
        m_UpcomingFloorDatas.Enqueue(straight);
        
        FloorTypeData stop = GetStraightFloorData(1);
        stop.AddObstacleByType(ObstacleType.AutoPilotStopSign);
        m_UpcomingFloorDatas.Enqueue(stop);

        FloorTypeData ss = GetStraightFloorData(10);
        ss.coinCount = Random.Range(4, 10);
        ss.coinStartIndex = Random.Range(4, 15);
        ss.AddObstacleByType(ObstacleType.Cube, Random.Range(1,4));
        m_UpcomingFloorDatas.Enqueue(ss);
    }

    public int GenerateStraightPath(int length)
    {
        FloorTypeData data = GetStraightFloorData(length);
        int straightstartIndex = (startIndex + 2) % floorMeshCount;

        //m_UpcomingFloorDatas.Clear();
        m_UpcomingFloorDatas.Enqueue(data);

        //ChallengeManager.current.GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>().StartBehaviour();

        return straightstartIndex;
    }

    void getFloorTypeByChallengeManager()
    {
        //floorTypeData = ChallengeManager.current.refreshFloorType(); 
    }

    public void ChangeAllFloorMaterials(Material mat)
    {
        foreach(var fm in floorMeshes)
        {
            fm.GetComponent<MeshRenderer>().material = mat;
            //print("change");
        }
    }

    public void EnableAllFloorMaterials(bool enable)
    {
        foreach (var fm in floorMeshes)
        {
            fm.GetComponent<MeshRenderer>().enabled = enable;
            //print("change");
        }

    }
}
