using UnityEngine;
using TMPro;

public class CraftingManager : MonoBehaviour
{
    // Thêm static instance để các script khác có thể truy cập
    public static CraftingManager instance;

    // Enum để định nghĩa các loại tài nguyên
    public enum ResourceType { Wood, Stone }

    [Header("Refs")]
    public Transform player;          // PlayerCapsule hoặc XR Origin
    public Transform buildPoint;      // Empty đặt trước mặt player
    public Transform buildParent;     // Chứa các object đã dựng (optional)

    [Header("Inventory UI")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI rockText;

    [Header("Counts")]
    public int woodCount = 0;
    public int rockCount = 0;

    [Header("Grounding (optional)")]
    public LayerMask groundMask = ~0;   // layer mặt đất
    public float dropFromHeight = 2f;   // raycast từ trên xuống

    void Awake()
    {
        // Khởi tạo Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateUI();
    }

    // Thêm tài nguyên
    public void AddResource(ResourceType type, int amount)
    {
        if (type == ResourceType.Wood)
        {
            woodCount += amount;
        }
        else if (type == ResourceType.Stone)
        {
            rockCount += amount;
        }
        UpdateUI();
    }

    // Thử dựng công trình từ Recipe
    public void TryBuild(CraftingRecipe recipe)
    {
        if (recipe == null || recipe.prefab == null) return;

        if (woodCount < recipe.woodRequired || rockCount < recipe.rockRequired)
        {
            Debug.Log("Không đủ nguyên liệu để dựng " + recipe.recipeName);
            return;
        }

        // Trừ nguyên liệu
        woodCount -= recipe.woodRequired;
        rockCount -= recipe.rockRequired;
        UpdateUI();

        // Tính vị trí spawn
        Vector3 basePos = (buildPoint != null)
            ? buildPoint.position
            : (player != null ? player.position + player.forward * 2f : Vector3.zero);

        // Raycast xuống đất để bếp dính địa hình
        Vector3 rayStart = basePos + Vector3.up * dropFromHeight;
        Vector3 spawnPos = basePos;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, dropFromHeight + 5f, groundMask, QueryTriggerInteraction.Ignore))
        {
            spawnPos = hit.point;
        }

        Quaternion spawnRot = (player != null)
            ? Quaternion.Euler(0f, player.eulerAngles.y, 0f)
            : Quaternion.identity;

        var parent = buildParent != null ? buildParent : null;
        Instantiate(recipe.prefab, spawnPos, spawnRot, parent);
    }

    void UpdateUI()
    {
        if (woodText) woodText.text = "x" + woodCount;
        if (rockText) rockText.text = "x" + rockCount;
    }
}
