using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem; // Input System New

public class CrosshairPickup : MonoBehaviour
{
    [Header("Raycast")]
    public Camera cam;
    public float maxDistance = 5f;
    public LayerMask interactMask;

    [Header("UI")]
    public Button pickupButton;     
    public Image crosshair;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.green;

    [Header("Counter")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI rockText;

    private GameObject target;
    private int woodCount = 0;
    private int rockCount = 0;

    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();

        // Chỉ PC mới lắng nghe phím F
#if UNITY_EDITOR || UNITY_STANDALONE
        controls.Player.Pickup.performed += ctx => Pickup();
#endif
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        if (pickupButton != null)
        {
            pickupButton.gameObject.SetActive(false);
            pickupButton.onClick.AddListener(Pickup); // Mobile
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
            if (hit.collider.CompareTag("wood") || hit.collider.CompareTag("rock"))
            {
                target = hit.collider.gameObject;
                if (pickupButton != null) pickupButton.gameObject.SetActive(true); // Mobile
                if (crosshair != null) crosshair.color = highlightColor;
            }
        }
    }

    void Pickup()
    {
        if (target == null) return;

        if (target.CompareTag("wood"))
        {
            woodCount++;
            if (woodText != null) woodText.text = "x" + woodCount;
        }
        else if (target.CompareTag("rock"))
        {
            rockCount++;
            if (rockText != null) rockText.text = "x" + rockCount;
        }

        Destroy(target);
        if (pickupButton != null) pickupButton.gameObject.SetActive(false);
        if (crosshair != null) crosshair.color = normalColor;
        target = null;
    }
}
