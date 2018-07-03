using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour {
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("jump trigger");
            QuestManager.UpdateQuestsStatic(QuestAction.LeapGap);
        }
    }
}
