using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform capsuleRef;
    [SerializeField] private Transform footPoint;
    private InputManager inputManager;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity;

    private Vector2 moveDir;
    private Vector2 lookDir;

    private Vector3 newPos;
    private Vector3 currentRot;

    private void Awake()
    {
        if (!GetComponent<PhotonView>().IsMine) return;

        inputManager = gameObject.AddComponent<InputManager>();
    }

    private void Start()
    {
        if (!GetComponent<PhotonView>().IsMine) return;

        DestroyOtherCameras();
        
        inputManager.LockCursor(true);
       // InvokeRepeating("DisplayFPS", 1f, 1f);
    }

    private void DestroyOtherCameras()
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject player in cameras)
        {
            if (PhotonView.Get(player).IsMine == false)
            {
                Destroy(player.GetComponent<Camera>().gameObject);
            }
        }
    }

    private void Update()
    {
        if (!GetComponent<PhotonView>().IsMine) return;

        MovementAndLook();
        HandleJump();
    }

    private void MovementAndLook()
    {
        speed = (inputManager.sprint.IsPressed()) ? runSpeed : walkSpeed;

        moveDir = inputManager.move.ReadValue<Vector2>();
        lookDir = inputManager.look.ReadValue<Vector2>();

        currentRot = rb.rotation.eulerAngles;

        newPos = ((capsuleRef.transform.forward * moveDir.y) + (capsuleRef.transform.right * moveDir.x)) * speed * Time.deltaTime;

        currentRot.y += lookDir.x * mouseSensitivity * Time.deltaTime;

        rb.Move(rb.position + newPos, Quaternion.Euler(currentRot));

        cam.transform.Rotate(Vector3.left * lookDir.y * mouseSensitivity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (inputManager.jump.WasPressedThisFrame() && IsGrounded())
        {
            rb.AddForce(capsuleRef.transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(
            footPoint.position,
            -footPoint.up,
            groundCheckDistance,
            groundLayer
        );
    }

    private void DisplayFPS()
    {
        Debug.Log("FPS: " + 1 / Time.deltaTime);
    }
}
