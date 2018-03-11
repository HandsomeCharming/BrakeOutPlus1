using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBall : ItemSuper
{
    const string prefabName = "Prefabs/Items/ShieldOnPlayer";
    public override void Disable()
    {
        base.Disable();
        Destroy(gameObject, 0.1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject go = (GameObject)Instantiate(Resources.Load(prefabName));
            go.GetComponent<ShieldOnPlayer>().SetTimer(ItemManager.GetItemDuration(ItemType.Shield));
            Player.current.m_Health++;
            Destroy(gameObject);
        }
    }
}
