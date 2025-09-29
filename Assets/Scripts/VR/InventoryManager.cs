using UnityEngine;
using TMPro; // Nếu bạn dùng TextMeshPro cho UI

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; // Tạo Singleton Pattern để dễ truy cập

    [Header("Resource Counts")]
    public int woodCount = 0;
    public int stoneCount = 0;

    [Header("UI References")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;

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
        UpdateUI(); // Cập nhật UI ban đầu
    }

    // Hàm để thêm tài nguyên
    public void AddResource(ResourceType type, int amount)
    {
        if (type == ResourceType.Wood)
        {
            woodCount += amount;
        }
        else if (type == ResourceType.Stone)
        {
            stoneCount += amount;
        }
        UpdateUI(); // Cập nhật UI sau khi thêm
    }

    // Hàm cập nhật UI
    void UpdateUI()
    {
        if (woodText != null)
        {
            woodText.text = "" + woodCount;
        }
        if (stoneText != null)
        {
            stoneText.text = "" + stoneCount;
        }
    }
}

// Enum để phân biệt loại tài nguyên
public enum ResourceType { Wood, Stone }    