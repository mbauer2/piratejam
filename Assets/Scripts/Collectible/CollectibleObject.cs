using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{

    [SerializeField] private Collectible collectible;
    private bool isProcessingTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIsProcessingTrigger( bool isProcessing )
    {
        isProcessingTrigger = isProcessing;
    }

    public bool IsProccessingTrigger()
    {
        return isProcessingTrigger;
    }

    public Collectible GetCollectible()
    {
        return collectible;
    }
}
