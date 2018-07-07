using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOverTime : MonoBehaviour {

    public MinMaxData m_ScaleMinMax;
    public float m_Duration;
    public bool m_Restart;


    bool runs = true;
    float m_DurationRemain;
    const float updateFreq = 0.05f;

    private void OnEnable()
    {
        m_DurationRemain = 0;
        runs = true;
        StartCoroutine(UpdateScale());
    }

    void OnDisable()
    {
        runs = false;
    }

    IEnumerator UpdateScale()
    {
        while(runs)
        {
			float scale = Mathf.Lerp(m_ScaleMinMax.max, m_ScaleMinMax.min, m_DurationRemain / m_Duration);
            transform.localScale = Vector3.one * scale;

            yield return new WaitForSecondsRealtime(updateFreq);
            m_DurationRemain += updateFreq;
            if (m_Restart && m_DurationRemain >= m_Duration)
                m_DurationRemain = 0;
        }
    }

    // Update is called once per frame
    void Update () {
    }
}
