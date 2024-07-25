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

    [SerializeField] private GameObject pauseMenuCanvas;

    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private TextMeshProUGUI promptText;

    [SerializeField] private GameObject conversationPane;
    
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
            energyTracker.text = playerCharacter.GetEnergy().ToString();
            if (playerCharacter.ShouldShowPrompt())
            {
                promptText.text = playerCharacter.GetInteracbleObjectInRange().GetPrompt();
                interactPrompt.SetActive(true);
            }
            else
            {
                interactPrompt.SetActive(false);
            }
        }
    }

    public void SpawnPlayer()
    {
        playerCharacter.RespawnAt(spawnLocation);
    }


    public void SetSpawnLocation( Transform newLocation )
    {
        spawnLocation = newLocation.position;
    }
    public Vector3 GetSpawnLocation()
    {
        return spawnLocation;
    }


}
