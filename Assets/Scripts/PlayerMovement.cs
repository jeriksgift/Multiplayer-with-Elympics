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

    private PhotonView photonView;

    [Header("Camera Cleanup")]
    [SerializeField] private float cameraSweepInterval = 0.5f;
    private float nextCameraSweep;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        //InputManager.Instance = gameObject.AddComponent<InputManager>();
    }

    private void Start()
    {
        if (!photonView.IsMine) return;

        DestroyOtherCameras();
        InputManager.Instance.LockCursor(true);
    }

    /// <summary>
    /// Removes the camera that belongs to every other networked player so only the
    /// local player's camera renders.
    /// <para>
    /// PhotonView.Get resolves the owning player by walking up the hierarchy (the camera
    /// sits under Player/GFX, two levels below the view). It returns null for cameras that
    /// belong to no player - the Lobby scene's Main Camera is still alive for the frame
    /// the Game scene loads in, because Destroy is deferred - and those must be skipped
    /// rather than dereferenced.
    /// </para>
    /// </summary>
    private void DestroyOtherCameras()
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (GameObject cameraObject in cameras)
        {
            PhotonView owner = PhotonView.Get(cameraObject);

            // Scene cameras are not ours to destroy.
            if (owner == null) continue;

            if (owner.IsMine) continue;

            Destroy(cameraObject);
        }
    }

    /// <summary>
    /// Remote player prefabs arrive after this player's Start, so a single pass in
    /// Start can miss their cameras. Re-sweep on a slow timer until the room settles.
    /// </summary>
    private void SweepForeignCameras()
    {
        if (Time.unscaledTime < nextCameraSweep) return;
        nextCameraSweep = Time.unscaledTime + cameraSweepInterval;

        if (GameObject.FindGameObjectsWithTag("MainCamera").Length > 1)
        {
            DestroyOtherCameras();
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        MovementAndLook();
        HandleJump();
        SweepForeignCameras();
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
