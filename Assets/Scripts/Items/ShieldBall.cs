using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBall : ItemSuper
{
    const string prefabName = "Prefabs/Items/ShieldOnPlayer";

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
            if (ShieldOnPlayer.current == null)
            {
                GameObject go = (GameObject)Instantiate(Resources.Load(prefabName));
                go.GetComponent<ShieldOnPlayer>().Init(ItemManager.GetItemDuration(ItemType.Shield));
            }
            else
            {
                ShieldOnPlayer.current.m_Time += ItemManager.GetItemDuration(ItemType.Shield);
            }
            Destroy(gameObject);
        }
    }
}
