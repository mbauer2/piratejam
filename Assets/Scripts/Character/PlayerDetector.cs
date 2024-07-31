using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private float processTime = 0.2f;
    private float processTimeLeft = 0;

    private GameObject detectedPlayer;

    public bool PlayerDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (processTimeLeft > 0)
        {
            processTimeLeft -= Time.deltaTime;
            if (processTimeLeft < 0)
            {
                processTimeLeft = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (processTimeLeft <= 0 && other.gameObject.GetComponent<PlayerCharacter>() != null && other.gameObject.GetComponent<PlayerCharacter>().IsAlive() )
        {
            processTimeLeft = processTime;
            PlayerDetected = true;
            detectedPlayer = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (processTimeLeft <= 0 && other.gameObject.GetComponent<PlayerCharacter>() != null)
        {
            processTimeLeft = processTime;
            PlayerDetected = false;
            detectedPlayer = null;
        }
    }


    public PlayerCharacter GetPlayerDetected()
    {
        return detectedPlayer.GetComponent<PlayerCharacter>();
    }
}
