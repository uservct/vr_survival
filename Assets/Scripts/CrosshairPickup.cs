using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrosshairPickup : MonoBehaviour
{
    [Header("Raycast")]
    public Camera cam;              // Main Camera (XR Origin)
    public float maxDistance = 5f;
    public LayerMask interactMask;  // layer Interactable

    [Header("UI")]
    public Button pickupButton;     // nút Nhặt (UI Button)
    public Image crosshair;         // ảnh Crosshair
    public Color normalColor = Color.white;
    public Color highlightColor = Color.green;

    [Header("Counter")]
    public TextMeshProUGUI woodText;

    private GameObject target;
    private int woodCount = 0;

    void Start()
    {
        if (pickupButton != null)
        {
            pickupButton.gameObject.SetActive(false);
            pickupButton.onClick.AddListener(Pickup);
        }

        if (crosshair != null)
            crosshair.color = normalColor;

        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        target = null;
        if (pickupButton != null) pickupButton.gameObject.SetActive(false);
        if (crosshair != null) crosshair.color = normalColor;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactMask))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);  // 👈 Thêm dòng này

            if (hit.collider.CompareTag("wood"))
            {
                target = hit.collider.gameObject;
                if (pickupButton != null) pickupButton.gameObject.SetActive(true);
                if (crosshair != null) crosshair.color = highlightColor;
            }
        }
    }


    void Pickup()
    {
        if (target == null) return;

        // Tăng số lượng gỗ
        woodCount++;
        if (woodText != null)
            woodText.text = "x" + woodCount;

        // Xoá gỗ trong scene
        Destroy(target);

        // Reset UI
        pickupButton.gameObject.SetActive(false);
        crosshair.color = normalColor;
        target = null;
    }
}
