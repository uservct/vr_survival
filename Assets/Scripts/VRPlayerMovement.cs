using UnityEngine;

public class VRPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Joystick joystick;        // Joystick từ Joystick Pack
    public Transform xrCamera;       // Main Camera của XR Origin

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Input từ Joystick
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Hướng theo camera VR
        Vector3 forward = xrCamera.forward;
        Vector3 right = xrCamera.right;

        forward.y = 0; // không làm nhân vật bay
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * vertical + right * horizontal;

        // Di chuyển bằng CharacterController
        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}
