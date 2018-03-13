using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOnPlayer : MonoBehaviour {

    public static ShieldOnPlayer current;

    public float m_Time = 10.0f;

    private void Awake()
    {
        current = this;
    }

    public void Init(float time)
    {
        this.m_Time = time;
        print(time);
        transform.parent = Player.current.transform;
        transform.localPosition = new Vector3(0, 0.97f, -0.4f);

        if (Player.current.m_Health == 1)
            Player.current.m_Health++;
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
