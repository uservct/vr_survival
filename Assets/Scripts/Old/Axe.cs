using UnityEngine;

public class Axe : MonoBehaviour
{
    [Header("Cấu hình chặt cây")]
    [Tooltip("Bán kính vùng kiểm tra quanh đầu rìu")]
    public float checkRadius = 0.15f;

    [Tooltip("Ngưỡng tốc độ tối thiểu để tính là chặt trúng (m/s)")]
    public float speedThreshold = 1.5f;

    [Tooltip("Thời gian giữa 2 lần chặt (giây)")]
    public float hitCooldown = 0.3f;

    [Tooltip("Layer chứa các cây")]
    public LayerMask treeLayer;

    [Header("Âm thanh")]
    [Tooltip("Âm thanh phát khi chặt trúng cây")]
    public AudioClip chopSound;

    [Tooltip("Âm lượng phát âm thanh")]
    [Range(0f, 1f)]
    public float volume = 1f;

    private Vector3 prevPos;
    private float lastHitTime;
    private AudioSource audioSource;

    void Start()
    {
        prevPos = transform.position;

        // Tạo AudioSource nếu chưa có
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.spatialBlend = 1f; // âm thanh 3D
        audioSource.playOnAwake = false;

        Debug.Log("🪓 Axe khởi động!");
    }

    void Update()
    {
        // Tính tốc độ đầu rìu (mỗi frame)
        float speed = (transform.position - prevPos).magnitude / Time.deltaTime;
        prevPos = transform.position;

        // Nếu chưa hết cooldown thì bỏ qua
        if (Time.time - lastHitTime < hitCooldown) return;

        // Kiểm tra vùng xung quanh đầu rìu
        Collider[] hits = Physics.OverlapSphere(transform.position, checkRadius, treeLayer);
        if (hits.Length == 0) return;

        // Lặp qua các đối tượng bị đụng
        foreach (var hit in hits)
        {
            Tree tree = hit.GetComponent<Tree>();
            if (tree != null && speed >= speedThreshold)
            {
                Debug.Log($"🌲 Chặt trúng cây {hit.name} | tốc độ: {speed:0.00} m/s");
                tree.TakeDamage(1);
                lastHitTime = Time.time;

                // Phát âm thanh
                if (chopSound != null)
                    audioSource.PlayOneShot(chopSound, volume);

                // Ngắt vòng để không chặt nhiều cây 1 lúc
                break;
            }
        }
    }

    // Vẽ vùng kiểm tra trong Scene
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
