using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemsMk2 {

    public string itemName;
    public int itemID;
    public string itemDesc;
    public Texture2D itemIcon;
    public int itemPower;
    public int itemWeight;
    public ItemType itemType;
    public int itemMaxStack;
    public int itemCurrentStack;

    public enum ItemType
    {
        Weapon,
        Consumable,
        Quest,
        Misc
    }

    public ItemsMk2(string name, int id, string desc, int power, int wgt, ItemType type, int mst)
    {
        itemName = name;
        itemID = id;
        itemDesc = desc;
        itemIcon = Resources.Load<Texture2D>("ItemIcons/" + id);
        itemPower = power;
        itemWeight = wgt;
        itemType = type;
        itemMaxStack = mst;
    }
    public ItemsMk2()
    {

    }
}
