using UnityEngine;

public class Tree : MonoBehaviour
{
    [Header("Máu Cây")]
    public int health = 3;

    [Header("Gỗ rơi ra khi chặt")]
    [Tooltip("Prefab item gỗ sẽ spawn ra khi cây bị chặt đổ")]
    public GameObject woodPrefab;

    [Tooltip("Số lượng gỗ spawn ra")]
    public int woodCount = 3;

    [Tooltip("Khoảng cách ngẫu nhiên quanh cây khi spawn")]
    public float spawnRadius = 0.5f;

    [Header("Âm thanh")]
    [Tooltip("Âm thanh khi cây bị đổ / gỗ spawn ra")]
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

        Debug.Log($"🌳 {gameObject.name} bị đổ! Spawn {woodCount} gỗ.");

        // Spawn gỗ
        if (woodPrefab != null)
        {
            for (int i = 0; i < woodCount; i++)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-spawnRadius, spawnRadius),
                    0.2f,
                    Random.Range(-spawnRadius, spawnRadius)
                );
                Vector3 spawnPos = transform.position + randomOffset;
                Instantiate(woodPrefab, spawnPos, Quaternion.identity);
            }
        }

        // Phát âm thanh
        if (spawnSound != null)
        {
            AudioSource.PlayClipAtPoint(spawnSound, transform.position, spawnVolume);
        }

        // Xóa cây
        Destroy(gameObject);
    }
}
