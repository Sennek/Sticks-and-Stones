using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemsMk2
{
    //common vars
    public string itemName;
    public int itemID;
    public string itemDesc;
    public Texture2D itemIcon;
    public int itemPower;
    public float itemWeight;
    public ItemType itemType;
    public int itemHandlingSpeed;
    public int itemCost;

    //Weapon vars
    public int itemMaxStack;
    public int itemMaxAttack;
    public int itemMinAttack;
    public float itemCritChance;

    public bool itemIsMainHand;
    public bool itemIsOffHand;
    public bool itemIsSharp;
    public bool itemIsBlunt;

    //Armor vars
    public int itemMinDef;
    public int itemMaxDef;

    public bool itemIsHead;
    public bool itemIsTorso;
    public bool itemIsHands;
    public bool itemIsFeet;
    public bool itemIsLegs;

    //Jewelry vars
    public int itemManaBonus;
    public int itemHealthBonus;
    public int itemStaminaBonus;

    public bool itemIsNeck;
    public bool itemIsFinger;

    //Consumable vars
    public int itemRestoreMana;
    public int itemRestoreHealth;
    public int itemRestoreStamina;

    //Misc vars
    public bool itemIsQuest;
    public bool itemIsResource;
    public bool itemIsFood;


    public enum ItemType
    {
        Weapon,
        Armor,
        Jewelry,
        Misc,
        Consumable
    }

    //Weapons
    public ItemsMk2(int id, ItemType type, string name, string desc, float weight, int cost, int maxStack, int atkMin, int atkMax, float critChance, int speed, bool mainHand, bool offHand, bool sharp, bool blunt)
    {
        itemID = id;
        itemType = type;
        itemIcon = Resources.Load<Texture2D>("ItemIcons/" + id);
        itemName = name;
        itemDesc = desc;
        itemWeight = weight;
        itemCost = cost;
        itemMaxStack = maxStack;
        itemMinAttack = atkMin;
        itemMaxAttack = atkMax;
        itemCritChance = critChance;
        itemHandlingSpeed = speed;
        itemIsMainHand = mainHand;
        itemIsOffHand = offHand;
        itemIsSharp = sharp;
        itemIsBlunt = blunt;
    }

    //Armor
    public ItemsMk2(int id, ItemType type, string name, string desc, float weight, int cost, int maxStack, int defMin, int defMax, int speed, bool isHead, bool isTorso, bool isHands, bool isLegs, bool isFeet)
    {
        itemID = id;
        itemType = type;
        itemIcon = Resources.Load<Texture2D>("ItemIcons/" + id);
        itemName = name;
        itemDesc = desc;
        itemWeight = weight;
        itemCost = cost;
        itemMaxStack = maxStack;
        itemMinDef = defMin;
        itemMaxDef = defMax;
        itemIsHead = isHead;
        itemIsTorso = isTorso;
        itemIsHands = isHands;
        itemIsLegs = isLegs;
        itemIsFeet = isFeet;
        itemHandlingSpeed = speed;
    }

    //Jewelry
    public ItemsMk2(int id, ItemType type, string name, string desc, float weight, int cost, int maxStack, int mana, int health, int stamina, bool isNeck, bool isFinger)
    {
        itemID = id;
        itemType = type;
        itemIcon = Resources.Load<Texture2D>("ItemIcons/" + id);
        itemName = name;
        itemDesc = desc;
        itemWeight = weight;
        itemCost = cost;
        itemMaxStack = maxStack;
        itemManaBonus = mana;
        itemHealthBonus = health;
        itemStaminaBonus = stamina;
        itemIsNeck = isNeck;
        itemIsFinger = isFinger;
    }

    //Misc
    public ItemsMk2(int id, ItemType type, string name, string desc, float weight, int cost, int maxStack, bool isQuest, bool isResource, bool isFood)
    {
        itemID = id;
        itemType = type;
        itemIcon = Resources.Load<Texture2D>("ItemIcons/" + id);
        itemName = name;
        itemDesc = desc;
        itemWeight = weight;
        itemCost = cost;
        itemMaxStack = maxStack;
        itemIsQuest = isQuest;
        itemIsResource = isResource;
        itemIsFood = isFood;
    }

    //Consumable
    public ItemsMk2(int id, ItemType type, string name, string desc, float weight, int cost, int maxStack, int health, int mana, int stamina)
    {
        itemID = id;
        itemType = type;
        itemIcon = Resources.Load<Texture2D>("ItemIcons/" + id);
        itemName = name;
        itemDesc = desc;
        itemWeight = weight;
        itemCost = cost;
        itemMaxStack = maxStack;
        itemRestoreMana = mana;
        itemRestoreHealth = health;
        itemRestoreStamina = stamina;
    }
    //empty
    public ItemsMk2()
    {

    }
}
