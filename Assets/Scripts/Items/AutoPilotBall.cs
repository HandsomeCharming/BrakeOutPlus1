using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPilotBall : ItemSuper {

    private void OnEnable()
    {
        ScaleUp(itemEnlargeSize);
    }

    public override void Disable()
    {
        base.Disable();
        Destroy(gameObject, 0.1f);
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
            if (!GameManager.current.m_StartBoosting)
            {
                float duration = ItemManager.GetItemDuration(ItemType.AutoPilot);
                if (Player.current.gameObject.GetComponent<AutoPilot>() == null)
                {
                    Player.current.gameObject.AddComponent<AutoPilot>().SetPilotTimeNoEffect(duration);
                }
                else
                {
                    Player.current.gameObject.GetComponent<AutoPilot>().SetPilotTime(ItemManager.GetItemDuration(ItemType.AutoPilot));
                }
            }
            Destroy(gameObject);
		}
	}
}
