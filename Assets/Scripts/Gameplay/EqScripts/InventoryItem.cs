using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory item",menuName ="Create Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public Sprite sprite; 
    public string name;
    public string description;
    public int spaceUsage;
    public ItemType myItemType;
    public List<GameObject> _cards = new List<GameObject>();
    public enum ItemType
    {
        Head,
        Chest,
        Legs,
        Boots,
        Weapon,
        Other
    }
}
