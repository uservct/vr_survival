using UnityEngine;

public class Tree : MonoBehaviour
{
    [Header("Máu Cây")]
    public int health = 3;

    [Header("Gỗ rơi ra khi chặt")]
    public GameObject woodPrefab;
    public int woodCount = 3;
    public float spawnRadius = 0.5f;

    [Header("Quả dừa (nếu là cây dừa)")]
    public bool isCoconutTree = false;
    public GameObject coconutPrefab;
    public int coconutCount = 2;

    [Header("Âm thanh")]
    public AudioClip spawnSound;
    [Range(0f, 1f)] public float spawnVolume = 1f;

    private bool isDestroyed = false;

    public void TakeDamage(int damage)
    {
        if (isDestroyed) return;

        health -= damage;
        Debug.Log($"[Tree] {gameObject.name} bị chặt! HP còn: {health}");

        if (health <= 0)
        {
            BreakTree();
        }
    }

    private void BreakTree()
    {
        isDestroyed = true;

        Debug.Log($"🌳 {gameObject.name} bị đổ! Spawn {woodCount} gỗ{(isCoconutTree ? " + dừa" : "")}.");

        // Spawn gỗ
        SpawnItems(woodPrefab, woodCount);

        // Spawn dừa (nếu là cây dừa)
        if (isCoconutTree && coconutPrefab != null)
        {
            SpawnItems(coconutPrefab, coconutCount);
        }

        // Âm thanh
        if (spawnSound != null)
        {
            AudioSource.PlayClipAtPoint(spawnSound, transform.position, spawnVolume);
        }

        // Xóa cây
        Destroy(gameObject);
    }

    private void SpawnItems(GameObject prefab, int count)
    {
        if (prefab == null) return;

        for (int i = 0; i < count; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0.5f,
                Random.Range(-spawnRadius, spawnRadius)
            );
            Vector3 spawnPos = transform.position + randomOffset;

            GameObject item = Instantiate(prefab, spawnPos, Quaternion.identity);

            // Thêm lực nhẹ cho vật rơi tự nhiên
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            }
        }
    }
}
