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
    public int itemAddStr;
    public int itemAddAgi;
    public int itemAddVit;
    public int itemAddInt;
    public int itemAddCha;
    public int itemAddMaxHealth;
    public int itemAddMaxStamina;
    public int itemAddMaxMana;

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
        Consumable,
    }

    //Weapons
    public ItemsMk2(int id, ItemType type, string name, string desc, float weight, int cost, int maxStack, int atkMin, int atkMax, float critChance, int speed, int str, int agi, int intl, int cha, int vit, int mHealth, int mStamina, int mMana, bool mainHand, bool offHand, bool sharp, bool blunt)
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
        itemAddStr = str;
        itemAddAgi = agi;
        itemAddCha = cha;
        itemAddVit = vit;
        itemAddInt = intl;
        itemAddMaxHealth = mHealth;
        itemAddMaxStamina = mStamina;
        itemAddMaxMana = mMana;
        itemIsMainHand = mainHand;
        itemIsOffHand = offHand;
        itemIsSharp = sharp;
        itemIsBlunt = blunt;
    }

    //Armor
    public ItemsMk2(int id, ItemType type, string name, string desc, float weight, int cost, int maxStack, int defMin, int defMax, int speed, int str, int agi, int intl, int cha, int vit, int mHealth, int mStamina, int mMana, bool isHead, bool isTorso, bool isHands, bool isLegs, bool isFeet)
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
        itemHandlingSpeed = speed;
        itemAddStr = str;
        itemAddAgi = agi;
        itemAddCha = cha;
        itemAddVit = vit;
        itemAddInt = intl;
        itemAddMaxHealth = mHealth;
        itemAddMaxStamina = mStamina;
        itemAddMaxMana = mMana;
        itemIsHead = isHead;
        itemIsTorso = isTorso;
        itemIsHands = isHands;
        itemIsLegs = isLegs;
        itemIsFeet = isFeet;
        itemHandlingSpeed = speed;
    }

    //Jewelry
    public ItemsMk2(int id, ItemType type, string name, string desc, float weight, int cost, int maxStack, int mMana, int mHealth, int mStamina, int str, int agi, int intl, int cha, int vit,int atkMin, int atkMax, int defMin, int defMax, float critChance, int Speed, bool isNeck, bool isFinger)
    {
        itemID = id;
        itemType = type;
        itemIcon = Resources.Load<Texture2D>("ItemIcons/" + id);
        itemName = name;
        itemDesc = desc;
        itemWeight = weight;
        itemCost = cost;
        itemMaxStack = maxStack;
        itemAddMaxMana = mMana;
        itemAddMaxHealth = mHealth;
        itemAddMaxStamina = mStamina;
        itemAddStr = str;
        itemAddAgi = agi;
        itemAddCha = cha;
        itemAddVit = vit;
        itemAddInt = intl;
        itemMinAttack = atkMin;
        itemMaxAttack = atkMax;
        itemMinDef = defMin;
        itemMaxDef = defMax;
        itemCritChance = critChance;
        itemHandlingSpeed = Speed;
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
    public ItemsMk2(int id, ItemType type, string name, string desc, float weight, int cost, int maxStack, int health, int mana, int stamina, int str, int agi, int intl, int cha, int vit, int mHealth, int mStamina, int mMana)
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
        itemAddStr = str;
        itemAddAgi = agi;
        itemAddCha = cha;
        itemAddVit = vit;
        itemAddMaxMana = mMana;
        itemAddMaxHealth = mHealth;
        itemAddMaxStamina = mStamina;
        itemAddInt = intl;
    }
    //empty
    public ItemsMk2(int id)
    {
        itemID = id;
    }
}
