using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugFunctionCaller : MonoBehaviour {

    public UnityEvent m_Event;

    public void CallFunc()
    {
        m_Event.Invoke();
    }
}
