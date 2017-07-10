using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabaseMk2 : MonoBehaviour {
    public List<ItemsMk2> items = new List<ItemsMk2>();

    void Start()
    {
        items.Add(new ItemsMk2("A rock", 0, "A simple rock", 1, 2, ItemsMk2.ItemType.Weapon));
        items.Add(new ItemsMk2("Sanya", 1, "A simple cock", 1, 2, ItemsMk2.ItemType.Weapon));
    }
}
