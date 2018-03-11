using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPilotBall : ItemSuper {

    public override void Disable()
    {
        base.Disable();
        Destroy(gameObject, 0.1f);
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			if (Player.current.gameObject.GetComponent<AutoPilot> () == null) {
				Player.current.gameObject.AddComponent<AutoPilot> ().SetPilotTime (ItemManager.GetItemDuration(ItemType.AutoPilot));
			} else {
				Player.current.gameObject.GetComponent<AutoPilot> ().SetPilotTime (ItemManager.GetItemDuration(ItemType.AutoPilot));
			}
            Destroy(gameObject);
		}
	}
}
