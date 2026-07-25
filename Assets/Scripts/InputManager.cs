using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    [HideInInspector] public InputAction move;
    [HideInInspector] public InputAction look;
    [HideInInspector] public InputAction sprint;
    [HideInInspector] public InputAction jump;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        move = playerInput.actions.FindAction("Move");
        look = playerInput.actions.FindAction("Look");
        jump = playerInput.actions.FindAction("Jump");
        sprint = playerInput.actions.FindAction("Sprint");
    }

    public void LockCursor(bool locked)
    {
        Cursor.lockState = (locked) ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
