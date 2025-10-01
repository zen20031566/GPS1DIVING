using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static bool MoveHeld;
    public static Vector2 MoveDirection;

    public static bool InteractPressed;
    public static bool EscPressed;

    //Input Actions
    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction escAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        moveAction = PlayerInput.actions.FindAction("Movement");
        interactAction = PlayerInput.actions.FindAction("Interact");
        escAction = PlayerInput.actions.FindAction("Esc");
    }

    private void Update()
    {
        MoveDirection = moveAction.ReadValue<Vector2>();
        MoveHeld = moveAction.IsPressed();

        InteractPressed = interactAction.WasPressedThisFrame();

        EscPressed = escAction.WasPressedThisFrame();
    }
}
