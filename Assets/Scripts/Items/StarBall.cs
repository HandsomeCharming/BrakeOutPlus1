using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBall : ItemSuper
{

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
            GameManager.current.AddStar(1);
            Destroy(gameObject);
        }
    }
}
