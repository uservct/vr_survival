using UnityEngine;
using UnityEngine.InputSystem;

public class UIActivator : MonoBehaviour
{
    // Kéo đối tượng Canvas hoặc Panel vào ô này trong Inspector
    public GameObject uiToToggle; 

    // Gán nút từ Input Actions vào ô này
    public InputActionProperty toggleAction; 

    private void OnEnable()
    {
        if (toggleAction.action != null)
        {
            toggleAction.action.Enable();
            toggleAction.action.performed += ToggleUI;
        }
    }

    private void OnDisable()
    {
        if (toggleAction.action != null)
        {
            toggleAction.action.performed -= ToggleUI;
            toggleAction.action.Disable();
        }
    }

    private void ToggleUI(InputAction.CallbackContext context)
    {
        // Đảo ngược trạng thái hoạt động của UI
        if (uiToToggle != null)
        {
            uiToToggle.SetActive(!uiToToggle.activeSelf);
        }
    }
}