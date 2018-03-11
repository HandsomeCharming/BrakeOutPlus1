using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideTrigger : MonoBehaviour {

    public static GlideTrigger current;

    private void Awake()
    {
        current = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            print("Glide trigger");
            Player.current.gameObject.AddComponent<GlideOnPlayer>();
        }
    }
}
