using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static bool MoveHeld;

    public static Vector2 MoveDirection;
    private InputAction moveAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        moveAction = PlayerInput.actions.FindAction("Move");
    }

    private void Update()
    {
        MoveDirection = moveAction.ReadValue<Vector2>();
        MoveHeld = moveAction.IsPressed();
    }
}
