using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PatrolBird : Enemy
{
    private SplineAnimate patrolSpline;
    private SplineContainer patrolContainer;

    [SerializeField] float pursuitTimeLeft;
    [SerializeField] float pursuitTime;

    private GameObject pursuitTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        LayerMask layerMask = 1 >> 6;
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            detector.transform.position = hit.point;
        }

        if ( isPursuing )
        {
            RaycastHit pursuitHit;
            Vector3 direction = pursuitTarget.transform.position - transform.position;

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, direction, out pursuitHit, Mathf.Infinity, layerMask))
            {
                if ( pursuitHit.transform.gameObject != pursuitTarget )
                {
                    ReturnToPatrol();
                }
            }
        }
    }

    private void StartPursuit()
    {
        patrolSpline.Pause();
        isPursuing = true;
        
    }

    private void Attack()
    { 
    
    }

    private void StartStun()
    { 
    
    }

    private void ReturnToPatrol()
    {
        patrolSpline.Play();
        isPursuing = false;
    }

}
