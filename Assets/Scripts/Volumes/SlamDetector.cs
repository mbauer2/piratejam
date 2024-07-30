using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamDetector : MonoBehaviour
{
    private bool BeenSlammed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( !BeenSlammed && collision.gameObject.GetComponent<PlayerCharacter>() != null )
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>().IsSlamming())
            {
                BeenSlammed = true;
            }
        }
    }

}
