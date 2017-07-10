using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabaseMk2 : MonoBehaviour {
    public List<ItemsMk2> items = new List<ItemsMk2>();

    void Start()
    {
        items.Add(new ItemsMk2("Rock", 0, "Rock or stone is a natural substance, a solid aggregate of one or more minerals or mineraloids. For example, granite, a common rock, is a combination of the minerals quartz, feldspar and biotite. The Earth's outer solid layer, the lithosphere, is made of rock.", 1, 2, ItemsMk2.ItemType.Weapon,3));
        items.Add(new ItemsMk2("Sanya", 1, "A simple cock. From russian Pitooh", 1, 2, ItemsMk2.ItemType.Weapon, 4));
    }
}
