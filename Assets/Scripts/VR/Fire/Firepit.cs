using UnityEngine;

public class Firepit : MonoBehaviour
{
    public GameObject fireFX;
    private bool isLit;

    public void ToggleFire()
    {
        isLit = !isLit;
        fireFX.SetActive(isLit);
        Debug.Log(isLit ? "🔥 Firepit lit!" : "❌ Firepit extinguished!");
    }

    public bool IsLit => isLit; // thêm thuộc tính để CookZone kiểm tra
}
