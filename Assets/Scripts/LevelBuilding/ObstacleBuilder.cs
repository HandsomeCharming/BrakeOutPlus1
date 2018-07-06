using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBuilder : MonoBehaviour {

    public static ObstacleBuilder current;
    public ObstacleDataObject m_Storer;

    GameObject CubePrefab; //future: change to array
    GameObject WallPrefab; //future: change to array
    GameObject BlackholePrefab;
    GameObject MovingCubePrefab;
    GameObject MovingWallPrefab;
    GameObject WallLeftCubePrefab; //future: change to array
    GameObject WallRightCubePrefab; //future: change to array
    GameObject WallMidCubePrefab; //future: change to array
    GameObject GlidingTriggerPrefab;
    GameObject AutoPilotTriggerPrefab;

    const float WallWidth = 10.0f;
    
    public const string Cube = "Cube";
    public const string Wall = "Wall";
    public const string Blackhole = "Blackhole";
    public const string MovingCube = "MovingCube";
    public const string MovingWall = "MovingWall";
    public const string WallLeftCube = "WallLeftCube";
    public const string WallRightCube = "WallRightCube";
    public const string WallMidCube = "WallMidCube";
    public const string GlidingTrigger = "GlidingTrigger";
    public const string JumpTrigger = "JumpTrigger";
    public const string BeforeJumpTrigger = "BeforeJumpTrigger";
    public const string AutoPilotTrigger = "AutoPilotTrigger";

    public const string BoostGroundSign = "BoostGroundSign";
    public const string BoostSign = "BoostSign";
    public const string GlideSign = "GlideSign";
    public const string StopSign = "StopSign";

    private void Awake()
    {
        current = this;

        CubePrefab = Resources.Load("Prefabs/Obstacles/CubeObstacle") as GameObject;
        WallPrefab = Resources.Load("Prefabs/Obstacles/WallObstacle") as GameObject;
        BlackholePrefab = Resources.Load("Prefabs/Obstacles/NewBlackHoleSuck") as GameObject;
        MovingCubePrefab = Resources.Load("Prefabs/Obstacles/MovingCubeObstacle") as GameObject;
        MovingWallPrefab = Resources.Load("Prefabs/Obstacles/MovingWallObstacle") as GameObject;
        WallLeftCubePrefab = Resources.Load("Prefabs/Obstacles/WallObstacleLeftCube") as GameObject;
        WallRightCubePrefab = Resources.Load("Prefabs/Obstacles/WallObstacleRightCube") as GameObject;
        WallMidCubePrefab = Resources.Load("Prefabs/Obstacles/WallObstacleMidCube") as GameObject;
        GlidingTriggerPrefab = Resources.Load("Prefabs/Obstacles/GlideTrigger") as GameObject;
        AutoPilotTriggerPrefab = Resources.Load("Prefabs/Obstacles/AutoPilotTrigger") as GameObject;
    }

    void Start () {

    }
	
    public void makeObstacleOnMesh(int meshIndex)
    {
        GameObject obstacle = Instantiate(CubePrefab) as GameObject;
        FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
        Vector3 cross = Vector3.Cross(floorMesh.prevDir, floorMesh.dir);
        float posScale = cross.y > 0 ? 0.8f : 0.2f;
        Vector3 prevPosMid = floorMesh.prevPos1 + (floorMesh.prevPos2 - floorMesh.prevPos1) * posScale;
        prevPosMid += floorMesh.dir * floorMesh.length / 2.0f;
        prevPosMid.y += 1.0f;
        obstacle.transform.position = prevPosMid;
        obstacle.transform.forward = floorMesh.prevDir;

        floorMesh.destroyOnRemake.Add(obstacle);
    }

    //Return has made mesh
    public bool makeObstacleOnMesh(int meshIndex, ObstacleType obstacleType)
    {
        //print(obstacleType);
        bool madeMesh = false;
        int sceneIndex = GameManager.current.m_CurrentSceneIndex;
        switch(obstacleType)
        {
            case ObstacleType.Cube:
                {
                    GameObject obstacle = Instantiate(ObstacleDataReader.GetObstaclePrefab(Cube));
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    float posScale = 0.8f;
                    if (floorMesh.prevDir == floorMesh.dir)
                    {
                        posScale = Random.value < 0.5f ? 0.2f : 0.8f;
                    }
                    else
                    {
                        Vector3 cross = Vector3.Cross(floorMesh.prevDir, floorMesh.dir);
                        posScale = cross.y > 0 ? 0.8f : 0.2f;
                    }
                    Vector3 prevPosMid = floorMesh.prevPos1 + (floorMesh.prevPos2 - floorMesh.prevPos1) * posScale;
                    prevPosMid += floorMesh.dir * floorMesh.length / 2.0f;
                    prevPosMid.y += 1.0f;
                    obstacle.transform.position = prevPosMid;
                    obstacle.transform.forward = floorMesh.prevDir;
                    obstacle.GetComponent<CubeObstacle>().StartAnim();

                    floorMesh.destroyOnRemake.Add(obstacle);
                    break;
                }
            case ObstacleType.Jump:
                {
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    floorMesh.prevPos1 = floorMesh.prevPos1 + floorMesh.dir * m_Storer.m_JumpDistance; //floorMesh.length * 6.2f;
                    floorMesh.prevPos2 = floorMesh.prevPos2 + floorMesh.dir * m_Storer.m_JumpDistance; // * floorMesh.length * 6.2f;
                    floorMesh.prevPos1.y -= m_Storer.m_JumpHeight; //6.5f;
                    floorMesh.prevPos2.y -= m_Storer.m_JumpHeight;  //6.5f;

                    floorMesh.makeMesh();

                    //Jump trigger
                    float scale = Vector3.Distance(floorMesh.prevPos1, floorMesh.prevPos2);
                    GameObject trigger = Instantiate(ObstacleDataReader.GetObstaclePrefab(JumpTrigger));
                    Vector3 endPosMid = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
                    trigger.transform.localScale = new Vector3(scale, 5, 1);
                    trigger.transform.position = endPosMid;
                    trigger.transform.forward = floorMesh.prevDir;

                    floorMesh.destroyOnRemake.Add(trigger);

                    //rim
                    Vector3 end1 = floorMesh.prevPos1;
                    Vector3 end2 = floorMesh.prevPos2;
                    GameObject rim = Instantiate(m_Storer.m_RimPrefab, null);
                    rim.transform.position = (end1 + end2) / 2.0f;
                    rim.transform.localScale = new Vector3(0.3f, 0.3f, (end1 - end2).magnitude);
                    rim.transform.forward = (end1 - end2).normalized;
                    rim.GetComponent<ItemSuper>().StartAnim();

                    //rim.transform.parent = floorMesh.transform;
                    floorMesh.destroyOnRemake.Add(rim);

                    madeMesh = true;
                    break;
                }
            case ObstacleType.BeforeJump:
                {
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    int signIndex = meshIndex - 1;
                    if (signIndex < 0) signIndex = FloorBuilder.current.floorMeshes.Length - 1;
                    FloorMesh signMesh = FloorBuilder.current.floorMeshes[signIndex];

                    //add boost sign
                    /*GameObject sign = Instantiate(m_Storer.BoostGroundSignPrefab); ;

                    Vector3 prevPosMid = (signMesh.prevPos1 + signMesh.prevPos2) / 2.0f;
                    prevPosMid.y += 0.1f;
                    sign.transform.position = prevPosMid;
                    sign.transform.forward = signMesh.prevDir;
                    sign.transform.Rotate(90.0f, 0, 0);
                    sign.GetComponent<ItemSuper>().StartAnim();
                    floorMesh.destroyOnRemake.Add(sign);*/

                    // make mesh
                    Vector3 dir = floorMesh.dir;
                    dir.z = -dir.z;
                    floorMesh.makeMesh();

                    // add rim
                    Vector3 end1 = floorMesh.endPos1;
                    Vector3 end2 = floorMesh.endPos2;
                    GameObject rim = Instantiate(m_Storer.m_RimPrefab, null);
                    rim.transform.position = (end1 + end2) / 2.0f;
                    rim.transform.localScale = new Vector3(0.3f, 0.3f, (end1 - end2).magnitude);
                    rim.transform.forward = (end1 - end2).normalized;
                    rim.GetComponent<ItemSuper>().StartAnim();
                    floorMesh.destroyOnRemake.Add(rim);

                    // add sign
                    GameObject signpost1 = Instantiate(ObstacleDataReader.GetObstaclePrefab(BoostSign), null);
                    signpost1.transform.position = Vector3.Lerp(end1, end2, 0.8f);
                    signpost1.transform.forward = signMesh.prevDir;
                    signpost1.GetComponent<ItemSuper>().StartAnim();
                    floorMesh.destroyOnRemake.Add(signpost1);


                    //Before Jump tutorial trigger
                    float scale = Vector3.Distance(floorMesh.prevPos1, floorMesh.prevPos2);
                    GameObject trigger = Instantiate(ObstacleDataReader.GetObstaclePrefab(BeforeJumpTrigger));
                    Vector3 endPosMid = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
                    endPosMid += (-floorMesh.prevDir) * 20.0f;
                    trigger.transform.localScale = new Vector3(scale, 5, 1);
                    trigger.transform.position = endPosMid;
                    trigger.transform.forward = floorMesh.prevDir;

                    floorMesh.destroyOnRemake.Add(trigger);


                    /*GameObject signpost2 = Instantiate(m_Storer.m_BoostSignPrefab, null);
                    signpost2.transform.position = end2;
                    signpost2.transform.forward = signMesh.prevDir;
                    signpost2.GetComponent<ItemSuper>().StartAnim();
                    floorMesh.destroyOnRemake.Add(signpost2);*/

                    madeMesh = true;
                    break;
                }
            case ObstacleType.HalfRoad:
                {
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    /*bool left = Random.value < 0.5f;
                    Vector3 mid = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
                    if (left)
                        floorMesh.prevPos1 = mid;
                    else
                        floorMesh.prevPos2 = mid;*/
                    floorMesh.width = FloorBuilder.current.m_GlobalWidth / 2.0f;
                    floorMesh.makeMesh();
                    madeMesh = true;
                    break;
                }
            case ObstacleType.Wall:
                {
                    GameObject obstacle = Instantiate(ObstacleDataReader.GetObstaclePrefab(Wall)) as GameObject;
                    MakeWall(obstacle, meshIndex);
                    break;
                }
            case ObstacleType.BlackHole:
                {
                    GameObject obstacle = Instantiate(ObstacleDataReader.GetObstaclePrefab(Blackhole)) as GameObject;
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    Vector3 cross = Vector3.Cross(floorMesh.prevDir, floorMesh.dir);
                    bool left = cross.y > 0;
                    int sign = left ? -1 : 1;

                    Vector3 prevPosMid = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
                    prevPosMid += Vector3.Cross(floorMesh.dir, Vector3.up) * (sign * m_Storer.m_BlackHoleDistance);
                    prevPosMid.y += 0.7f;
                    obstacle.transform.position = prevPosMid;
                    obstacle.transform.forward = floorMesh.prevDir;
                    obstacle.GetComponent<Obstacle>().StartAnim();

                    floorMesh.destroyOnRemake.Add(obstacle);
                    break;
                }
            case ObstacleType.MovingCube:
                {
                    print("make cube");
                    GameObject obstacle = Instantiate<GameObject>(ObstacleDataReader.GetObstaclePrefab(MovingCube)) as GameObject;
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    Vector3 cross = Vector3.Cross(floorMesh.prevDir, floorMesh.dir);

                    Vector3 prevPosMid = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
                    prevPosMid.y += 1.0f;
                    Vector3 pos1 = prevPosMid + Vector3.Cross(floorMesh.dir, Vector3.up) * m_Storer.m_MovingCubeMoveDistance;
                    Vector3 pos2 = prevPosMid - Vector3.Cross(floorMesh.dir, Vector3.up) * m_Storer.m_MovingCubeMoveDistance;
                    obstacle.transform.position = prevPosMid;
                    obstacle.transform.forward = floorMesh.prevDir;
                    obstacle.GetComponent<MovingCubeObstacle>().StartAnim();
                    obstacle.GetComponent<MovingCubeObstacle>().movePos = new Vector3[2];
                    obstacle.GetComponent<MovingCubeObstacle>().movePos[0] = pos1;
                    obstacle.GetComponent<MovingCubeObstacle>().movePos[1] = pos2;

                    floorMesh.destroyOnRemake.Add(obstacle);
                    break;
                }
            case ObstacleType.MovingWall:
                {
                    print("make cube");
                    GameObject obstacle = Instantiate<GameObject>(ObstacleDataReader.GetObstaclePrefab(MovingWall)) as GameObject;
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    Vector3 cross = Vector3.Cross(floorMesh.prevDir, floorMesh.dir);

                    Vector3 prevPosMid = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
                    prevPosMid.y += 1.0f;
                    Vector3 pos1 = prevPosMid + Vector3.Cross(floorMesh.dir, Vector3.up) * m_Storer.m_MovingCubeMoveDistance;
                    Vector3 pos2 = prevPosMid - Vector3.Cross(floorMesh.dir, Vector3.up) * m_Storer.m_MovingCubeMoveDistance;
                    obstacle.transform.position = prevPosMid;
                    obstacle.transform.forward = floorMesh.prevDir;
                    obstacle.GetComponent<MovingCubeObstacle>().StartAnim();
                    obstacle.GetComponent<MovingCubeObstacle>().movePos = new Vector3[2];
                    obstacle.GetComponent<MovingCubeObstacle>().movePos[0] = pos1;
                    obstacle.GetComponent<MovingCubeObstacle>().movePos[1] = pos2;

                    floorMesh.destroyOnRemake.Add(obstacle);
                    break;
                }
            case ObstacleType.WallLeftCube:
                {
                    GameObject obstacle = Instantiate(ObstacleDataReader.GetObstaclePrefab(WallLeftCube)) as GameObject;
                    MakeWall(obstacle, meshIndex);
                    break;
                }
            case ObstacleType.WallRightCube:
                {
                    GameObject obstacle = Instantiate(ObstacleDataReader.GetObstaclePrefab(WallRightCube)) as GameObject;
                    MakeWall(obstacle, meshIndex);
                    break;
                }
            case ObstacleType.WallMidCube:
                {
                    GameObject obstacle = Instantiate(ObstacleDataReader.GetObstaclePrefab(WallMidCube)) as GameObject;
                    MakeWall(obstacle, meshIndex);
                    break;
                }
            case ObstacleType.RandomPlacedCube:
                {
                    GameObject obstacle = Instantiate(ObstacleDataReader.GetObstaclePrefab(Cube));
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    float posScale = Random.Range(0.1f, 0.9f);
                    Vector3 prevPosMid = floorMesh.prevPos1 + (floorMesh.prevPos2 - floorMesh.prevPos1) * posScale;
                    prevPosMid += floorMesh.dir * floorMesh.length / 2.0f;
                    prevPosMid.y += 1.0f;
                    obstacle.transform.position = prevPosMid;
                    obstacle.transform.forward = floorMesh.prevDir;
                    obstacle.GetComponent<CubeObstacle>().StartAnim();

                    floorMesh.destroyOnRemake.Add(obstacle);
                    break;
                }
            case ObstacleType.GlidingStart:
                {
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    float scale = Vector3.Distance(floorMesh.prevPos1, floorMesh.prevPos2);
                    floorMesh.makeMesh();

                    //Gliding trigger
                    GameObject trigger = Instantiate(ObstacleDataReader.GetObstaclePrefab(GlidingTrigger));
                    Vector3 endPosMid = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
                    trigger.transform.localScale = new Vector3(scale, 5, 1);
                    trigger.transform.position = endPosMid;
                    trigger.transform.forward = floorMesh.prevDir;

                    floorMesh.destroyOnRemake.Add(trigger);
                    
                    // Add rim
                    Vector3 end1 = floorMesh.endPos1;
                    Vector3 end2 = floorMesh.endPos2;
                    GameObject rim = Instantiate(m_Storer.m_RimPrefab, null);
                    rim.transform.position = (end1 + end2) / 2.0f;
                    rim.transform.localScale = new Vector3(0.3f, 0.3f, (end1 - end2).magnitude);
                    rim.transform.forward = (end1 - end2).normalized;
                    rim.GetComponent<ItemSuper>().StartAnim();

                    // Add sign
                    GameObject signpost1 = Instantiate(ObstacleDataReader.GetObstaclePrefab(GlideSign), null);
                    signpost1.transform.position = Vector3.Lerp (end1, end2, 0.8f);
                    signpost1.transform.forward = floorMesh.prevDir;
                    signpost1.GetComponent<ItemSuper>().StartAnim();
                    floorMesh.destroyOnRemake.Add(signpost1);

                   /* GameObject signpost2 = Instantiate(m_Storer.m_GlideSignPrefab, null);
                    signpost2.transform.position = end2;
                    signpost2.transform.forward = floorMesh.prevDir;
                    signpost2.GetComponent<ItemSuper>().StartAnim();
                    floorMesh.destroyOnRemake.Add(signpost2);*/

                    //rim.transform.parent = floorMesh.transform;
                    floorMesh.destroyOnRemake.Add(rim);

                    madeMesh = true;
                    break;
                }
            case ObstacleType.GlidingEnd:
                {
                    FloorColorController.current.NextColorPalette();

                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    floorMesh.prevPos1 = floorMesh.prevPos1 + floorMesh.dir * m_Storer.m_GlideDistance; //floorMesh.length * 6.2f;
                    floorMesh.prevPos2 = floorMesh.prevPos2 + floorMesh.dir * m_Storer.m_GlideDistance; // * floorMesh.length * 6.2f;
                    floorMesh.prevPos1.y -= m_Storer.m_GlideHeight; //6.5f;
                    floorMesh.prevPos2.y -= m_Storer.m_GlideHeight;  //6.5f;
                    floorMesh.makeMesh();

                    if(GlideTrigger.current )
                    {
                        float upY = 3.5f;

                        Vector3 prevPos = GlideTrigger.current.transform.position;
                        prevPos.y += upY;
                        Vector3 endPos = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
                        endPos.y += upY;
                        Vector3 dir = (endPos - prevPos).normalized;
                        float dist = Vector3.Distance(prevPos, endPos);
                        float freq = Random.Range(5.0f, 10.0f);
                        Vector3 left = Vector3.Cross(dir, Vector3.up);
                        float offset = Random.Range(1.0f, 5.0f);

                        float gap = m_Storer.m_GlideCoinGap;
                        int count = (int) (dist / gap);
                        
                        for(int i=1; i < count - 1; ++i)
                        {
                            float ii = (float)i;
                            GameObject coins = Instantiate(m_Storer.m_GliderCoins[Random.Range(0, m_Storer.m_GliderCoins.Length)]);
                            //Coin coin = CoinGenerator.current.GetNextCoin();
                            coins.transform.position = prevPos + dir * ii * gap + left * offset * Mathf.Sin(ii /freq);
                            coins.transform.forward = dir;
                            coins.gameObject.SetActive(true);
                            coins.GetComponent<ItemSuper>().StartAnim();
                            //coin.m_LifeTime = 20.0f;
                            Destroy(coins, 20.0f);
                        }
                    }
                    
                    Vector3 end1 = floorMesh.prevPos1;
                    Vector3 end2 = floorMesh.prevPos2;
                    GameObject rim = Instantiate(m_Storer.m_RimPrefab, null);
                    rim.transform.position = (end1 + end2) / 2.0f;
                    rim.transform.localScale = new Vector3(0.3f, 0.3f, (end1 - end2).magnitude);
                    rim.transform.forward = (end1 - end2).normalized;
                    rim.GetComponent<ItemSuper>().StartAnim();

                    madeMesh = true;
                    break;
                }
            case ObstacleType.AutoPilotEnd:
                {
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    float scale = Vector3.Distance(floorMesh.prevPos1, floorMesh.prevPos2);

                    GameObject trigger = Instantiate(ObstacleDataReader.GetObstaclePrefab(AutoPilotTrigger));
                    Vector3 endPosMid = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
                    trigger.transform.localScale = new Vector3(scale, 5, 1);
                    trigger.transform.position = endPosMid;
                    trigger.transform.forward = floorMesh.prevDir;

                    floorMesh.destroyOnRemake.Add(trigger);

                    break;
                }
            case ObstacleType.AutoPilotStopSign:
                {
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    float scale = Vector3.Distance(floorMesh.prevPos1, floorMesh.prevPos2);

                    // Add sign
                    Vector3 end1 = floorMesh.prevPos1;
                    Vector3 end2 = floorMesh.prevPos2;
                    GameObject signpost1 = Instantiate(ObstacleDataReader.GetObstaclePrefab(StopSign), null);
                    signpost1.transform.position = Vector3.Lerp(end1, end2, 0.8f);
                    signpost1.transform.forward = floorMesh.prevDir;
                    signpost1.GetComponent<ItemSuper>().StartAnim();
                    floorMesh.destroyOnRemake.Add(signpost1);

                    /*GameObject signpost2 = Instantiate(m_Storer.m_StopSignPrefab, null);
                    signpost2.transform.position = end2;
                    signpost2.transform.forward = floorMesh.prevDir;
                    signpost2.GetComponent<ItemSuper>().StartAnim();
                    floorMesh.destroyOnRemake.Add(signpost2);*/

                    print("sign");

                    break;
                }
            default:break;
        }
        return madeMesh;
    }

    void MakeWall(GameObject obstacle, int meshIndex)
    {
        FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
        float scale = Vector3.Distance(floorMesh.prevPos1, floorMesh.prevPos2) / WallWidth;

        Vector3 prevPosMid = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
        prevPosMid += floorMesh.dir * floorMesh.length / 2.0f;
        prevPosMid.y += 1.0f;
        obstacle.transform.localScale = new Vector3(scale, 1, 1);
        obstacle.transform.position = prevPosMid;
        obstacle.transform.forward = floorMesh.prevDir;
        obstacle.GetComponent<WallObstacle>().StartAnim();

        floorMesh.destroyOnRemake.Add(obstacle);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
