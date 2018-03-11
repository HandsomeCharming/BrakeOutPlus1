using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPilotEndTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(Player.current.GetComponent<AutoPilot>())
            {
                Player.current.GetComponent<AutoPilot>().EndAutoPilotInTime(1);
            }
            
        }
    }

    private void OnDisable()
    {
        if (Player.current && Player.current.GetComponent<AutoPilot>())
        {
            Player.current.GetComponent<AutoPilot>().EndAutoPilot();
        }
    }
}
