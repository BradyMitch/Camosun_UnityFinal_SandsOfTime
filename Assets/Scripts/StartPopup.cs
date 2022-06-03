using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPopup : BasePopup
{
    public void OnStartButton()
    {
        Messenger.Broadcast(GameEvent.START_GAME);
        Close();
    }
}
