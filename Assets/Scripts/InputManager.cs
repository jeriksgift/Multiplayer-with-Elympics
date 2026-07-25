using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInput playerInput;

    [HideInInspector] public InputAction move;
    [HideInInspector] public InputAction look;
    [HideInInspector] public InputAction sprint;
    [HideInInspector] public InputAction jump;
    [HideInInspector] public InputAction fire;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        playerInput = GetComponent<PlayerInput>();
        move = playerInput.actions.FindAction("Move");
        look = playerInput.actions.FindAction("Look");
        jump = playerInput.actions.FindAction("Jump");
        sprint = playerInput.actions.FindAction("Sprint");
        fire = playerInput.actions.FindAction("Fire");
    }

    public void LockCursor(bool locked)
    {
        Cursor.lockState = (locked) ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
