using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableItem
    {
        Sand,
        Coins,
        Key
    }

    [SerializeField] private CollectableItem itemType;
    [SerializeField] private int value = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (itemType == CollectableItem.Sand)
            {
                Messenger.Broadcast(GameEvent.PICKUP_SAND);
            } else if (itemType == CollectableItem.Coins)
            {
                Messenger<int>.Broadcast(GameEvent.PICKUP_COINS, value);
            } else if (itemType == CollectableItem.Key)
            {
                Messenger<int>.Broadcast(GameEvent.PICKUP_KEY, value);
            }
            Destroy(this.gameObject);
        }
    }
}
