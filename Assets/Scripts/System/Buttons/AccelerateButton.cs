using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class AccelerateButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static List<AccelerateButton> accButtons;

    bool m_Held;

    private void Awake()
    {
        m_Held = false;
    }

    private void Update()
    {
        if (m_Held)
        {
            InputHandler.current.AccelerateButton(true);
        }
    }

	void OnEnable()
	{
		m_Held = false;

        if (accButtons == null)
            accButtons = new List<AccelerateButton>();
        accButtons.Add(this);
	}

    private void OnDisable()
    {
        accButtons.Remove(this);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        InputHandler.current.AccelerateButton(true);
        m_Held = true;

        if(GameManager.current.IsPaused())
        {
            UIManager.current.m_Tutorial.HideJumpTutorial();
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        m_Held = false;
        InputHandler.current.AccelerateButton(false);
    }
}