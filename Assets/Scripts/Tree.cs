using UnityEngine;

public class Tree : MonoBehaviour
{
    public int health = 5;          // số hit cần để đốn
    public GameObject woodPrefab;   // prefab gỗ rơi ra
    public int woodDrop = 3;        // số lượng gỗ rơi
    [Header("Audio")]
    public AudioClip breakSound;      // âm thanh khi cây gãy / spawn gỗ
    private AudioSource audioSource;

    void Start()
    {
        // gắn AudioSource tự động
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 3D sound
    }

    public void Chop()
    {
        health--;
        Debug.Log("Tree hit! HP = " + health);

        if (health <= 0)
        {
            ChopDown();
        }
    }

    void ChopDown()
    {

        // spawn gỗ
        for (int i = 0; i < woodDrop; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * 0.5f;
            pos.y = transform.position.y;
            Instantiate(woodPrefab, pos, Quaternion.identity);
        }

        // phát âm thanh cây gãy bằng audio tạm
        if (breakSound != null)
        {
            GameObject tempAudio = new GameObject("TreeBreakSound");
            tempAudio.transform.position = transform.position;
            AudioSource a = tempAudio.AddComponent<AudioSource>();
            a.spatialBlend = 1f;
            a.clip = breakSound;
            a.Play();
            Destroy(tempAudio, breakSound.length); // hủy sau khi phát xong
        }

        // xóa cây ngay lập tức
        Destroy(gameObject);
    }
}

