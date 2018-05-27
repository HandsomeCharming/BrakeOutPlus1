using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScoreBall : ItemSuper
{
    const string prefabName = "Prefabs/Items/MagnetOnPlayer";
    
    public override void Disable()
    {
        base.Disable();
        Destroy(gameObject, 0.1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            float time = ItemManager.GetItemDuration(ItemType.DoubleScore);
            InGameUI.Instance.StartPowerup(Powerups.DoubleScore);
            GameManager.current.SetItemMultiplier(2.0f, time);
            //UIManager.current.m_Ingame.StartDoubleScoreCountDown(time);
            
            Destroy(gameObject);
        }
    }
}
