using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ShowPlayerLevelText : MonoBehaviour {

    private void OnEnable()
    {
        if(QuestManager.current != null)
        {
            GetComponent<Text>().text = (QuestManager.current.m_QuestData.currentLevel+1).ToString();
        }
    }
}
