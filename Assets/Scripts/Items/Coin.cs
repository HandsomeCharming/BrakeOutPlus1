using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : ItemSuper {
    public int index;
    public int meshIndex;

    public int m_CoinAmount = 1;
    
    public bool isMagneting;

    public CoinModel model;
    public GameObject shadow;

    public float m_MagSpeed;

    // if -1, always live, if >0,count down
    public float m_LifeTime = -1;

    const float m_MagnetAcc = 150.0f;
    const float m_MagnetDistThreshold = 0.6f;

	// Use this for initialization
	void Start () {
        isMagneting = false;
	}

    public override void Disable()
    {
        disableCoinWithGenerator();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            TouchedByPlayer();
        }
    }

    public void TurnRed()
    {
        m_CoinAmount = 5;
        model.TurnRed();
    }

    public void TurnYellow()
    {
        m_CoinAmount = 1;
        model.TurnYellow();
    }

    public void TouchedByPlayer()
    {
        GameManager.current.AddCoinInGame(m_CoinAmount);
        isMagneting = false;
        AudioSystem.current.PlayCoin();
        VibrateService.instance.TriggerLight();

        disableCoinWithGenerator();

        //FMODUnity.RuntimeManager.PlayOneShot("event:/Coin");
        //FloorBuilder.current.floorMeshes[meshIndex].coinIndex = -1;
    }

    public void resetCoin()
    {
        //model.resetCoin();
        //shadow.SetActive(true);
        //GetComponent<BoxCollider>().enabled = true;
        m_LifeTime = -1;
		gameObject.SetActive(true);
    }

    public void StartMagneting()
    {
        isMagneting = true;
        GetComponent<BoxCollider>().enabled = false;
        m_MagSpeed = 0;
    }

    public void disableCoin()
    {
        //model.disabled();
        //GetComponent<BoxCollider>().enabled = false;
        isMagneting = false;
        //shadow.SetActive(false);
        gameObject.SetActive(false);
    }

    public void disableCoinWithGenerator()
    {
        CoinGenerator.current.disableCoin(index);
    }

	// Update is called once per frame
	void Update () {
        if(isMagneting && Player.current != null)
        {
            //m_MagVelocity /= 2.0f;
            //m_MagVelocity += (Player.current.transform.position - transform.position).normalized * (m_MagnetAcc * Time.deltaTime);
            m_MagSpeed += m_MagnetAcc * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Player.current.transform.position, m_MagSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, Player.current.transform.position) <= m_MagnetDistThreshold)
            {
                TouchedByPlayer();
            }
        }

        if(m_LifeTime >0)
        {
            m_LifeTime -= Time.deltaTime;
            if(m_LifeTime <= 0)
            {
                disableCoinWithGenerator();
            }
        }
    }
}
