using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// Required when using Event data.

public class ResumeIfGravityControl : MonoBehaviour, IPointerDownHandler
{
    // hard code, only for jump tutorial
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (InputHandler.current.m_ControlScheme == ControlSchemes.Gravity && GameManager.current.IsPaused())
        {
            UIManager.current.m_Tutorial.HideJumpTutorial();
        }
    }
}
