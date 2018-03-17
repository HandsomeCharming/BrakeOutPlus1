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

        floorMesh.destroyOnRemake = obstacle;
    }

    //Return has made mesh
    public bool makeObstacleOnMesh(int meshIndex, ObstacleType obstacleType)
    {
        //print(obstacleType);
        bool madeMesh = false;
        switch(obstacleType)
        {
            case ObstacleType.Cube:
                {
                    GameObject obstacle = Instantiate(CubePrefab);
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

                    floorMesh.destroyOnRemake = obstacle;
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
                    madeMesh = true;
                    break;
                }
            case ObstacleType.BeforeJump:
                {
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    int signIndex = meshIndex - 1;
                    if (signIndex < 0) signIndex = FloorBuilder.current.floorMeshes.Length - 1;
                    FloorMesh signMesh = FloorBuilder.current.floorMeshes[signIndex];


                    GameObject sign = Instantiate(m_Storer.BoostSignPrefab); ;

                    Vector3 prevPosMid = (signMesh.prevPos1 + signMesh.prevPos2) / 2.0f;
                    prevPosMid.y += 0.1f;
                    sign.transform.position = prevPosMid;
                    sign.transform.forward = signMesh.prevDir;
                    sign.transform.Rotate(90.0f, 0, 0);
                    sign.GetComponent<ItemSuper>().StartAnim();

                    Vector3 dir = floorMesh.dir;
                    dir.z = -dir.z;
                    floorMesh.makeMesh();
                    //floorMesh.changeTexture("RedWhiteStripe");
                    //floorMesh.changeNormalByDir(Vector3.Cross(dir, new Vector3(dir.z, 0, -dir.x)));
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
                    GameObject obstacle = Instantiate(WallPrefab) as GameObject;
                    MakeWall(obstacle, meshIndex);
                    break;
                }
            case ObstacleType.BlackHole:
                {
                    GameObject obstacle = Instantiate(BlackholePrefab) as GameObject;
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

                    floorMesh.destroyOnRemake = obstacle;
                    break;
                }
            case ObstacleType.MovingCube:
                {
                    print("make cube");
                    GameObject obstacle = Instantiate<GameObject>(MovingCubePrefab) as GameObject;
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

                    floorMesh.destroyOnRemake = obstacle;
                    break;
                }
            case ObstacleType.MovingWall:
                {
                    print("make cube");
                    GameObject obstacle = Instantiate<GameObject>(MovingWallPrefab) as GameObject;
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

                    floorMesh.destroyOnRemake = obstacle;
                    break;
                }
            case ObstacleType.WallLeftCube:
                {
                    GameObject obstacle = Instantiate(WallLeftCubePrefab) as GameObject;
                    MakeWall(obstacle, meshIndex);
                    break;
                }
            case ObstacleType.WallRightCube:
                {
                    GameObject obstacle = Instantiate(WallRightCubePrefab) as GameObject;
                    MakeWall(obstacle, meshIndex);
                    break;
                }
            case ObstacleType.WallMidCube:
                {
                    GameObject obstacle = Instantiate(WallMidCubePrefab) as GameObject;
                    MakeWall(obstacle, meshIndex);
                    break;
                }
            case ObstacleType.RandomPlacedCube:
                {
                    GameObject obstacle = Instantiate(CubePrefab);
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    float posScale = Random.Range(0.1f, 0.9f);
                    Vector3 prevPosMid = floorMesh.prevPos1 + (floorMesh.prevPos2 - floorMesh.prevPos1) * posScale;
                    prevPosMid += floorMesh.dir * floorMesh.length / 2.0f;
                    prevPosMid.y += 1.0f;
                    obstacle.transform.position = prevPosMid;
                    obstacle.transform.forward = floorMesh.prevDir;
                    obstacle.GetComponent<CubeObstacle>().StartAnim();

                    floorMesh.destroyOnRemake = obstacle;
                    break;
                }
            case ObstacleType.GlidingStart:
                {
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    float scale = Vector3.Distance(floorMesh.prevPos1, floorMesh.prevPos2);

                    GameObject trigger = Instantiate(GlidingTriggerPrefab);
                    Vector3 endPosMid = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
                    trigger.transform.localScale = new Vector3(scale, 5, 1);
                    trigger.transform.position = endPosMid;
                    trigger.transform.forward = floorMesh.prevDir;

                    floorMesh.destroyOnRemake = trigger;
                    
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
                     
                    madeMesh = true;
                    break;
                }
            case ObstacleType.AutoPilotEnd:
                {
                    FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
                    float scale = Vector3.Distance(floorMesh.prevPos1, floorMesh.prevPos2);

                    GameObject trigger = Instantiate(AutoPilotTriggerPrefab);
                    Vector3 endPosMid = (floorMesh.prevPos1 + floorMesh.prevPos2) / 2.0f;
                    trigger.transform.localScale = new Vector3(scale, 5, 1);
                    trigger.transform.position = endPosMid;
                    trigger.transform.forward = floorMesh.prevDir;

                    floorMesh.destroyOnRemake = trigger;

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

        floorMesh.destroyOnRemake = obstacle;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
