using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResourceItem : MonoBehaviour
{
    public CraftingManager.ResourceType resourceType;
    public int amount = 1;
    public bool isTool = false;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("❌ XRGrabInteractable not found on " + gameObject.name);
            return;
        }

        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (isTool) return;

        string tagName = gameObject.tag;

        // 🍳 Nấm chín → ăn
        if (tagName == "mushroom_cooked")
        {
            PlayerStats stats = FindObjectOfType<PlayerStats>();
            if (stats != null)
            {
                stats.AddHunger(20f); // tùy chỉnh giá trị
                stats.Heal(5f);
                Debug.Log("😋 Ăn nấm chín → +20 Hunger, +5 Health");
            }
            else
            {
                Debug.LogWarning("⚠️ PlayerStats not found!");
            }

            Destroy(gameObject);
            return;
        }

        // 🍄 Nấm sống → cộng kho
        if (tagName == "mushroom_raw")
        {
            if (CraftingManager.instance != null)
            {
                CraftingManager.instance.AddResource(CraftingManager.ResourceType.Mushroom, amount);
                Debug.Log("🍄 Nhặt nấm sống → +1 Mushroom");
            }

            gameObject.SetActive(false);
            return;
        }

        // 🥥 Dừa → tăng khát nước
        if (tagName == "coconut")
        {
            PlayerStats stats = FindObjectOfType<PlayerStats>();
            if (stats != null)
            {
                stats.AddThirst(30f); // tăng 30 thirst
                Debug.Log("🥥 Uống nước dừa → +30 Thirst");
            }
            else
            {
                Debug.LogWarning("⚠️ PlayerStats not found!");
            }

            Destroy(gameObject);
            return;
        }
        // 🪵 Gỗ, đá → cộng kho + xóa
        if (CraftingManager.instance != null)
            CraftingManager.instance.AddResource(resourceType, amount);

        Destroy(gameObject);
    }
}
