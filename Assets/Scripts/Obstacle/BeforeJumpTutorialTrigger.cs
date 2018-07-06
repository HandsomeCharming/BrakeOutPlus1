using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeJumpTutorialTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIManager.current.m_Tutorial.ShowJumpTutorialIfFirstTime();
        }
    }
}
