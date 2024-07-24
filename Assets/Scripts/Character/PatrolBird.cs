using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBird : Enemy
{
    [SerializeField] GameObject detector;
    private PlayerDetector playerDetector;

    private bool isPursuing;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            detector.transform.position = hit.point;
        }
        else
        {

        }
    }

    private void StartPursuit()
    { 
    
    }

    private void Attack()
    { 
    
    }

    private void StartStun()
    { 
    
    }

    private void ReturnToPatrol()
    {
    
    }

}
