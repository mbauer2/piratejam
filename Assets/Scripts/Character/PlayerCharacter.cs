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
    [SerializeField] private float jumpAscentTime = 1;
    [SerializeField] private float jumpHeldModifier = 0.5f;
    [SerializeField] private float rotationSpeed = 90;
    [SerializeField] private float glideFallSpeed = -1;
    [SerializeField] private float fallSpeed = -10;
    [SerializeField] private float slamSpeed = -20;

    [SerializeField] private float stamRechargeRate = 20;
    [SerializeField] private float baseStamina = 100;
    [SerializeField] private float orbStamValue = 5;
    [SerializeField] private float jumpStamCost = 30;
    [SerializeField] private float glideStamCost = 2;
    [SerializeField] private float dashStamCost = 30;
    [SerializeField] private float slamStamCost = 30;

    private float currentStamina;

    private bool jumpPressed = false;
    private bool shouldGlide = false;
    private bool dashActive = false;
    private bool slamActive = false;

    private float dashLength = 0.5f;
    private float dashTimeLeft = 0;

    private float jumpAscentTimeLeft = 0;

    [SerializeField] private float respawnTimer = 1;
    private float respawnTimeLeft = 0;

    private GameObject interactableObject;
    private bool interactPressed = false;

    private CharacterController characterController;


    private void Awake()
    {
        playerControls = new OverworldInput();
        playerControls.Player.Interact.performed += Interact;
        playerControls.Player.Jump.performed += Jump;
        playerControls.Player.Jump.canceled += JumpReleased;
        playerControls.Player.SpeedUp.performed += SpeedUp;
        playerControls.Player.Fire.performed += Stun;

        characterController = GetComponent<CharacterController>();

        currentStamina = GetMaxStamina();
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
                dashActive = false;
            }
        }

        if ( jumpAscentTimeLeft > 0 )
        {
            jumpAscentTimeLeft -= Time.deltaTime;
            if ( jumpAscentTimeLeft < 0 )
            {
                jumpAscentTimeLeft = 0;
            }
        }

        if (respawnTimeLeft > 0)
        {
            respawnTimeLeft -= Time.deltaTime;
            if (respawnTimeLeft < 0)
            {
                respawnTimeLeft = 0;
                playerControls.Player.Enable();
            }
        }

        float currentMaxStam = GetMaxStamina();

        if ( GetCurrentStamina() < currentMaxStam && !shouldGlide && jumpAscentTimeLeft <= 0 )
        {
            currentStamina = Mathf.Min( currentStamina + stamRechargeRate * Time.deltaTime, currentMaxStam );   
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnCollisionExit(Collision collision)
    {
 
    }

    public bool IsSlamming()
    {
        return slamActive;
    }

    private void ClearFlags()
    {
        slamActive = false;
        dashActive = false;
        jumpPressed = false;
        shouldGlide = false;
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public float GetMaxStamina()
    {
        return baseStamina + energy * orbStamValue;
    }

    private bool SpendStamina( float spendAmount )
    {
        bool success = false;
        if ( currentStamina >= spendAmount )
        {
            success = true;
            currentStamina -= spendAmount;
        }

        return success;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if ( !jumpPressed && !shouldGlide )
        {
            if (characterController.isGrounded && SpendStamina(jumpStamCost) )
            {
                jumpPressed = true;
                jumpAscentTimeLeft = jumpAscentTime;
            }
            else if (!characterController.isGrounded && !slamActive && SpendStamina(glideStamCost))
            {
                shouldGlide = true;
                jumpPressed = false;

            }
        }
    }

    private void JumpReleased(InputAction.CallbackContext context)
    {
        jumpPressed = false;
        shouldGlide = false;
        jumpAscentTimeLeft = 0;
    }

    private void SpeedUp(InputAction.CallbackContext context)
    {
        if (characterController.isGrounded)
        {
            if (!dashActive && SpendStamina(dashStamCost) )
            {
                dashActive = true;
                jumpPressed = false;
                shouldGlide = false;

                dashTimeLeft = dashLength;
            }
        }
        else
        {
            if (!slamActive && SpendStamina(slamStamCost))
            {
                slamActive = true;
                dashActive = false;
                jumpPressed = false;
                shouldGlide = false;
            }
        }

    }

    public void RespawnAt( Vector3 position )
    {
        playerControls.Player.Disable();
        respawnTimeLeft = respawnTimer;
        gameObject.transform.position = position;
    }

    private void FixedUpdate()
    {
        if (inputVelocity.sqrMagnitude != 0)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(inputVelocity.normalized * speed * Time.deltaTime, Vector3.up);
        }

        bool wasInAir = false;

        if (characterController.isGrounded)
        {
            velocity = inputVelocity.normalized * speed * Time.deltaTime;
            if (dashActive)
            {
                velocity *= dashMultiplier;
            }
            if (jumpPressed)
            {
                velocity.y = jumpSpeed;
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

        if ( wasInAir && slamActive )
        {
            velocity.x = 0;
            velocity.z = 0;
        }

        float currentFallSpeed = fallSpeed;
        if (shouldGlide)
        {
            if (SpendStamina(glideStamCost))
            {
                currentFallSpeed = glideFallSpeed;
            }
            else
            {
                shouldGlide = false;
            }
            
        }
        else if (wasInAir && slamActive)
        {
            currentFallSpeed = slamSpeed;
        }
        else if (jumpPressed && jumpAscentTimeLeft > 0)
        {
              currentFallSpeed *= jumpHeldModifier;
        }

        if ( velocity.y * currentFallSpeed < 0 && velocity.y + currentFallSpeed * Time.deltaTime < 0 && jumpPressed )
        {
            /// switch to glide if jump still pressed
            shouldGlide = true;
            jumpPressed = false;
        }

        velocity.y += currentFallSpeed * Time.deltaTime;

        characterController.Move( velocity * speed * Time.deltaTime );
        if ( characterController.isGrounded )
        {
            jumpPressed = false;
            shouldGlide = false;

            if ( wasInAir )
            {
                slamActive = false;
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
        if (interactableObject == other.gameObject )
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
                bool paySuccess;
                if (interactable.GetCost() != null)
                {
                    paySuccess = Pay(interactable.GetCost());
                }
                else
                {
                    paySuccess = true;
                }
                // Consume prize
                if (!paySuccess)
                {
                    interactPressed = false;
                    return;
                }
                bool success = Collect( interactable.GetPrize() );
                if (success )
                {
                    interactable.DeactivateInteractable();
                }
            }
            else
            { 
                // Start Conversation
            }
            
        }
    }

    public Interactable GetInteracbleObjectInRange()
    {
        return interactableObject.GetComponent<Interactable>();
    }

    public bool ShouldShowPrompt()
    {
        return interactableObject != null && !interactPressed;
    }

    public bool ShouldShowConversation()
    {
        return false;
    }

    public void Stun(InputAction.CallbackContext context)
    {

    }

}
