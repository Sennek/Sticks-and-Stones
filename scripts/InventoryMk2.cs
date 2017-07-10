using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMk2 : MonoBehaviour {
    public int slotsX, slotsY;
    public GUISkin skin;
    public List<ItemsMk2> inventory = new List<ItemsMk2>();
    public List<ItemsMk2> slots = new List<ItemsMk2>();
    public bool showInventory;

    private int spawnId;

    private ItemDatabaseMk2 database;

    private void Awake()
    {

    }

    private void Start()
    {
        database = GameObject.Find("Item Database").GetComponent<ItemDatabaseMk2>();
        for (int i = 0; i < (slotsX * slotsY); i++)
        {
            slots.Add(new ItemsMk2());
            inventory.Add(new ItemsMk2());
            inventory.Add(new ItemsMk2());
        }
        AddItem(1);
        showInventory = true;
    }

    void RemoveItem(int id)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].itemID == id)
            {
                inventory[i] = new ItemsMk2();
                break;
            }
        }
    }


    private void OnGUI()
    {
        GUI.skin = skin;
        if (showInventory)
        {
            DrawInventory();
        }

        GUILayout.BeginArea(new Rect(300, 100, 200, 200));
        GUILayout.BeginVertical();
        int.TryParse(GUILayout.TextField(spawnId.ToString()), out spawnId);
        if (GUILayout.Button("Add Item", GUILayout.Width(90), GUILayout.Height(25)))
        {
            AddItem(spawnId);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

        for (int i = 0; i < inventory.Count; i++)
        {
            GUI.Label(new Rect(10, i * 20, 200, 50), string.Empty);
        }
    }


    void DrawInventory()
    {
        int i = 0;
        for (int y = 0; y < slotsY; y++)
        {
            for (int x = 0; x < slotsX; x++)
            {
                Rect slotRect = new Rect(x * 60, y * 60, 50, 50);
                GUI.Box(slotRect, "", skin.GetStyle("Slot"));


                if (i<inventory.Count) // make sure i don't exceed inventory[] array count
                { 
                slots[i] = inventory[i];
                if (slots[i].itemName != null)
                {
                    GUI.DrawTexture(slotRect, slots[i].itemIcon);
                }

                i++;
                }
            }
        }
    }

     
    void AddItem(int id)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].itemName == null)
            {
                for(int j = 0; j < database.items.Count; j++)
                {
                    if (database.items[j].itemID == id)
                    {
                        inventory[i] = database.items[j];
                    }
                }
                break;
            }
        }
    }

    bool InventoryContains (int id)
    {
        bool result = false;
        for (int i = 0; i <inventory.Count; i++)
        {
            result = inventory[i].itemID == id;
            if(result)
            {
                break;
            }
        }
        return result;

    }
}
