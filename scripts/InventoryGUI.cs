using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGUI : MonoBehaviour
{
    Rect characterWindowRect;
    Rect tipRect;
    Rect destroyAmountRect;
    Rect showOverloadWarningRect;
    public bool characterWindowShow = true;
    public bool showTip = false;
    public bool showDestroyAmount = false;
    public bool weightOverload = false;
    public bool showTipEquipped = false;
    //inventory item slots variables
    public float inventoryItemsXPos;
    public float inventoryItemsYPos;
    float inventoryItemsRowWidth;
    float inventoryItemsRowHeight;
    private float inventoryItemsBoxHeight;
    public float inventoryItemsBoxWidth;
    public int InventoryItemsRowCount = 7;
    public int InventoryItemsBoxInRow = 5;



    public Vector2 scsize = new Vector2(Screen.width, Screen.height);

    private int itemCount = 1;
    private float itemCountFloat;
    public int tipIdItem;
    public int tipIdSlot;
    public float tipIdWeight;


    public Vector2 testVector1 = new Vector2(100, 100);
    public Vector2 testVector2 = new Vector2(100, 100);
    public int spawnId;
    public int spawnAmount;

    //Lists
    public List<ItemsMk2> inventory = new List<ItemsMk2>();
    public List<ItemsMk2> emptySlots = new List<ItemsMk2>();
    public List<int> inventoryStacks = new List<int>();
    public List<ItemsMk2> equippedItems = new List<ItemsMk2>();
    public List<ItemsMk2> inventoryActual = new List<ItemsMk2>();

    //CALLS TO OTHER SCRIPTS
    private ItemDatabaseMk2 database;
    private playerscript playerscript;

    //inventory item type tabs variables

    //equipped items variables
    public Vector2 horizontalTop = new Vector2(169, 32.5f);
    public Vector2 horizontalBottom = new Vector2(169, 581.1f);
    public Vector2 VerticalLeft = new Vector2(21.7f, 163.2f);
    public Vector2 VerticalRight = new Vector2(420.5f, 252.2f);

    //progress bars variables
    public Texture2D encFull;
    public Texture2D encEmpty;

    public Vector2 pos = new Vector2(114, 708);

    private void Start()
    {
        playerscript = GameObject.Find("Player").GetComponent<playerscript>();
        database = GameObject.Find("ItemDB").GetComponent<ItemDatabaseMk2>();

        for (int i = 0; i < (InventoryItemsRowCount * InventoryItemsBoxInRow); i++)
        {
            inventory.Add(new ItemsMk2());
            inventoryActual.Add(new ItemsMk2());
            emptySlots.Add(new ItemsMk2());
            inventoryStacks.Add(1);
        }

        for (int i = 0; i < 10; i++)
        {
            equippedItems.Add(new ItemsMk2());
        }

        //inventory item slots variables
        characterWindowRect = new Rect(0, 0, Screen.width, Screen.height);
        inventoryItemsXPos = Screen.width * 0.5f;
        inventoryItemsBoxWidth = Screen.width * 0.0857f;
        inventoryItemsYPos = inventoryItemsBoxWidth + inventoryItemsBoxWidth / 3;
        inventoryItemsBoxHeight = inventoryItemsBoxWidth;

        tipRect = new Rect((Screen.width / 2) - 250, (Screen.height / 2) - 250, 405, 405);
        destroyAmountRect = tipRect;
        showOverloadWarningRect = new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 200, 407, 407);
        //inventory item type tabs variables
    }

    //ADD ITEM METHOD
    void AddItem(int id, int amount)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (amount == 0)
            { break; }
            //if underfilled stacks are found, add to them first
            if ((inventory[i].itemName == database.items[id].itemName) && (inventoryStacks[i] < database.items[id].itemMaxStack))
            {
                if (amount == 0)
                { break; }
                if (amount > database.items[id].itemMaxStack)
                {
                    AddPlayerWeight(database.items[id].itemWeight * (database.items[id].itemMaxStack - inventoryStacks[i]));
                    if (weightOverload)
                    { break; }
                    amount = amount - (database.items[id].itemMaxStack - inventoryStacks[i]);
                    inventoryStacks[i] = database.items[id].itemMaxStack;
                }

                if (amount < database.items[id].itemMaxStack)
                {
                    if ((database.items[id].itemMaxStack - inventoryStacks[i]) >= amount)
                    {
                        AddPlayerWeight(database.items[id].itemWeight * amount);
                        if (weightOverload)
                        { break; }
                        inventoryStacks[i] = inventoryStacks[i] + amount;
                        break;
                    }
                    if ((database.items[id].itemMaxStack - inventoryStacks[i]) < amount)
                    {
                        AddPlayerWeight(database.items[id].itemMaxStack - inventoryStacks[i]);
                        if (weightOverload)
                        { break; }
                        amount = amount - (database.items[id].itemMaxStack - inventoryStacks[i]);
                        inventoryStacks[i] = database.items[id].itemMaxStack;
                    }

                }
            }

            //if no underfilled stack found, look for empty slots and add new stacks
            else if ((i == inventory.Count - 1) && (inventory[i].itemName != database.items[id].itemName) && amount != 0)
            {
                for (int r = 0; r < inventory.Count; r++)
                {
                    //if need to add more than or one stack
                    if (amount > database.items[id].itemMaxStack)
                    {
                        // add full X full stacks
                        for (int x = 0; x < amount / database.items[id].itemMaxStack; x++)
                        {
                            if ((inventory[r].itemName == null) && (amount != 0))
                            {
                                AddPlayerWeight(database.items[id].itemWeight * database.items[id].itemMaxStack);
                                if (weightOverload)
                                { break; }
                                inventory[r] = database.items[id];
                                inventoryStacks[r] = database.items[id].itemMaxStack;
                                amount = amount - database.items[id].itemMaxStack;
                                if (amount == 0)
                                { break; }
                            }
                        }

                        if ((amount < database.items[id].itemMaxStack) && (amount > 0))
                        {
                            // add an underfilled stack
                            if (inventory[r].itemName == null)
                            {
                                AddPlayerWeight(database.items[id].itemWeight * amount);
                                inventory[r] = database.items[id];
                                if (weightOverload)
                                { break; }
                                inventoryStacks[r] = amount;
                                break;
                            }
                        }
                    }
                    //if need to add an underfilled stack only
                    else
                    {
                        if (inventory[r].itemName == null)
                        {
                            AddPlayerWeight(database.items[id].itemWeight * amount);
                            if (weightOverload)
                            { break; }
                            inventory[r] = database.items[id];
                            inventoryStacks[r] = amount;
                            break;
                        }
                    }
                }
            }
        }
    }


    void AddPlayerWeight(float inputWeight)
    {
        if (inputWeight > (playerscript.maxWeight - playerscript.currentWeight))
        {
            weightOverload = true;
        }

        if (inputWeight <= (playerscript.maxWeight - playerscript.currentWeight) && inputWeight > 0)
        {
            playerscript.currentWeight = playerscript.currentWeight + inputWeight;
        }
    }

    void RemovePlayerWeight(float inputWeight)
    {
        Debug.Log("weight=" + inputWeight);
        playerscript.currentWeight = playerscript.currentWeight - inputWeight;
    }

    void ShowOverloadWarning(int windowId)
    {
        characterWindowShow = false;
        GUILayout.BeginArea(new Rect(0, 0, 30, 30));
        if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(30)))
        {
            weightOverload = false;
            characterWindowShow = true;
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0, 0, 407, 407));
        GUILayout.BeginVertical();
        GUILayout.Box("Too heavy! Can't carry that much!", GUILayout.Width(400), GUILayout.Height(350));
        if (GUILayout.Button("OK", GUILayout.Width(400), GUILayout.Height(50)))
        {
            weightOverload = false;
            characterWindowShow = true;
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void EquipItem(int itemId)
    {
        if (database.items[itemId].itemIsHead)
        {
            if (equippedItems[0].itemName == null)
            {
                equippedItems[0] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }
            else
            {
                inventory[tipIdSlot] = equippedItems[0];
                equippedItems[0] = database.items[itemId];
                showTip = false;
            }
        }
        if (database.items[itemId].itemIsNeck)
        {
            if (equippedItems[1].itemName == null)
            {
                equippedItems[1] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }
            else
            {
                inventory[tipIdSlot] = equippedItems[1];
                equippedItems[0] = database.items[itemId];
                showTip = false;
            }
        }
        if (database.items[itemId].itemIsHands)
        {
            if (equippedItems[5].itemName == null)
            {
                equippedItems[5] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }
            else
            {
                inventory[tipIdSlot] = equippedItems[5];
                equippedItems[5] = database.items[itemId];
                showTip = false;
            }
        }
        if (database.items[itemId].itemIsTorso)
        {
            if (equippedItems[4].itemName == null)
            {
                equippedItems[4] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }
            else
            {
                inventory[tipIdSlot] = equippedItems[4];
                equippedItems[4] = database.items[itemId];
                showTip = false;
            }
        }
        if (database.items[itemId].itemIsFinger)
        {
            if (equippedItems[6].itemName == null)
            {
                equippedItems[6] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }
            else if (equippedItems[7].itemName == null)
            {
                equippedItems[7] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }

        }
        if (database.items[itemId].itemIsMainHand)
        {
            if (equippedItems[8].itemName == null)
            {
                equippedItems[8] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }
            else
            {
                inventory[tipIdSlot] = equippedItems[8];
                equippedItems[8] = database.items[itemId];
                showTip = false;
            }
        }
        if (database.items[itemId].itemIsOffHand)
        {
            if (equippedItems[9].itemName == null)
            {
                equippedItems[9] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }
            else
            {
                inventory[tipIdSlot] = equippedItems[9];
                equippedItems[9] = database.items[itemId];
                showTip = false;
            }
        }
        if (database.items[itemId].itemIsLegs)
        {
            if (equippedItems[2].itemName == null)
            {
                equippedItems[2] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }
            else
            {
                inventory[tipIdSlot] = equippedItems[2];
                equippedItems[2] = database.items[itemId];
                showTip = false;
            }
        }
        if (database.items[itemId].itemIsFeet)
        {
            if (equippedItems[3].itemName == null)
            {
                equippedItems[3] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }
            else
            {
                inventory[tipIdSlot] = equippedItems[3];
                equippedItems[3] = database.items[itemId];
                showTip = false;
            }
        }
    }
    void UnequipItem()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == null)
            {
                equippedItems[tipIdSlot] = emptySlots[tipIdSlot];
                inventory[i] = database.items[tipIdItem];
                break;
            }
        }

        showTipEquipped = false;
    }

    private void OnGUI()
    {
        GUI.skin.box.wordWrap = true;
        if (characterWindowShow)
        {
            characterWindowRect = GUI.Window(0, characterWindowRect, characterWindowMethod, "Inventory");
        }

        if (showTip)
        {
            tipRect = GUI.Window(1, tipRect, ShowTip, "");
        }

        if (showTipEquipped)
        {
            tipRect = GUI.Window(1, tipRect, ShowTipEquipped, "");
        }

        if (showDestroyAmount)
        {
            destroyAmountRect = GUI.Window(1, destroyAmountRect, ShowDestroyAmount, "");
        }

        if (weightOverload)
        {
            showOverloadWarningRect = GUI.Window(2, showOverloadWarningRect, ShowOverloadWarning, "");
        }

    }

    //DESTROY AMOUNT
    void ShowDestroyAmount(int windowId)
    {
        GUILayout.BeginArea(new Rect(0, 0, 30, 30));
        if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(30)))
        {
            showTip = true;
            showDestroyAmount = false;
        }
        GUILayout.EndArea();

        GUILayout.BeginVertical();

        GUILayout.BeginArea(new Rect(0, 0, 400, 350));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("-", GUILayout.Width(50), GUILayout.Height(50)) && itemCount > 0)
        {
            itemCount--;
        }

        GUILayout.Box("" + itemCount, GUILayout.Width(50), GUILayout.Height(50));

        if (GUILayout.Button("+", GUILayout.Width(50), GUILayout.Height(50)) && itemCount < inventoryStacks[tipIdSlot])
        {
            itemCount++;
        }
        GUILayout.EndArea();
        GUILayout.EndHorizontal();


        GUILayout.BeginArea(new Rect(0, 350, 400, 50));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Destroy", GUILayout.Width(200), GUILayout.Height(50)))
        {
            if (itemCount == inventoryStacks[tipIdSlot]) // if input == full stack, destroy all
            {
                RemovePlayerWeight(database.items[tipIdItem].itemWeight * itemCount);
                itemCount = 1;
                inventoryStacks[tipIdSlot] = 1;
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showDestroyAmount = false;
            }
            else //if input != full stack, destroy input count
            {
                inventoryStacks[tipIdSlot] = inventoryStacks[tipIdSlot] - itemCount;
                showDestroyAmount = false;
                RemovePlayerWeight(database.items[tipIdItem].itemWeight * itemCount);
                itemCount = 1;
            }
        }

        if (GUILayout.Button("Destroy All", GUILayout.Width(200), GUILayout.Height(50)))
        {
            RemovePlayerWeight(database.items[tipIdItem].itemWeight * inventoryStacks[tipIdSlot]);
            itemCount = 1;
            inventoryStacks[tipIdSlot] = 1;
            inventory[tipIdSlot] = emptySlots[tipIdSlot];
            showDestroyAmount = false;
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    //ITEM TIP INVENTORY
    void ShowTip(int windowId)
    {
        GUILayout.BeginArea(new Rect(0, 0, 30, 30));
        if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(30)))
        {
            showTip = false;
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0, 0, 407, 407));
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Box(database.items[tipIdItem].itemIcon, GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.Box(database.items[tipIdItem].itemName + '\n' + database.items[tipIdItem].itemName + " stats go here" + '\n' + "can sell for " + database.items[tipIdItem].itemCost + " gp", GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.EndHorizontal();
        GUILayout.Box(database.items[tipIdItem].itemDesc, GUILayout.Width(407), GUILayout.Height(150));


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Destroy", GUILayout.Width(200), GUILayout.Height(50)))
        {
            if (database.items[tipIdSlot].itemType == ItemsMk2.ItemType.Weapon)
            {
                Debug.Log("GOOOOD");
            }

            //ask how much to destroy if stacked
            if (inventoryStacks[tipIdSlot] > 1)
            {
                showDestroyAmount = true;
                showTip = false;
            }
            //destroy immidiately if not stackable or stack == 1
            if (inventoryStacks[tipIdSlot] == 1)
            {
                showTip = false;
                Debug.Log("weight=" + database.items[tipIdItem].itemWeight);
                RemovePlayerWeight(database.items[tipIdItem].itemWeight);
                inventoryStacks[tipIdSlot] = 0;
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
            }
        }

        if ((database.items[tipIdItem].itemType == ItemsMk2.ItemType.Weapon) || (database.items[tipIdItem].itemType == ItemsMk2.ItemType.Armor) || (database.items[tipIdItem].itemType == ItemsMk2.ItemType.Jewelry))
        {
            if (GUILayout.Button("Equip", GUILayout.Width(200), GUILayout.Height(50)))
            {
                EquipItem(tipIdItem);
            }
        }

        else
        {
            if (GUILayout.Button("Use", GUILayout.Width(200), GUILayout.Height(50)))
            {

            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    //ITEM TIP EQUIPPED
    void ShowTipEquipped(int windowId)
    {
        GUILayout.BeginArea(new Rect(0, 0, 30, 30));
        if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(30)))
        {
            showTipEquipped = false;
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0, 0, 407, 407));
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Box(database.items[tipIdItem].itemIcon, GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.Box(database.items[tipIdItem].itemName + '\n' + database.items[tipIdItem].itemName + " stats go here" + '\n' + "can sell for " + database.items[tipIdItem].itemCost + " gp", GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.EndHorizontal();
        GUILayout.Box(database.items[tipIdItem].itemDesc, GUILayout.Width(407), GUILayout.Height(150));


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Destroy", GUILayout.Width(200), GUILayout.Height(50)))
        {
            showTipEquipped = false;
            RemovePlayerWeight(database.items[tipIdItem].itemWeight);
            equippedItems[tipIdSlot] = emptySlots[tipIdSlot];
        }
        if (GUILayout.Button("Unequip", GUILayout.Width(200), GUILayout.Height(50)))
        {
            UnequipItem();
        }
        GUILayout.EndHorizontal();


        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    //DISPLAY INVENTORY
    void characterWindowMethod(int windowId)
    {
        //SPAWN ITEMS FOR TESTING
        GUILayout.BeginArea(new Rect(300, 100, 200, 200));
        GUILayout.BeginVertical();
        int.TryParse(GUILayout.TextField(spawnId.ToString()), out spawnId);
        int.TryParse(GUILayout.TextField(spawnAmount.ToString()), out spawnAmount);
        if (GUILayout.Button("Add Item", GUILayout.Width(90), GUILayout.Height(25)))
        {
            if (spawnId < database.items.Count)
            {
                AddItem(spawnId, spawnAmount);
            }
            else { }

        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

        //horizontal top
        GUILayout.BeginArea(new Rect(horizontalTop, new Vector2(inventoryItemsBoxWidth * 2.25f, inventoryItemsBoxHeight)));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(equippedItems[0].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[0].itemName != null)
            {
                tipIdItem = equippedItems[0].itemID;
                tipIdSlot = 0;
                showTipEquipped = true;
            }
        }
        if (GUILayout.Button(equippedItems[1].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[1].itemName != null)
            {
                tipIdItem = equippedItems[1].itemID;
                tipIdSlot = 1;
                showTipEquipped = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        //horizontal bottom
        GUILayout.BeginArea(new Rect(horizontalBottom, new Vector2(inventoryItemsBoxWidth * 2.25f, inventoryItemsBoxHeight)));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(equippedItems[2].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[2].itemName != null)
            {
                tipIdItem = equippedItems[2].itemID;
                tipIdSlot = 2;
                showTipEquipped = true;
            }
        }
        if (GUILayout.Button(equippedItems[3].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[3].itemName != null)
            {
                tipIdItem = equippedItems[3].itemID;
                tipIdSlot = 3;
                showTipEquipped = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        //vertical left
        GUILayout.BeginArea(new Rect(VerticalLeft, new Vector2(inventoryItemsBoxWidth, inventoryItemsBoxHeight * 4.25f)));
        GUILayout.BeginVertical();
        if (GUILayout.Button(equippedItems[4].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[4].itemName != null)
            {
                tipIdItem = equippedItems[4].itemID;
                tipIdSlot = 4;
                showTipEquipped = true;
            }
        }

        if (GUILayout.Button(equippedItems[5].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[5].itemName != null)
            {
                tipIdItem = equippedItems[5].itemID;
                tipIdSlot = 5;
                showTipEquipped = true;
            }
        }
        if (GUILayout.Button(equippedItems[6].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[6].itemName != null)
            {
                tipIdItem = equippedItems[6].itemID;
                tipIdSlot = 6;
                showTipEquipped = true;
            }
        }
        if (GUILayout.Button(equippedItems[7].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[7].itemName != null)
            {
                tipIdItem = equippedItems[7].itemID;
                tipIdSlot = 7;
                showTipEquipped = true;
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

        //vertical right
        GUILayout.BeginArea(new Rect(VerticalRight, new Vector2(inventoryItemsBoxWidth, inventoryItemsBoxHeight * 2.25f)));
        GUILayout.BeginVertical();
        if (GUILayout.Button(equippedItems[8].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[8].itemName != null)
            {
                tipIdItem = equippedItems[8].itemID;
                tipIdSlot = 8;
                showTipEquipped = true;
            }
        }
        if (GUILayout.Button(equippedItems[9].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[9].itemName != null)
            {
                tipIdItem = equippedItems[9].itemID;
                tipIdSlot = 9;
                showTipEquipped = true;
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

        //Inventory item types tab

        GUILayout.BeginArea(new Rect(inventoryItemsXPos, 20, inventoryItemsBoxWidth * InventoryItemsBoxInRow * 1.25f, inventoryItemsBoxHeight));

        GUILayout.BeginHorizontal();

        GUILayout.Button("All", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f));
        GUILayout.Button("Equpment", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f));
        GUILayout.Button("Consumables", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f));
        GUILayout.Button("Resources", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f));
        GUILayout.Button("Misc", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f));

        GUILayout.EndHorizontal();

        GUILayout.EndArea();


        // Inventory item slots
        int j = 0;
        for (int i = 0; i < InventoryItemsRowCount; i++)
        {
            GUILayout.BeginHorizontal();
            for (int t = 0; t < InventoryItemsBoxInRow; t++)
            {
                if (j < inventory.Count)
                {
                    GUILayout.BeginArea(new Rect(inventoryItemsXPos + (inventoryItemsBoxWidth * t * 1.05f), inventoryItemsYPos + (inventoryItemsBoxWidth * i * 1.05f), inventoryItemsBoxWidth * 1.05f, inventoryItemsBoxHeight * 1.05f));
                    if (GUILayout.Button(inventory[j].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)) && inventory[j].itemName != null)
                    {
                        showTip = true;
                        tipIdItem = inventory[j].itemID;
                        tipIdSlot = j;
                    }
                    //DISPLAY ITEMCOUNT BEGIN
                    GUILayout.BeginArea(new Rect(inventoryItemsBoxWidth - 25, inventoryItemsBoxWidth - 20, 40, 20));
                    //if stackable
                    if (inventory[j].itemMaxStack > 1)
                    {
                        GUILayout.Box("" + inventoryStacks[j], GUILayout.Height(20), GUILayout.Width(25));
                        GUILayout.Space(inventoryItemsBoxWidth - 20);
                    }
                    GUILayout.EndArea();
                    //DISPLAY ITEMCOUNT END
                    GUILayout.EndArea();
                    j++;
                }
            }
            GUILayout.EndHorizontal();
        }

        //encumbrance bar
        GUI.BeginGroup(new Rect(Screen.width * 0.6f, Screen.height - 45, 350, 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), encEmpty);

        GUI.BeginGroup(new Rect(0, 0, 350 * (playerscript.currentWeight / playerscript.maxWeight), 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), encFull);
        GUI.EndGroup();

        GUI.EndGroup();

        //bar area

        //  GUILayout.BeginArea(new Rect(pos, new Vector2(360, 100)));

        ////exp bar

        //GUI.BeginGroup(new Rect(0, 0, 350, 25));
        //GUI.DrawTexture(new Rect(0, 0, 350, 25), encEmpty);

        //GUI.BeginGroup(new Rect(0, 0, 350 * (currentWeight / maxWeight), 25));
        //GUI.DrawTexture(new Rect(0, 0, 350, 25), encFull);
        //GUI.EndGroup();

        //GUI.EndGroup();

        ////hp bar

        //GUI.BeginGroup(new Rect(0, 35, 350, 25));
        //GUI.DrawTexture(new Rect(0, 0, 350, 25), encEmpty);

        //GUI.BeginGroup(new Rect(0, 0, 350 * (currentWeight / maxWeight), 25));
        //GUI.DrawTexture(new Rect(0, 0, 350, 25), encFull);
        //GUI.EndGroup();

        //GUI.EndGroup();

        ////mp/sp bar

        //GUI.BeginGroup(new Rect(0, 70, 350, 25));
        //GUI.DrawTexture(new Rect(0, 0, 350, 25), encEmpty);

        //GUI.BeginGroup(new Rect(0, 0, 350 * (currentWeight / maxWeight), 25));
        //GUI.DrawTexture(new Rect(0, 0, 350, 25), encFull);
        //GUI.EndGroup();

        //GUI.EndGroup();

        //GUILayout.EndArea();

    }

}