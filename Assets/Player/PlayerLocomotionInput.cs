using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerActions
{
    public PlayerControls PlayerControls { get; private set; }

    public Vector2 moveInput;

    public Vector2 rotateInput;

    public bool isJumping;
    public bool isSprinting;

    public PlayerController playerController;
    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.Player.Enable();
        PlayerControls.Player.SetCallbacks(this);
    }

    private void OnDisable()
    {
        PlayerControls.Player.Disable();
        PlayerControls.Disable();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        
            moveInput = context.ReadValue<Vector2>();
        


    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
            rotateInput = context.ReadValue<Vector2>();
        
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started){
            playerController.EquipThrowable();
        }
        if (context.canceled)
        {
            playerController.DropThrowable();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isJumping = true;

        }
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
    }

    public void OnNext(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed;
    }
}
