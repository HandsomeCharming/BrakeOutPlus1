using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBall : ItemSuper
{
    void OnEnable()
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
            GameManager.current.AddStar(1);
            Destroy(gameObject);
        }
    }
}
