using UnityEngine;

public class Tree : MonoBehaviour
{
    public int health = 5;          // số hit cần để đốn
    public GameObject woodPrefab;   // prefab gỗ rơi ra
    public int woodDrop = 3;        // số lượng gỗ rơi

    public void Chop()
    {
        health--;
        Debug.Log("Tree hit! HP = " + health);

        if (health <= 0)
        {
            // Spawn gỗ tại vị trí gốc cây
            for (int i = 0; i < woodDrop; i++)
            {
                Vector3 dropPos = transform.position + Random.insideUnitSphere * 0.5f;
                dropPos.y = transform.position.y;
                Instantiate(woodPrefab, dropPos, Quaternion.identity);
            }

            Destroy(gameObject); // xoá cây
        }
    }
}
