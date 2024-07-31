using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject detector;
    protected PlayerDetector playerDetector;
    protected bool isPursuing;
    protected bool isReturning;

    // Start is called before the first frame update
    void Start()
    {
        playerDetector = detector.GetComponent<PlayerDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
