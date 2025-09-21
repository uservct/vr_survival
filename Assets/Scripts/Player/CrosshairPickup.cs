using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

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

    [Header("UI - Fire")]
    public Button fireButton;      // nút icon lửa
    public Image fireIcon;         // nếu muốn chỉnh màu/icon

    [Header("Refs")]
    public CraftingManager craftingManager;

    private GameObject target;
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();

        // PC: nhặt đồ = F
#if UNITY_EDITOR || UNITY_STANDALONE
        controls.Player.Pickup.performed += ctx => {
            if (target != null && (target.CompareTag("wood") || target.CompareTag("rock")))
                Pickup(); 
        };

        // PC: đốt bếp = G
        controls.Player.InteractSpecial.performed += ctx => {
            if (target != null && target.CompareTag("firepit"))
            {
                Firepit pit = target.GetComponent<Firepit>();
                if (pit != null) pit.ToggleFire();
            }
        };
#endif
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        if (pickupButton != null)
        {
            pickupButton.gameObject.SetActive(false);
            pickupButton.onClick.AddListener(() =>
            {
                if (target != null)
                {
                    if (target.CompareTag("wood") || target.CompareTag("rock"))
                        Pickup();
                    else if (target.CompareTag("firepit"))
                        target.GetComponent<Firepit>()?.ToggleFire();
                }
            });
        }

        if (fireButton != null)
        {
            fireButton.onClick.AddListener(() =>
            {
                if (target != null && target.CompareTag("firepit"))
                {
                    Firepit pit = target.GetComponent<Firepit>();
                    if (pit != null) pit.ToggleFire();
                }
            });
        }

        if (crosshair != null) crosshair.color = normalColor;
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        target = null;
        if (pickupButton != null) pickupButton.gameObject.SetActive(false);
        if (fireButton != null) fireButton.gameObject.SetActive(false);
        if (crosshair != null) crosshair.color = normalColor;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactMask))
        {
            if (hit.collider.CompareTag("wood") || hit.collider.CompareTag("rock"))
            {
                target = hit.collider.gameObject;
                if (pickupButton != null) pickupButton.gameObject.SetActive(true);
                if (crosshair != null) crosshair.color = highlightColor;
            }
            else if (hit.collider.CompareTag("firepit"))
            {
                target = hit.collider.gameObject;
                if (fireButton != null) fireButton.gameObject.SetActive(true);
                if (crosshair != null) crosshair.color = highlightColor;
            }
        }
    }


    void Pickup()
    {
        if (target == null || craftingManager == null) return;

        if (target.CompareTag("wood"))
        {
            craftingManager.AddWood(1);
            Destroy(target);
        }
        else if (target.CompareTag("rock"))
        {
            craftingManager.AddRock(1);
            Destroy(target);
        }

        if (pickupButton != null) pickupButton.gameObject.SetActive(false);
        if (crosshair != null) crosshair.color = normalColor;
        target = null;
    }
}
