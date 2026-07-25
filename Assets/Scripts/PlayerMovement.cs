using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform capsuleRef;
    [SerializeField] private Transform footPoint;
    //private InputManager InputManager.Instance;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float camMinAngle;
    [SerializeField] private float camMaxAngle;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity;

    private Vector2 moveDir;
    private Vector2 lookDir;

    private Vector3 newPos;
    private Vector3 currentRot;
    float pitch;

    private void Awake()
    {
        if (!GetComponent<PhotonView>().IsMine) return;

        //InputManager.Instance = gameObject.AddComponent<InputManager>();
    }

    private void Start()
    {
        DestroyOtherCameras();
        
        InputManager.Instance.LockCursor(true);
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
        speed = (InputManager.Instance.sprint.IsPressed()) ? runSpeed : walkSpeed;

        moveDir = InputManager.Instance.move.ReadValue<Vector2>();
        lookDir = InputManager.Instance.look.ReadValue<Vector2>();

        currentRot = rb.rotation.eulerAngles;

        newPos = ((capsuleRef.transform.forward * moveDir.y) + (capsuleRef.transform.right * moveDir.x)) * speed * Time.deltaTime;

        currentRot.y += lookDir.x * mouseSensitivity * Time.deltaTime;

        rb.Move(rb.position + newPos, Quaternion.Euler(currentRot));

        //float pitch = cam.transform.localEulerAngles.x;
        pitch -= lookDir.y * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, camMinAngle, camMaxAngle);

        Vector3 angles = cam.transform.localEulerAngles;

        // Convert X to signed angle if needed
        float currentY = angles.y;
        float currentZ = angles.z;

        cam.transform.localRotation = Quaternion.Euler(pitch, currentY, currentZ);
    }

    private void HandleJump()
    {
        if (InputManager.Instance.jump.WasPressedThisFrame() && IsGrounded())
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
