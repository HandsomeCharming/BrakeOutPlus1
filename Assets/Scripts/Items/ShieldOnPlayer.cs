using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOnPlayer : MonoBehaviour {

    public float m_Time;

    private void Awake()
    {
    }

    public void SetTimer(float time)
    {
        this.m_Time = time;
        transform.parent = Player.current.transform;
        transform.localPosition = new Vector3(0, 0.97f, -0.4f);
    }

    void PopHealth()
    {
        if (Player.current.m_Health > 1)
            Player.current.m_Health--;
    }

    public void PlayPopAnim()
    {
        m_Time = 10.0f;
        Destroy(gameObject, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayPopAnim();
    }

    private void Update()
    {
        m_Time -= Time.deltaTime;
        if (m_Time <= 0)
        {
            PlayPopAnim();
            PopHealth();
        }
    }
}
