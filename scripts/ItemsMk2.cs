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
    public int itemSpeed;
    public ItemType itemType;

    public enum ItemType
    {
        Weapon,
        Consumable,
        Quest
    }

    public ItemsMk2(string name, int id, string desc, int power, int speed, ItemType type)
    {
        itemName = name;
        itemID = id;
        itemDesc = desc;
        itemIcon = Resources.Load<Texture2D>("ItemIcons/" + name);
        itemPower = power;
        itemSpeed = speed;
        itemType = type;
    }
    public ItemsMk2()
    {

    }
}
