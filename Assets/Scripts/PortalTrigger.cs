using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Messenger.Broadcast(GameEvent.PORTAL_ENTERED);
            Debug.Log("Portal Entered");
        }
    }
}
