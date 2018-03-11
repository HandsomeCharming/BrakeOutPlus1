using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using System;

public class LeftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool m_Held;

    private void Awake()
    {
        m_Held = false;
    }

    private void Update()
    {
        if (m_Held)
        {
            InputHandler.current.LeftTurnPlayer(true);
        }
	}

	void OnEnable()
	{
		m_Held = false;
	}

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        InputHandler.current.LeftTurnPlayer(true);
        m_Held = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        m_Held = false;
        InputHandler.current.LeftTurnPlayer(false);
    }
}
