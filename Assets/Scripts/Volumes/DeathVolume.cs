using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVolume : MonoBehaviour
{
    private float processTime = 1;
    private float processTimeLeft = 0;

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
        if (processTimeLeft <= 0 && other.gameObject.GetComponent<PlayerCharacter>() != null)
        {
            processTimeLeft = processTime;
            GameController.Instance.SpawnPlayer();
        }
    }
}
