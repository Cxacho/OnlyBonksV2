using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlots : MonoBehaviour
{
    // Start is called before the first frame update
    public bool occupied;
    public GameObject currentItem;
    public mySlotType slot;
    public enum mySlotType
    {
        head,
        chest,
        legs,
        boots,
        primaryWeapon,
            secondaryWeapon
    }
}
