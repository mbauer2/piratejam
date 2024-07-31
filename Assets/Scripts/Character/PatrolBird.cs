using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PatrolBird : Enemy
{
    private SplineAnimate patrolSpline;
    private SplineContainer patrolContainer;

    [SerializeField] float pursuitSpeed = 5;

    [SerializeField] float pursuitTimeLeft;
    [SerializeField] float pursuitTime;

    private GameObject pursuitTarget;

    private Vector3 lastPatrolPoint;
    private Quaternion lastPatrolAngle;

    private void Awake()
    {
        patrolSpline = GetComponent<SplineAnimate>();
        patrolContainer = patrolSpline.Container;
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

        if ( playerDetector.PlayerDetected && !isPursuing && !isReturning )
        {
            StartPursuit();
        }

        if (isPursuing)
        {
            RaycastHit pursuitHit;
            Vector3 direction = pursuitTarget.transform.position - transform.position;
            LayerMask enemyLayer = 1 >> 7;
            enemyLayer = ~enemyLayer;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, direction, out pursuitHit, Mathf.Infinity, layerMask))
            {
                if (pursuitHit.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
                {
                    ReturnToPatrol();
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, pursuitTarget.transform.position, pursuitSpeed * Time.deltaTime);
                    transform.LookAt(pursuitTarget.transform.position);
                }
            }
        }
        else if ( isReturning )
        {
            transform.position = Vector3.MoveTowards(transform.position, lastPatrolPoint, pursuitSpeed * Time.deltaTime);
            transform.LookAt(lastPatrolPoint);
            if ( Vector3.Distance( transform.position, lastPatrolPoint ) < .02 )
            {
                transform.position = lastPatrolPoint;
                transform.rotation = lastPatrolAngle;
                patrolSpline.Play();
            }
        }
    }

    private void StartPursuit()
    {
        patrolSpline.Pause();
        lastPatrolPoint = transform.position;
        lastPatrolAngle = transform.rotation;
        isPursuing = true;
        pursuitTarget = playerDetector.GetPlayerDetected().gameObject;
        
    }

    private void Attack()
    { 
    
    }

    private void StartStun()
    { 
    
    }

    private void ReturnToPatrol()
    {
        //patrolSpline.Play();
        isPursuing = false;
        isReturning = true;
        pursuitTarget = null;
    }

}
