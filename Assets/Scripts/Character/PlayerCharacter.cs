using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : Character
{
    private OverworldInput playerControls;
    [SerializeField] private float speed = 20;
    [SerializeField] private float dashMultiplier = 1.5f;
    [SerializeField] private float jumpSpeed = 2;
    [SerializeField] private float rotationSpeed = 90;
    [SerializeField] private float glideFallSpeed = -1;
    [SerializeField] private float fallSpeed = -10;
    [SerializeField] private float slamSpeed = -20;

    private bool jumpPressed = false;
    private bool shouldGlide = false;
    private bool speedUpActive = false;

    private float dashLength = 0.5f;
    private float dashTimeLeft = 0;

    private List<GameObject> collidedObjects;
    private GameObject interactableObject;
    private bool interactPressed = false;

    private CharacterController characterController;

    //private Rigidbody rb;

    private void Awake()
    {
        playerControls = new OverworldInput();
        playerControls.Player.Fire.performed += Interact;
        playerControls.Player.Jump.performed += Jump;
        playerControls.Player.SpeedUp.performed += SpeedUp;
        collidedObjects = new List<GameObject>();
        //rb = gameObject.GetComponent<Rigidbody>();

        characterController = GetComponent<CharacterController>();
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
        inputVelocity.x = playerControls.Player.Move.ReadValue<Vector2>().x;
        inputVelocity.z = playerControls.Player.Move.ReadValue<Vector2>().y;

        if ( dashTimeLeft > 0)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft < 0)
            {
                dashTimeLeft = 0;
                speedUpActive = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if ( !collidedObjects.Contains(collision.gameObject) )
        //{
        //    collidedObjects.Add(collision.gameObject);
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (collidedObjects.Contains(collision.gameObject))
        //{
        //    collidedObjects.Remove(collision.gameObject);
        //}    
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if ( !jumpPressed )
        {
            if (characterController.isGrounded)
            {
                jumpPressed = true;
            }
            else
            {
                if ( !speedUpActive )
                {
                    shouldGlide = !shouldGlide;
                }
            }
        }
    }

    private void SpeedUp(InputAction.CallbackContext context)
    {
        if (!speedUpActive)
        {
            speedUpActive = true;
            jumpPressed = false;
            shouldGlide = false;

            if (characterController.isGrounded)
            {
                dashTimeLeft = dashLength;
            }
            else
            {
                // height check for slam?

            }
        }
    }

    private void FixedUpdate()
    {
        //rb.velocity = velocity * speed * Time.deltaTime;
        if (inputVelocity.sqrMagnitude != 0)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(inputVelocity.normalized * speed * Time.deltaTime, Vector3.up);
        }

        bool wasInAir = false;

        if (characterController.isGrounded)
        {
            velocity = inputVelocity.normalized * speed * Time.deltaTime;
            if (speedUpActive)
            {
                velocity *= dashMultiplier;
            }
            // turnVelocity
            if (jumpPressed)
            {
                velocity.y = jumpSpeed;
                jumpPressed = false;
            }
        }
        else
        {
            wasInAir = true;
        }

        if ( shouldGlide && velocity.y > 0 )
        {
            velocity.y = 0;
        }

        if ( wasInAir && speedUpActive )
        {
            velocity.x = 0;
            velocity.z = 0;
        }

        float currentFallSpeed = fallSpeed;
        if (shouldGlide)
        {
            currentFallSpeed = glideFallSpeed;
        }
        else if ( wasInAir && speedUpActive )
        {
            currentFallSpeed = slamSpeed;
        }

        velocity.y += currentFallSpeed * Time.deltaTime;

        characterController.Move( velocity * speed * Time.deltaTime );
        if ( characterController.isGrounded )
        {
            jumpPressed = false;
            shouldGlide = false;

            if ( wasInAir )
            {
                speedUpActive = false;
                // Landed this frame
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactableObject == null && other.gameObject.GetComponent<Interactable>() != null )
        {
            interactableObject = other.gameObject;
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
        if (interactableObject == other.gameObject.GetComponent<Interactable>() )
        {
            interactableObject = null;
            interactPressed = false;
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if ( interactableObject != null && !interactPressed )
        {
            Interactable interactable = GetInteracbleObjectInRange();
            interactPressed = true;
            if (!interactable.ShouldShowConversation())
            {
                // Consume prize
                Collect( interactable.ConsumePrize() );
                interactable.DeactivateInteractable();
            }
            else
            { 
                // Start Conversation
            }
            
        }
    }

    private Interactable GetInteracbleObjectInRange()
    {
        return interactableObject.GetComponent<Interactable>();
    }

    private bool ShouldShowPrompt()
    {
        return interactableObject != null && !interactPressed;
    }

    private bool ShouldShowConversation()
    {
        return false;
    }

}
