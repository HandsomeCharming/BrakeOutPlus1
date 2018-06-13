using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinNoPool : MonoBehaviour {

    public int m_CoinAmount = 1;

    public bool isMagneting;
    public float m_MagSpeed;
    
    const float m_MagnetAcc = 150.0f;
    const float m_MagnetDistThreshold = 0.6f;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TouchedByPlayer();
        }
    }

    public void StartMagneting()
    {
        isMagneting = true;
        GetComponent<BoxCollider>().enabled = false;
        m_MagSpeed = 0;
    }

    public void TouchedByPlayer()
    {
        GameManager.current.AddCoinInGame(m_CoinAmount);
        isMagneting = false;
        AudioSystem.current.PlayCoin();

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
        if (isMagneting && Player.current != null)
        {
            //m_MagVelocity /= 2.0f;
            //m_MagVelocity += (Player.current.transform.position - transform.position).normalized * (m_MagnetAcc * Time.deltaTime);
            m_MagSpeed += m_MagnetAcc * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Player.current.transform.position, m_MagSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, Player.current.transform.position) <= m_MagnetDistThreshold)
            {
                TouchedByPlayer();
            }
        }
    }
}
