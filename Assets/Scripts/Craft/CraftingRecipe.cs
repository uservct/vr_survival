using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;      // Tên (Firepit, Tent...)
    public Sprite icon;            // Icon hiển thị trong UI
    public GameObject prefab;      // Prefab sẽ spawn khi build

    [Header("Nguyên liệu yêu cầu")]
    public int woodRequired;
    public int rockRequired;
}
