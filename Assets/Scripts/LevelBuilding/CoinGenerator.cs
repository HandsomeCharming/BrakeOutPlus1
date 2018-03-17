using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour {

    public static CoinGenerator current;

    public int coinCount;
    public GameObject coinPrefab;
    
    public Coin[] coins;
    public Queue<Coin> notShowingCoins;

	// Use this for initialization
	void Awake () {
        current = this;
        coins = new Coin[coinCount];
        notShowingCoins = new Queue<Coin>();
        for (int a = 0; a != coinCount; ++a)
        {
            GameObject coinObject = (GameObject)Instantiate<GameObject>(coinPrefab);
            coinObject.transform.position = Vector3.zero;
            coins[a] = coinObject.GetComponent<Coin>();
            coins[a].index = a;
			notShowingCoins.Enqueue(coins[a]);
			coins[a].disableCoin();
            coinObject.transform.parent = transform;
        }
    }

    public void putCoin(int meshIndex)
    {
        Coin coin = GetNextCoin();
        while (coin.gameObject.activeSelf && notShowingCoins.Count > 0)
            coin = GetNextCoin();
        if (notShowingCoins.Count == 0) return;
        //Coin coin = GetNextCoin();
        coin.resetCoin();
        if(ChallengeManager.current.ShouldRedCoin())
        {
            coin.TurnRed();
        }
        else
        {
            coin.TurnYellow();
        }

        FloorMesh floorMesh = FloorBuilder.current.floorMeshes[meshIndex];
        
        coin.meshIndex = meshIndex;
        GameObject obj = coin.gameObject;
        Vector3 cross = Vector3.Cross(floorMesh.prevDir, floorMesh.dir);
        int slot = floorMesh.floorTypeData.coinSlot; // cross.y < 0 ? 0 : floorMesh.GetMaxSlot();
        floorMesh.PutItemOnSlot(coin, slot);

        //print(dot);
        /* float posScale = cross.y < 0 ? 0.8f : 0.2f;
        Vector3 prevPosMid = floorMesh.prevPos1 + (floorMesh.prevPos2 - floorMesh.prevPos1) * posScale;
        prevPosMid += floorMesh.dir * floorMesh.length / 2.0f;
        prevPosMid.y += 1.0f;
        
        obj.transform.position = prevPosMid;
        obj.transform.forward = floorMesh.prevDir;
        coin.StartAnim();*/
    }

    public Coin GetNextCoin()
    {
        if (notShowingCoins.Count > 0)
        {
            return notShowingCoins.Dequeue();
        }
        else
        {
            print("No coin");
            return null;
        }
    }

    public GameObject GetNextCoinGO()
    {
        if(notShowingCoins.Count > 0)
        {
            return notShowingCoins.Dequeue().gameObject;
        }
        else
        {
            print("No coin");
            return null;
        }
    }

    public void disableCoin(int index)
    {
        //print(index);
        if (index < 0 || index >= coinCount) return;
        notShowingCoins.Enqueue(coins[index]);
        coins[index].disableCoin();
    }

    public void ResetAllCoins()
    {
        for (int a = 0; a != coinCount; ++a)
        {
            disableCoin(a);
        }
    }
}
