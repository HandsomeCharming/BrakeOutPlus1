using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventCallBackResult
{
    Success,
    Failure
}

public class EventDispatcher {
    
    static EventDispatcher Instance
    {
        get
        {
            if (_instance == null)
                _instance = new EventDispatcher();
            return _instance;
        }
        set { }
    }
    static EventDispatcher _instance;
    
    public delegate EventCallBackResult EventHandler();

    public Dictionary<string, EventHandler> m_EventMap;

    public static void ListenTo(string eventName, EventHandler action)
    {
        Instance.ListenToPrivate(eventName, action);
    }
    
    public static void UnListenTo(string eventName, EventHandler action)
    {
        Instance.UnListenToPrivate(eventName, action);
    }

    public static void CallEvent(string eventName)
    {
        Instance.CallEventPrivate(eventName);
    }
    
    void ListenToPrivate(string eventName, EventHandler action)
    {
        if (!m_EventMap.ContainsKey(eventName))
        {
            m_EventMap.Add(eventName, null);
        }
        m_EventMap[eventName] += action;
    }

    void UnListenToPrivate(string eventName, EventHandler action)
    {
        if (!m_EventMap.ContainsKey(eventName))
        {
            m_EventMap.Add(eventName, null);
        }
        m_EventMap[eventName] -= action;
    }

    void CallEventPrivate(string eventName)
    {
        if (m_EventMap.ContainsKey(eventName) && m_EventMap[eventName] != null)
        {
            m_EventMap[eventName]();
        }
    }
}
