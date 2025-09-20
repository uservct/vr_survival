using UnityEngine;
using TMPro;

public class PickupController : MonoBehaviour
{
    public float pickupRange = 5f;         // khoảng cách ray
    public LayerMask interactLayer;        // layer vật thể có thể nhặt
    public TextMeshProUGUI woodText;       // UI số gỗ
    public TextMeshProUGUI rockText;       // UI số đá

    private int woodCount = 0;
    private int rockCount = 0;

    void Update()
    {
        // Khi tap màn hình
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            TryPickup();
        }
    }

    void TryPickup()
    {
        // Ray từ tâm camera (giữa màn hình)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); 
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, interactLayer))
        {
            if (hit.collider.CompareTag("Wood"))
            {
                woodCount++;
                Destroy(hit.collider.gameObject);
                woodText.text = "x" + woodCount;
            }
            else if (hit.collider.CompareTag("Rock"))
            {
                rockCount++;
                Destroy(hit.collider.gameObject);
                rockText.text = "x" + rockCount;
            }
        }
    }
}
