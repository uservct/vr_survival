using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class BonfireInteractor : MonoBehaviour
{
    public Firepit firepit;
    public CraftingManager craftingManager;
    public GameObject mushroomPrefab;
    public Transform spawnPoint;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.activated.AddListener(OnActivated);

        // 🧩 Tự động gán CraftingManager từ Singleton
        if (craftingManager == null && CraftingManager.instance != null)
            craftingManager = CraftingManager.instance;
    }

    public void OnActivated(ActivateEventArgs args)
    {
        // 1️⃣ Kiểm tra bếp bật chưa
        if (firepit == null || !firepit.IsLit)
        {
            Debug.Log("🔥 Bếp chưa bật, không thể nấu!");
            return;
        }

        // 2️⃣ Kiểm tra có nấm trong kho không
        if (craftingManager == null || craftingManager.mushroomCount <= 0)
        {
            Debug.Log("⚠️ Không có nấm trong kho!");
            return;
        }

        // 3️⃣ Giảm nấm trong kho + cập nhật UI
        craftingManager.mushroomCount--;
        craftingManager.UpdateUI();

        // 4️⃣ Spawn nấm ra vị trí
        if (mushroomPrefab != null && spawnPoint != null)
        {
            Instantiate(mushroomPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("🍄 Đã đặt nấm lên bếp để nấu!");
        }
        else
        {
            Debug.LogError("❌ Chưa gán prefab hoặc spawnPoint!");
        }
    }
}
