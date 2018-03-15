using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class MakeFloorTypeActionTask : ActionTask
{

    public BBParameter<int> minFloorCount;
    public BBParameter<int> maxFloorCount;

    public BBParameter<float> minFloorTurningAngle;
    public BBParameter<float> maxFloorTurningAngle;

    public BBParameter<int> minCoinCount;
    public BBParameter<int> maxCoinCount;

    public BBParameter<float> coinShowProb; // should be 0-1
    
    public BBParameter<ObstacleType> obstacleType;
    public BBParameter<int> minObstacleCount;
    public BBParameter<int> maxObstacleCount;

    //public float floorWidth;

    protected override void OnExecute()
    {
        FloorTypeData data = new FloorTypeData();
        
        data.SetFloorCount(Random.Range(minFloorCount.value, maxFloorCount.value));
        bool hasCoin = Random.value < coinShowProb.value;
        data.coinCount = hasCoin ? Random.Range(minCoinCount.value, maxCoinCount.value) : 0;
        data.coinStartIndex = Random.Range(0, data.floorCount - data.coinCount);
        data.floorTurningAngle = (Random.Range(0.0f, 1.0f) > 0.5f ? 1 : -1) * Random.Range(minFloorTurningAngle.value, maxFloorTurningAngle.value);
        //data.floorWidth = floorWidth;
        data.obstacles = new ArrayList();
        ObstacleData obstacleData = new ObstacleData();

        if (obstacleType.value == ObstacleType.Jump)
        {
            obstacleData.obstacleType = ObstacleType.Jump;
            obstacleData.obstacleCount = 1;
            obstacleData.obstacleStartIndex = 1;//Random.Range(1, data.floorCount - obstacleData.obstacleCount - 1);
            data.obstacles.Add(obstacleData);
        }
        else
        {
            obstacleData.obstacleType = obstacleType.value;
            obstacleData.obstacleCount = Random.Range(minObstacleCount.value, maxObstacleCount.value);
            obstacleData.obstacleStartIndex = Random.Range(0, data.floorCount - obstacleData.obstacleCount);
            data.obstacles.Add(obstacleData);
        }
        ChallengeManager.SetFloorTypeData(data);

        EndAction(true);
    }
}
