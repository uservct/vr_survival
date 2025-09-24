using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Axe : MonoBehaviour
{
    private Transform handSlot;
    private bool isEquipped = false;
    private PlayerControls controls;

    [Header("Axe Settings")]
    public float chopRange = 2.5f;     // khoảng cách chặt
    public LayerMask treeMask;         // layer cây

    public float swingAngle = 45f;   // góc vung
    public float swingSpeed = 10f;   // tốc độ vung

    private bool isSwinging = false;
    void Awake()
    {
        controls = new PlayerControls();

#if UNITY_EDITOR || UNITY_STANDALONE
        // PC: phím R để cất / lấy rìu
        controls.Player.ToggleWeapon.performed += ctx => ToggleEquip();

        // PC: chuột trái để chặt
        controls.Player.Chop.performed += ctx => Swing();
#endif
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    // Khi nhặt rìu
    public void PickUp()
    {
        if (handSlot == null)
        {
            GameObject slotObj = GameObject.Find("HandSlot");
            if (slotObj != null) handSlot = slotObj.transform;
        }

        if (handSlot == null) return;

        transform.SetParent(handSlot);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        isEquipped = true;
    }

    // Toggle bằng phím R
    void ToggleEquip()
    {
        if (handSlot == null) return;

        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();

        if (isEquipped)
        {
            if (renderer) renderer.enabled = false; // ẩn model
            isEquipped = false;
        }
        else
        {
            if (renderer) renderer.enabled = true; // hiện lại model
            isEquipped = true;
        }
    }

    void Swing()
    {
        Debug.Log("Swing called!");  // <- nếu input hoạt động thì sẽ in dòng này

        if (!isEquipped || isSwinging) return;
        StartCoroutine(SwingRoutine());

        Camera cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, chopRange, treeMask))
        {
            Debug.Log("Ray hit from Axe: " + hit.collider.name + " | Tag: " + hit.collider.tag);

            if (hit.collider.CompareTag("tree"))
            {
                Tree tree = hit.collider.GetComponentInParent<Tree>(); // lấy từ parent
                if (tree != null)
                {
                    tree.Chop();
                }
                else
                {
                    Debug.LogWarning("Tree.cs not found on " + hit.collider.name);
                }
            }

        }
    }
    IEnumerator SwingRoutine()
    {
        isSwinging = true;
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = startRot * Quaternion.Euler(-swingAngle, -35f, 0);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * swingSpeed;
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        // trở về
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * swingSpeed;
            transform.localRotation = Quaternion.Slerp(targetRot, startRot, t);
            yield return null;
        }

        isSwinging = false;
    }
}
