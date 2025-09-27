using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResourceItem : MonoBehaviour
{
    public ResourceType resourceType; // Chọn loại tài nguyên cho vật thể này
    public int amount = 1; // Số lượng tài nguyên nhận được

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component not found on this object!");
            return;
        }

        // Đăng ký sự kiện khi vật thể được nhặt (grab)
        grabInteractable.selectExited.AddListener(OnResourceGrabbed);
    }

    private void OnResourceGrabbed(SelectExitEventArgs args)
    {
        // Khi người chơi thả vật thể, thêm tài nguyên và hủy đối tượng
        // Sử dụng selectExited để vật thể biến mất khi người chơi thả tay cầm, tránh bị "giật"
        if (InventoryManager.instance != null)
        {
            InventoryManager.instance.AddResource(resourceType, amount);
        }

        // Hủy đối tượng vật thể
        Destroy(gameObject);
    }
}