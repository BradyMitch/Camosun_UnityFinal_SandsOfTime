using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TextMeshPro timerText;
    [SerializeField] private GameObject spotlight;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioClip clockTickClip;
    [SerializeField] private AudioClip vaultOpenClip;
    [SerializeField] private AudioClip coinPickupClip;
    [SerializeField] private AudioClip keyPickupClip;
    [SerializeField] private AudioClip genericPickupClip;
    [SerializeField] private AudioClip exitPortalClip;
    [SerializeField] private AudioSource audioSourcePlayer;

    private int sand = 0;
    private int coins = 0;
    private int bankedCoins = 0;

    private bool redKey = false;
    private bool greenKey = false;
    private bool blueKey = false;

    [SerializeField] private float playerReach = 5;
    [SerializeField] private int seconds = 60;
    [SerializeField] private int secondsPerSand = 10;

    void Awake()
    {
        Messenger.AddListener(GameEvent.START_GAME, this.OnGameStart);
        Messenger.AddListener(GameEvent.PICKUP_SAND, this.OnPickupSand);
        Messenger.AddListener(GameEvent.PORTAL_ENTERED, this.OnPortalEnter);
        Messenger<int>.AddListener(GameEvent.PICKUP_COINS, this.OnPickupCoins);
        Messenger<int>.AddListener(GameEvent.PICKUP_KEY, this.OnPickupKey);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.START_GAME, this.OnGameStart);
        Messenger.RemoveListener(GameEvent.PICKUP_SAND, this.OnPickupSand);
        Messenger.RemoveListener(GameEvent.PORTAL_ENTERED, this.OnPortalEnter);
        Messenger<int>.RemoveListener(GameEvent.PICKUP_COINS, this.OnPickupCoins);
        Messenger<int>.RemoveListener(GameEvent.PICKUP_KEY, this.OnPickupKey);
    }

    private void Update()
    {
        //Check for mouse click 
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, playerReach))
            {
                if (raycastHit.transform != null)
                {
                    //Our custom method. 
                    CurrentClickedGameObject(raycastHit.transform.gameObject);
                }
            }
        }
    }

    public void CurrentClickedGameObject(GameObject gameObject)
    {
        if (gameObject.tag == "TimerBowl")
        {
            if (sand >= 1)
            {
                sand--;
                uiManager.UpdateSandText(sand);
                seconds += secondsPerSand;
                audioSourcePlayer.PlayOneShot(genericPickupClip, 10f);
                Debug.Log("Added " + secondsPerSand + " seconds");
            }
        } else if (gameObject.tag == "Bank")
        {
            if (coins >= 5)
            {
                bankedCoins += Mathf.RoundToInt(coins * 0.80f);
                coins = 0;
                uiManager.UpdateCoinText(coins, bankedCoins);
                audioSourcePlayer.PlayOneShot(coinPickupClip, 10f);
                Debug.Log("Banked coins: " + bankedCoins);
            }
        } else if (gameObject.tag == "RedVault" && redKey)
        {
            redKey = false;
            iTween.MoveTo(gameObject, gameObject.transform.position + new Vector3(0, 3f, 0), 2f);
            audioSourcePlayer.PlayOneShot(vaultOpenClip, 10f);
        } else if (gameObject.tag == "GreenVault" && greenKey)
        {
            greenKey = false;
            iTween.MoveTo(gameObject, gameObject.transform.position + new Vector3(0, 3f, 0), 2f);
            audioSourcePlayer.PlayOneShot(vaultOpenClip, 10f);
        } else if (gameObject.tag == "BlueVault" && blueKey)
        {
            blueKey = false;
            iTween.MoveTo(gameObject, gameObject.transform.position + new Vector3(0, 3f, 0), 2f);
            audioSourcePlayer.PlayOneShot(vaultOpenClip, 10f);
        }
    }

    void OnGameStart()
    {
        StartCoroutine(CountdownTimer());
    }

    void OnPickupSand()
    {
        sand++;
        uiManager.UpdateSandText(sand);
        StartCoroutine(uiManager.PickupItemText("+1 Sand"));
        audioSourcePlayer.PlayOneShot(genericPickupClip, 10f);
        Debug.Log("Sand after pickup: " + sand);
    }

    void OnPickupCoins(int value)
    {
        coins += value;
        uiManager.UpdateCoinText(coins, bankedCoins);
        StartCoroutine(uiManager.PickupItemText("+" + value + " Coins"));
        audioSourcePlayer.PlayOneShot(coinPickupClip, 10f);
        Debug.Log(value + " coins picked up. Balance: " + coins);
    }

    void OnPickupKey(int value)
    {
        //0 red, 1 green, 2 blue
        if (value == 0) { 
            redKey = true;
            StartCoroutine(uiManager.PickupItemText("+1 Red Vault Key"));
        }
        else if (value == 1) { 
            greenKey = true;
            StartCoroutine(uiManager.PickupItemText("+1 Green Vault Key"));
        }
        else if (value == 2) { 
            blueKey = true;
            StartCoroutine(uiManager.PickupItemText("+1 Blue Vault Key"));
        }
        else { Debug.Log("Unknown key value."); }
        audioSourcePlayer.PlayOneShot(keyPickupClip, 10f);
    }

    void OnPortalEnter()
    {
        StopCoroutine(CountdownTimer());
        Destroy(spotlight);
        int finalCoins = coins + bankedCoins;
        audioSourcePlayer.PlayOneShot(exitPortalClip, 50f);
        uiManager.GameFinished(finalCoins, true); //true for portalExit
    }

    void GameOver()
    {
        Destroy(spotlight);
        uiManager.GameFinished(bankedCoins, false); //false for no portalExit
    }

    IEnumerator CountdownTimer()
    {
        while (seconds >= 0)
        {
            if (seconds % 15 == 0 || seconds <= 15)
            {
                audioSourcePlayer.PlayOneShot(clockTickClip, 10f);
            }

            if (seconds < 60)
            {
                timerText.text = seconds.ToString();
            } else
            {
                int minutes = seconds / 60;
                int remainderSeconds = seconds % 60;
                if (remainderSeconds <= 9) {
                    timerText.text = minutes.ToString() + ":0" + remainderSeconds.ToString();
                } else
                {
                    timerText.text = minutes.ToString() + ":" + remainderSeconds.ToString();
                }
            }
            yield return new WaitForSeconds(1);
            seconds--;
        }
        GameOver();
    }
}
