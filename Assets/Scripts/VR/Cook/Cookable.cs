using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Cookable : MonoBehaviour
{
    [Header("Cooking Settings")]
    public float cookTime = 3f;
    private float currentTime = 0f;
    private bool isCooking = false;
    private bool isCooked = false;

    [Header("Prefabs")]
    public GameObject rawPrefab;     // ✅ Gán chính prefab nấm sống (tùy chọn, không dùng)
    public GameObject cookedPrefab;  // ✅ Prefab nấm chín

    [Header("Nutrition")]
    public float hungerValue = 20f;
    public float healthValue = 5f;

    void Update()
    {
        if (isCooking && !isCooked)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= cookTime)
                CookDone();
        }
    }

    public void StartCooking()
    {
        if (!isCooked)
        {
            isCooking = true;
            Debug.Log($"🍳 Bắt đầu nấu {gameObject.name}");
        }
    }

    public void StopCooking()
    {
        isCooking = false;
    }

    private void CookDone()
    {
        isCooking = false;
        isCooked = true;

        Debug.Log($"✅ {gameObject.name} đã chín!");

        // 🔹 Spawn prefab nấm chín
        if (cookedPrefab != null)
        {
            Instantiate(cookedPrefab, transform.position, transform.rotation);
            Debug.Log("🍄 Spawn nấm chín!");
        }

        // 🔹 Xóa nấm sống cũ
        Destroy(gameObject);
    }
    public bool IsCooked => isCooked;
}
