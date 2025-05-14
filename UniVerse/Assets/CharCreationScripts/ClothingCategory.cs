using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ClothingCategory", menuName = "Character Creator/Clothing Category")]
public class ClothingCategory : ScriptableObject
{
    public string CategoryName; // Name of the category (e.g., "Top Wear")
    public List<ClothingItem> Items; // List of items in this category
}
