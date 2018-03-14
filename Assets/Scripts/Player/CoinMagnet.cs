using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour {

    public float m_Time;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            Coin coin = other.GetComponent<Coin>();
            if (coin != null)
                coin.StartMagneting();
            else
            {
                CoinNoPool c = other.GetComponent<CoinNoPool>();
                c.StartMagneting();
            }
        }
    }

    public void SetTimer(float time)
    {
        this.m_Time = time;
    }

    private void OnDisable()
    {
        AudioSystem.current.PlayEvent(AudioSystemEvents.MagnetStopEventName);
    }

    private void Awake()
    {
        AudioSystem.current.PlayEvent(AudioSystemEvents.MagnetStartEventName);
    }

    private void Update()
    {
        if(Player.current != null)
        {
            transform.position = Player.current.transform.position;
        }

        m_Time -= Time.deltaTime;
        if(m_Time <= 0)
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
