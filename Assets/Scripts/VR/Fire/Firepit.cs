using UnityEngine;

public class Firepit : MonoBehaviour
{
    public GameObject fireFX;  // object ngọn lửa (đã tắt sẵn)
    private bool isLit = false;

    public void ToggleFire()
    {
        isLit = !isLit;

        if (fireFX != null)
            fireFX.SetActive(isLit);

        Debug.Log(isLit ? "🔥 Firepit lit!" : "❌ Firepit extinguished!");
    }
}
