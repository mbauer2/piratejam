using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private GameObject gameMenuCanvas;
    [SerializeField] private TextMeshProUGUI energyTracker;
    [SerializeField] private TextMeshProUGUI clock;
    [SerializeField] private Slider stamSlider;
    [SerializeField] private Image stamBar;
    [SerializeField] private Image dedImage;

    [SerializeField] private float currentFadeSpeed;
    [SerializeField] private float fadeTimeLeft;

    [SerializeField] private GameObject pauseMenuCanvas;

    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private TextMeshProUGUI promptText;

    [SerializeField] private GameObject conversationPane;
    [SerializeField] private TextMeshProUGUI convoText;

    [SerializeField] private GameObject playerObject;
    private PlayerCharacter playerCharacter;

    private Vector3 spawnLocation;

    private void Awake()
    {
        if ( Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = playerObject.GetComponent<PlayerCharacter>();
        spawnLocation = playerCharacter.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCharacter != null)
        {
            energyTracker.text = "Orbs:" + playerCharacter.GetEnergy().ToString() + "\nStamina: ";
            stamBar.fillAmount = playerCharacter.GetCurrentStamina() / playerCharacter.GetMaxStamina();
            if (playerCharacter.ShouldShowPrompt())
            {
                promptText.text = playerCharacter.GetInteracbleObjectInRange().GetPrompt();
                interactPrompt.SetActive(true);
            }
            else
            {
                interactPrompt.SetActive(false);
            }
            if (playerCharacter.ShouldShowConversation())
            {
                convoText.text = playerCharacter.GetInteracbleObjectInRange().GetConversation();
                conversationPane.SetActive(true);
            }
            else
            {
                conversationPane.SetActive(false);
            }

            if ( fadeTimeLeft > 0)
            {
                fadeTimeLeft -= Time.deltaTime;
                if ( fadeTimeLeft < 0 )
                {
                    fadeTimeLeft = 0;
                }
                dedImage.color = new Color( dedImage.color.r, dedImage.color.g, dedImage.color.b, 1 - (fadeTimeLeft / currentFadeSpeed));


            }
        }
    }

    public void SpawnPlayer( bool triggerFade = true )
    {
        if ( triggerFade )
        {
            TriggerDeathFade(0.01f);
        }
        
        playerCharacter.RespawnAt(spawnLocation);
    }

    public bool IsPlayerAlive()
    {
        return playerCharacter.IsAlive();
    }

    public void SetSpawnLocation( Transform newLocation )
    {
        spawnLocation = newLocation.position;
    }
    public Vector3 GetSpawnLocation()
    {
        return spawnLocation;
    }

    public void TriggerDeathFade( float fadeSpeed )
    {
        fadeTimeLeft = fadeSpeed;
        currentFadeSpeed = fadeSpeed;
    }

    public void StopFade()
    {
        SetDeathFade(0);
    }

    public void SetDeathFade( float fadeValue )
    {
        dedImage.color = new Color(dedImage.color.r, dedImage.color.g, dedImage.color.b, fadeValue);
    }

}
