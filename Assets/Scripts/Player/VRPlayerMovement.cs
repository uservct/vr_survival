using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float gravity = -9.81f;
    public float groundedStickForce = -2f;

    [Header("Ground Check")]
    public float groundCheckRadius = 0.5f;
    public LayerMask groundMask = ~0; 
    public float groundProbeOffsetY = 0.05f;

    [Header("Input")]
    public Joystick moveJoystick;   // gán Fixed Joystick
    public Transform xrCamera;      // gán Main Camera của XR Origin

    [Header("Editor Test (PC)")]
    public bool simulateMouseLookInEditor = true;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private float verticalVelocity;
    private float pitch, yaw;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (simulateMouseLookInEditor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
#endif
    }

    void Update()
    {
        HandleMove();
        HandleGravity();
        SimulateMouseLookIfNeeded();
    }

    void HandleMove()
    {
        float h = 0f, v = 0f;

#if UNITY_EDITOR || UNITY_STANDALONE
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Debug.Log($"Input WASD: H={h}, V={v}");
#elif UNITY_ANDROID || UNITY_IOS
        if (moveJoystick != null)
        {
            h = moveJoystick.Horizontal;
            v = moveJoystick.Vertical;
        }
#endif

        Vector3 fwd = xrCamera.forward; fwd.y = 0f; fwd.Normalize();
        Vector3 right = xrCamera.right; right.y = 0f; right.Normalize();

        Vector3 move = (fwd * v + right * h) * moveSpeed;

        Vector3 motion = move * Time.deltaTime + Vector3.up * verticalVelocity * Time.deltaTime;
        controller.Move(motion);
    }

    void HandleGravity()
    {
        Vector3 sphereCenter = transform.position + Vector3.down * (controller.height / 2f - groundProbeOffsetY);

        bool grounded = Physics.CheckSphere(sphereCenter, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);

        if (grounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedStickForce;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    void SimulateMouseLookIfNeeded()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (!simulateMouseLookInEditor || xrCamera == null) return;

        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mx;
        pitch = Mathf.Clamp(pitch - my, -80f, 80f);

        xrCamera.localRotation = Quaternion.Euler(pitch, yaw, 0f);
#endif
    }

    void OnDrawGizmosSelected()
    {
        if (controller == null) return;
        Gizmos.color = Color.yellow;
        Vector3 sphereCenter = transform.position + Vector3.down * (controller.height / 2f - groundProbeOffsetY);
        Gizmos.DrawWireSphere(sphereCenter, groundCheckRadius);
    }
}
