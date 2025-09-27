using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Input System New

public class CraftingUIController : MonoBehaviour
{
    [Header("Refs")]
    public GameObject craftingPanel;     // CraftingPanel
    public Button openBuildButton;       // Nút mở (mobile)
    public Button closeButton;           // Nút đóng (X)

    [Header("Ẩn các UI khi mở menu")]
    public GameObject joystickRoot;
    public GameObject crosshairUI;
    public GameObject pickupButton;

    private bool isOpen = false;

    // Input System
    private InputAction toggleAction;

    void Awake()
    {
        // Tạo action cho phím C
        toggleAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/c");
    }

    void OnEnable()
    {
        toggleAction.Enable();
        toggleAction.performed += ctx => Toggle();
    }

    void OnDisable()
    {
        toggleAction.performed -= ctx => Toggle();
        toggleAction.Disable();
    }

    void Start()
    {
        if (openBuildButton != null)
            openBuildButton.onClick.AddListener(() => SetOpen(true));

        if (closeButton != null)
            closeButton.onClick.AddListener(() => SetOpen(false));

        SetOpen(false); // ẩn mặc định
    }

    void Toggle()
    {
        SetOpen(!isOpen);
    }

    public void SetOpen(bool open)
    {
        isOpen = open;
        if (craftingPanel) craftingPanel.SetActive(open);

        // Ẩn/hiện UI khác
        if (joystickRoot) joystickRoot.SetActive(!open);
        if (crosshairUI) crosshairUI.SetActive(!open);
        if (pickupButton) pickupButton.SetActive(!open);

#if UNITY_EDITOR || UNITY_STANDALONE
        Cursor.visible = open;
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
#endif
    }
}
