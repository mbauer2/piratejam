using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject gameMenuCanvas;
    [SerializeField] private TextMeshProUGUI energyTracker;
    [SerializeField] private TextMeshProUGUI clock;

    [SerializeField] private GameObject pauseMenuCanvas;



    [SerializeField] private GameObject playerObject;
    private PlayerCharacter playerCharacter;

    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = playerObject.GetComponent<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCharacter != null)
        {
            energyTracker.text = playerCharacter.GetEnergy().ToString();

        }
    }
}
