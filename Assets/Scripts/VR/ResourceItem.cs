using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResourceItem : MonoBehaviour
{
    public CraftingManager.ResourceType resourceType;
    public int amount = 1;
    public bool isTool = false;

    [Header("Âm thanh nhặt đồ")]
    public AudioClip pickupSound;
    [Range(0f, 1f)] public float pickupVolume = 1f;

    [Header("Âm thanh ăn và uống")]
    public AudioClip eatSound;
    public AudioClip drinkSound;
    [Range(0f, 1f)] public float consumeVolume = 1f;

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
            PlayEatSound();

            PlayerStats stats = FindObjectOfType<PlayerStats>();
            if (stats != null)
            {
                stats.AddHunger(20f);
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
            PlayPickupSound();

            if (CraftingManager.instance != null)
            {
                CraftingManager.instance.AddResource(CraftingManager.ResourceType.Mushroom, amount);
                Debug.Log("🍄 Nhặt nấm sống → +1 Mushroom");
            }

            gameObject.SetActive(false);
            return;
        }

        // 🥥 Dừa → uống
        if (tagName == "coconut")
        {
            PlayDrinkSound();

            PlayerStats stats = FindObjectOfType<PlayerStats>();
            if (stats != null)
            {
                stats.AddThirst(15f);
                Debug.Log("🥥 Uống nước dừa → +15 Thirst");
            }
            else
            {
                Debug.LogWarning("⚠️ PlayerStats not found!");
            }

            Destroy(gameObject);
            return;
        }

        // 🪵 Gỗ, đá → cộng kho + âm thanh nhặt
        if (CraftingManager.instance != null)
            CraftingManager.instance.AddResource(resourceType, amount);

        PlayPickupSound();
        Destroy(gameObject);
    }

    private void PlayPickupSound()
    {
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);
    }

    private void PlayEatSound()
    {
        if (eatSound != null)
            AudioSource.PlayClipAtPoint(eatSound, transform.position, consumeVolume);
    }

    private void PlayDrinkSound()
    {
        if (drinkSound != null)
            AudioSource.PlayClipAtPoint(drinkSound, transform.position, consumeVolume);
    }
}
