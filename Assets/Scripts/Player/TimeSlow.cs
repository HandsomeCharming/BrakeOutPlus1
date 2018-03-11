using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour {

    public float m_Time;

    public void SetTimer(float time)
    {
        this.m_Time = time;
        GameManager.current.TokiyoTomare(0.7f);

        Camera.main.GetComponent<CameraFollow>().EnableMotionBlur(true);

        AudioSystem.current.PlayEvent(AudioSystemEvents.SlowTimeStartEventName);
    }

    private void OnDisable()
    {
        AudioSystem.current.PlayEvent(AudioSystemEvents.SlowTimeStopEventName);
    }

    private void Update()
    {
        m_Time -= Time.deltaTime;
        if (m_Time <= 0)
        {
            Destroy(this, 0.1f);
            GameManager.current.TokiyoTomare(1.0f);

            Camera.main.GetComponent<CameraFollow>().EnableMotionBlur(false);
        }
    }
}
