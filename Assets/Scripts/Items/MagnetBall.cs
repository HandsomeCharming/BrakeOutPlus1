using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBall : ItemSuper
{
    const string prefabName = "Prefabs/Items/MagnetOnPlayer";

    private void OnEnable()
    {
        ScaleUp(1.5f);
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
            GameObject go = (GameObject) Instantiate(Resources.Load(prefabName));
            go.GetComponent<CoinMagnet>().SetTimer(ItemManager.GetItemDuration(ItemType.Magnet));
            Destroy(gameObject);
        }
    }
}
