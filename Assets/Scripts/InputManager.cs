using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static bool MoveHeld;
    public static Vector2 MoveDirection;

    public static bool InteractPressed;

    //Input Actions
    private InputAction moveAction;
    private InputAction interactAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        moveAction = PlayerInput.actions.FindAction("Move");
        interactAction = PlayerInput.actions.FindAction("Interact");
    }

    private void Update()
    {
        MoveDirection = moveAction.ReadValue<Vector2>();
        MoveHeld = moveAction.IsPressed();

        InteractPressed = interactAction.WasPressedThisFrame();
    }
}
