using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private StartPopup startPopup;
    [SerializeField] private GameFinishedPopup gameFinishedPopup;

    [SerializeField] private Image crossHair;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI sandText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI pickupText;

    private int popupsOpen = 1;

    void Awake()
    {
        Messenger.AddListener(GameEvent.POPUP_OPENED, this.OnPopupsOpened);
        Messenger.AddListener(GameEvent.POPUP_CLOSED, this.OnPopupsClosed);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.POPUP_OPENED, this.OnPopupsOpened);
        Messenger.RemoveListener(GameEvent.POPUP_CLOSED, this.OnPopupsClosed);
    }

    private void Start()
    {
        SetGameActive(false);
    }

    public void SetGameActive(bool active)
    {
        if (active)
        {
            Debug.Log("GameActive");
            Messenger.Broadcast(GameEvent.GAME_ACTIVE);
            Time.timeScale = 1;                        // unpause the game 
            Cursor.visible = false; // hide cursor
            Cursor.lockState = CursorLockMode.Locked;  // show the cursor 
            crossHair.gameObject.SetActive(true);      // show the crosshair
            coinsText.gameObject.SetActive(true);
            sandText.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("GameInActive");
            Messenger.Broadcast(GameEvent.GAME_INACTIVE);
            Time.timeScale = 0;                       // pause the game 
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;   // show the cursor 
            crossHair.gameObject.SetActive(false);    // turn off the crosshair 
            coinsText.gameObject.SetActive(false);
            sandText.gameObject.SetActive(false);
        }
    }

    private void OnPopupsOpened()
    {
        if (popupsOpen == 0)
        {
            SetGameActive(false);
        }
        popupsOpen++;
    }

    private void OnPopupsClosed()
    {
        popupsOpen--;
        if (popupsOpen == 0)
        {
            SetGameActive(true);
        }
    }

    public void UpdateCoinText(int coins, int bankedCoins)
    {
        if (bankedCoins > 0)
        {
            coinsText.text = "Coins: " + coins.ToString() + " + " + bankedCoins + " banked.";
        } else
        {
            coinsText.text = "Coins: " + coins.ToString();
        }
    }

    public void UpdateSandText(int sand)
    {
        sandText.text = "Sand: " + sand.ToString();
    }

    public IEnumerator PickupItemText(string text)
    {
        pickupText.text = text;
        pickupText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        pickupText.gameObject.SetActive(false);
    }

    public void GameFinished(int coins, bool portalExit)
    {
        if (portalExit)
        {
            finalScoreText.text = "You escaped the temple with " + coins + " coins!";
        } else
        {
            finalScoreText.text = "You couldn't escape the temple. You banked " + coins + " coins.";
        }
        gameFinishedPopup.Open();
    }
}
