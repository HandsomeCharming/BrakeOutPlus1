using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourglassBall : ItemSuper {

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
        if (other.tag == "Player")
        {
            if (Player.current.gameObject.GetComponent<TimeSlow>() == null)
            {
                Player.current.gameObject.AddComponent<TimeSlow>().SetTimer(ItemManager.GetItemDuration(ItemType.TimeSlow));
            }
            else
            {
                Player.current.gameObject.GetComponent<TimeSlow>().SetTimer(ItemManager.GetItemDuration(ItemType.TimeSlow));
            }
            Destroy(gameObject);
        }
    }
}
