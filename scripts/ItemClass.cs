using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass
{
    public int id;
    public string name;
    public Texture2D icon;
    public string description;
    public float weight;
    //for sorting tabs
    public bool equipment;
    public bool consumable;
    public bool resource;
    public bool misc;

    //functional bools
    public bool weapon;
    public bool shield;

    public static Texture2D rockIcon;

    void Start()
    {
        rockIcon = Resources.Load("rock") as Texture2D;
    }

    public ItemClass(int ide, string nam, Texture2D ico, string des, float wei)
    {
        ide = id;
        nam = name;
        ico = icon;
        des = description;
        wei = weight;
    }

    public static ItemClass rockItem = new ItemClass(0, "Rock", rockIcon, "This is a rock", 1.2f);

}
