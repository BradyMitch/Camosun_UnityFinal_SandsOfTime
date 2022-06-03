using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFinishedPopup : BasePopup
{
    public void OnExitButton()
    {
        Debug.Log("exit game");
        Application.Quit();
    }
}
