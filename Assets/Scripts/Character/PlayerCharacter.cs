using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : Character
{
    private OverworldInput playerControls;
    [SerializeField] private float speed = 200;

    private List<GameObject> collidedObjects;
    private GameObject interactableObject;

    private Rigidbody rb;

    private void Awake()
    {
        playerControls = new OverworldInput();
        playerControls.Player.Fire.performed += Interact;
        collidedObjects = new List<GameObject>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity.x = playerControls.Player.Move.ReadValue<Vector2>().x;
        velocity.y = 0;
        velocity.z = playerControls.Player.Move.ReadValue<Vector2>().y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( !collidedObjects.Contains(collision.gameObject) )
        {
            collidedObjects.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collidedObjects.Contains(collision.gameObject))
        {
            collidedObjects.Remove(collision.gameObject);
        }    
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity * speed * Time.deltaTime;
        if (rb.velocity.sqrMagnitude != 0)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        }

        
        foreach ( GameObject collision in collidedObjects )
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactableObject == null && other.gameObject.GetComponent<Interactable>() != null )
        { 
            
        }
        if ( other.gameObject.GetComponent<CollectibleObject>() != null )
        {
            if ( !other.gameObject.GetComponent<CollectibleObject>().IsProccessingTrigger() )
            {
                other.gameObject.GetComponent<CollectibleObject>().SetIsProcessingTrigger(true);
                bool success = Collect(other.gameObject);
                if ( !success )
                {
                    other.gameObject.GetComponent<CollectibleObject>().SetIsProcessingTrigger(false);
                }
            }
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void Interact(InputAction.CallbackContext context)
    {
        
    }

}
