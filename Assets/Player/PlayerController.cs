using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    private PlayerLocomotionInput playerLocomotionInput;
    private CharacterController characterController;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float moveSpeed = 5f; // walking speed
    [SerializeField] private float sprintSpeed = 7f; // sprinting speed
    [SerializeField] private bool isGrounded;
    private Vector2 moveInput;

    [Header("Rotation")]

    [SerializeField] private float yawRotation = 0f;
    [SerializeField] private float pitchRotation = 0f;

    [SerializeField] private float rotateSpeedX = 0.2f;
    [SerializeField] private float rotateSpeedY = 0.2f;

    private Vector2 rotateInput;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 1f; // height the player can jump
    [SerializeField] private float verticalVelocity = 0f; // speed at which the player is moving vertically (up or down)
    [SerializeField] private float gravity = -20f;
    [SerializeField] private bool isJumping = false;

    [Header("Sprinting")]
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private bool isWalking = false;

    [SerializeField] private float stamina = 100f;
    [SerializeField] private float staminaDrainRate = 10f; // how quickly stamina drains while sprinting
    [SerializeField] private float staminaRegenRate = 5f; // how quickly stamina regenerates when not sprinting
    [SerializeField] private bool canSprint = true;
    [SerializeField] private float minStaminaNeeded = 20f; // minimum stamina needed to start sprinting
    private bool isSprintingInput = false;
    [SerializeField] private bool infiniteStamina = false;
    [SerializeField] private float maxStamina = 100f; // maximum stamina value

    [Header("Physics")]

    [SerializeField] private float forceMagnitude = 1f;

    [SerializeField] private GameObject holdPoint;  
    [SerializeField] private Rigidbody heldObject = null;

    [SerializeField] private float pushForce = 1f;
    [SerializeField] private float pickupRange = 3f; // max distance to pick up obj

    [Header("Layers")]

    private int layermask;

    [Header("Audio")]

    public AudioSource walk;
    public AudioSource sprint;
    public AudioSource jump;
    public AudioSource swoosh;


    void Start()
    {
       Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        speed = moveSpeed;

    }

    private void Awake()
    {
        playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        characterController = GetComponent<CharacterController>();

        layermask = ~LayerMask.GetMask("Player"); // includes all layer masks but the player

}


    // Update is called once per frame

    float nextWalkTime = 0f; // start at 0 so first step can play
    float nextJumpTime = 0f;

    void Update()
    {
        isWalking = moveInput.magnitude > 0.1;
        if (isSprinting && !sprint.isPlaying && Time.time >= nextWalkTime)
        {
            sprint.Play();
            nextWalkTime = Time.time + sprint.clip.length + 0.2f;

        }
        else if (isWalking && !walk.isPlaying && Time.time >= nextWalkTime)
        {
            walk.Play();
            nextWalkTime = Time.time + walk.clip.length + 0.4f;

        }

        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * pickupRange, Color.red);
        if (heldObject != null)
        {
            heldObject.transform.position = holdPoint.transform.position;
        }

        isGrounded = characterController.isGrounded;
        isJumping = playerLocomotionInput.isJumping;
        isSprintingInput = playerLocomotionInput.isSprinting;

        moveInput = playerLocomotionInput.moveInput;
        rotateInput = playerLocomotionInput.rotateInput;

        if (infiniteStamina)
        {
            stamina = maxStamina; // keep stamina at max if infinite stamina is enabled
            canSprint = true; // allow sprinting regardless of stamina

        }

        if (isSprintingInput && canSprint && moveInput.magnitude > 0)
        {
            isSprinting = true;


            speed = sprintSpeed;

            if (!infiniteStamina)
            {
                stamina = Mathf.Clamp(stamina -= staminaDrainRate * Time.deltaTime, 0, maxStamina);

                if (stamina <= 0)
                {
                    canSprint = false;
                }
            }

        }
        else 
        {
            speed = moveSpeed;
            isSprinting = false;


            stamina = Mathf.Clamp(stamina += staminaRegenRate * Time.deltaTime, 0, maxStamina);

            if (stamina >= minStaminaNeeded)
                {
                    canSprint = true;
                }
            
        }
        

        if (isJumping && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jump.Play();
            
        }
        else if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // small negative value to keep the player grounded
        }

        playerLocomotionInput.isJumping = false;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 moveDirection = (transform.forward * moveInput.y) + (transform.right * moveInput.x); // move direction 

        characterController.Move((transform.up * verticalVelocity + moveDirection * speed) * Time.deltaTime);
        pitchRotation -= rotateInput.y * rotateSpeedY;
        yawRotation += rotateInput.x * rotateSpeedX;
        
        pitchRotation = Mathf.Clamp(pitchRotation, -80f, 80f); // prevent the camera from flipping over

        characterController.transform.localRotation = Quaternion.Euler(0, yawRotation, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(pitchRotation, 0, 0);
        


    }

    // obstacle push
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;
        if (rigidbody != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
            swoosh.Play();
        }
    }



    // pickup / throw "Throwable" objects

    private float sphereRadius = 0.15f; // thickness of raycast
    public void EquipThrowable()
    {
        if (heldObject == null)
        {
            RaycastHit hit;
            if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out hit, pickupRange, layermask))
            {

                if (hit.collider.tag.Equals("Throwable"))
                {
                    swoosh.Play();
                    heldObject = hit.rigidbody;
                    heldObject.isKinematic = true;

                    
                }
            }
        }
    }
    public void DropThrowable()
    {
        if (heldObject != null)
        {
            swoosh.Play();
            heldObject.isKinematic = false;
            heldObject.AddForce(playerCamera.transform.forward * pushForce, ForceMode.Impulse);
            heldObject = null;
        }
    }
}
