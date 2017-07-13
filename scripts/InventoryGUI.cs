using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGUI : MonoBehaviour
{
    Rect characterWindowRect;
    Rect tipRect;
    Rect destroyAmountRect;
    Rect showOverloadWarningRect;
    public bool characterWindowShow = false;
    public bool showTip = false;
    public bool showDestroyAmount = false;
    public bool weightOverload = false;
    public bool showTipEquipped = false;
    public bool showTradingWindow = false;
    //inventory item slots variables
    public float inventoryItemsXPos;
    public float inventoryItemsYPos;
    float inventoryItemsRowWidth;
    float inventoryItemsRowHeight;
    private float inventoryItemsBoxHeight;
    public float inventoryItemsBoxWidth;
    public int InventoryItemsRowCount = 7;
    public int InventoryItemsBoxInRow = 5;

    public int gp = 0;


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
    public List<int> inventoryStacksDisplayed = new List<int>();
    public List<ItemsMk2> equippedItems = new List<ItemsMk2>();
    public List<ItemsMk2> inventoryDisplayed = new List<ItemsMk2>();
    public List<int> displayedSlotsToActual = new List<int>();

    //CALLS TO OTHER SCRIPTS
    private ItemDatabaseMk2 database;
    private playerscript playerscript;

    //inventory item type tabs variables
    public bool showAll = true;
    public bool showEquipment = false;
    public bool showConsumables = false;
    public bool showResources = false;
    public bool showMisc = false;

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
            inventory.Add(new ItemsMk2(0));
            inventoryDisplayed.Add(new ItemsMk2(0));
            emptySlots.Add(new ItemsMk2(0));
            inventoryStacks.Add(1);
            inventoryStacksDisplayed.Add(1);
            displayedSlotsToActual.Add(i);
        }

        for (int i = 0; i < 10; i++)
        {
            equippedItems.Add(new ItemsMk2(0));
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
                    playerscript.currentWeight = playerscript.currentWeight + (database.items[id].itemWeight * (database.items[id].itemMaxStack - inventoryStacks[i]));
                    if (weightOverload)
                    { break; }
                    amount = amount - (database.items[id].itemMaxStack - inventoryStacks[i]);
                    inventoryStacks[i] = database.items[id].itemMaxStack;
                }

                if (amount < database.items[id].itemMaxStack)
                {
                    if ((database.items[id].itemMaxStack - inventoryStacks[i]) >= amount)
                    {
                        playerscript.currentWeight = playerscript.currentWeight + (database.items[id].itemWeight * amount);
                        if (weightOverload)
                        { break; }
                        inventoryStacks[i] = inventoryStacks[i] + amount;
                        break;
                    }
                    if ((database.items[id].itemMaxStack - inventoryStacks[i]) < amount)
                    {
                        playerscript.currentWeight = playerscript.currentWeight + (database.items[id].itemMaxStack - inventoryStacks[i]);
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
                            if ((inventory[r].itemID == 0) && (amount != 0))
                            {
                                playerscript.currentWeight = playerscript.currentWeight + (database.items[id].itemWeight * database.items[id].itemMaxStack);
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
                            if (inventory[r].itemID == 0)
                            {
                                playerscript.currentWeight = playerscript.currentWeight + (database.items[id].itemWeight * amount);
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
                        Debug.Log(database.items[r].itemID);
                        if (inventory[r].itemID == 0)
                        {
                            playerscript.currentWeight = playerscript.currentWeight + (database.items[id].itemWeight * amount);
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

    void RemoveItem(int slotId, int amount)
    {
        //destroy full stack
        if (inventoryStacks[slotId] == amount)
        {
            playerscript.currentWeight = playerscript.currentWeight - inventory[slotId].itemWeight;
            inventory[slotId] = emptySlots[1];
            inventoryStacks[slotId] = 1;
        }
        else
        {
            playerscript.currentWeight = playerscript.currentWeight - inventory[slotId].itemWeight * amount;
            inventoryStacks[slotId] = inventoryStacks[slotId] - amount;
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemID == 0)
            {
                for (int j = i; j < inventory.Count; j++)
                {
                    if (inventory[j].itemID != 0)
                    {
                        inventory[i] = inventory[j];
                        inventoryStacks[i] = inventoryStacks[j];
                        inventory[j] = emptySlots[1];
                        inventoryStacks[j] = 1;
                        break;
                    }
                }
            }
        }
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
            if (equippedItems[0].itemID == 0)
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
            if (equippedItems[1].itemID == 0)
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
            if (equippedItems[5].itemID == 0)
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
            if (equippedItems[4].itemID == 0)
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
            if (equippedItems[6].itemID == 0)
            {
                equippedItems[6] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }
            else if (equippedItems[7].itemID == 0)
            {
                equippedItems[7] = database.items[itemId];
                inventory[tipIdSlot] = emptySlots[tipIdSlot];
                showTip = false;
            }

        }
        if (database.items[itemId].itemIsMainHand)
        {
            if (equippedItems[8].itemID == 0)
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
            if (equippedItems[9].itemID == 0)
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
            if (equippedItems[2].itemID == 0)
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
            if (equippedItems[3].itemID == 0)
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
            if (inventory[i].itemID == 0)
            {
                equippedItems[tipIdSlot] = emptySlots[tipIdSlot];
                inventory[i] = database.items[tipIdItem];
                break;
            }
        }

        showTipEquipped = false;
    }
    void SellItem(int amount)
    {
        gp = gp + inventory[tipIdSlot].itemCost;
        RemoveItem(tipIdSlot, amount);
        inventory[tipIdSlot] = emptySlots[1];
    }
    private void OnGUI()
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
        //SPAWN ITEMS FOR TESTING END

        GUILayout.BeginArea(new Rect(0, 0, 100, 50));
        GUILayout.BeginHorizontal();
        GUILayout.Box(Resources.Load<Texture2D>("gp"), GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.Box("" + gp, GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUI.skin.box.wordWrap = true;
        if (characterWindowShow)
        {
            characterWindowRect = GUI.Window(0, characterWindowRect, characterWindowMethod, "Inventory");
        }

        if (showTradingWindow)
        {
            characterWindowRect = GUI.Window(2, characterWindowRect, TradingWindow, "Trading");
        }

        if (showTip)
        {
            tipRect = GUI.Window(2, tipRect, ShowTip, "");
        }

        if (showTipEquipped)
        {
            tipRect = GUI.Window(3, tipRect, ShowTipEquipped, "");
        }

        if (showDestroyAmount)
        {
            destroyAmountRect = GUI.Window(4, destroyAmountRect, ShowDestroyAmount, "");
        }

        if (weightOverload)
        {
            showOverloadWarningRect = GUI.Window(5, showOverloadWarningRect, ShowOverloadWarning, "");
        }

    }
    void UseItem(int id)
    { }
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
            RemoveItem(tipIdSlot, itemCount);
            itemCount = 1;
            showDestroyAmount = false;
        }

        if (GUILayout.Button("Destroy All", GUILayout.Width(200), GUILayout.Height(50)))
        {
            RemoveItem(tipIdSlot, inventoryStacks[tipIdSlot]);
            itemCount = 1;
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
                RemoveItem(tipIdSlot, 1);
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
                UseItem(tipIdItem);
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
            playerscript.currentWeight = playerscript.currentWeight - database.items[tipIdItem].itemWeight;
            equippedItems[tipIdSlot] = emptySlots[1];
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
        //horizontal top
        GUILayout.BeginArea(new Rect(horizontalTop, new Vector2(inventoryItemsBoxWidth * 2.25f, inventoryItemsBoxHeight)));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(equippedItems[0].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[0].itemID != 0)
            {
                tipIdItem = equippedItems[0].itemID;
                tipIdSlot = 0;
                showTipEquipped = true;
            }
        }
        if (GUILayout.Button(equippedItems[1].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[1].itemID != 0)
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
            if (equippedItems[2].itemID != 0)
            {
                tipIdItem = equippedItems[2].itemID;
                tipIdSlot = 2;
                showTipEquipped = true;
            }
        }
        if (GUILayout.Button(equippedItems[3].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[3].itemID != 0)
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
            if (equippedItems[4].itemID != 0)
            {
                tipIdItem = equippedItems[4].itemID;
                tipIdSlot = 4;
                showTipEquipped = true;
            }
        }

        if (GUILayout.Button(equippedItems[5].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[5].itemID != 0)
            {
                tipIdItem = equippedItems[5].itemID;
                tipIdSlot = 5;
                showTipEquipped = true;
            }
        }
        if (GUILayout.Button(equippedItems[6].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[6].itemID != 0)
            {
                tipIdItem = equippedItems[6].itemID;
                tipIdSlot = 6;
                showTipEquipped = true;
            }
        }
        if (GUILayout.Button(equippedItems[7].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[7].itemID != 0)
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
            if (equippedItems[8].itemID != 0)
            {
                tipIdItem = equippedItems[8].itemID;
                tipIdSlot = 8;
                showTipEquipped = true;
            }
        }
        if (GUILayout.Button(equippedItems[9].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
        {
            if (equippedItems[9].itemID != 0)
            {
                tipIdItem = equippedItems[9].itemID;
                tipIdSlot = 9;
                showTipEquipped = true;
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

        //Inventory item type tabs

        GUILayout.BeginArea(new Rect(inventoryItemsXPos, 20, inventoryItemsBoxWidth * InventoryItemsBoxInRow * 1.25f, inventoryItemsBoxHeight));

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("All", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f)))
        {
            showAll = true;
            showEquipment = false;
            showConsumables = false;
            showResources = false;
            showMisc = false;
        }
        if (GUILayout.Button("Equpment", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f)))
        {
            showAll = false;
            showEquipment = true;
            showConsumables = false;
            showResources = false;
            showMisc = false;
        }
        if (GUILayout.Button("Consumables", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f)))
        {
            showAll = false;
            showEquipment = false;
            showConsumables = true;
            showResources = false;
            showMisc = false;
        }
        if (GUILayout.Button("Resources", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f)))
        {
            showAll = false;
            showEquipment = false;
            showConsumables = false;
            showResources = true;
            showMisc = false;
        }
        if (GUILayout.Button("Misc", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f)))
        {
            showAll = false;
            showEquipment = false;
            showConsumables = false;
            showResources = false;
            showMisc = true;
        }

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

                    inventoryDisplayed[j] = emptySlots[1];
                    inventoryStacksDisplayed[j] = 1;
                    if (showAll)
                    {
                        int displayedSlot = 0;
                        for (int k = 0; k < inventory.Count; k++)
                        {
                            if (inventory[k].itemID != 0)
                            {
                                displayedSlotsToActual[displayedSlot] = k;
                                inventoryDisplayed[displayedSlot] = inventory[k];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[k];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showMisc)
                    {
                        int displayedSlot = 0;
                        for (int k = 0; k < inventory.Count; k++)
                        {
                            if (inventory[k].itemType == ItemsMk2.ItemType.Misc)
                            {
                                displayedSlotsToActual[displayedSlot] = k;
                                inventoryDisplayed[displayedSlot] = inventory[k];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[k];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showResources)
                    {
                        int displayedSlot = 0;
                        for (int k = 0; k < inventory.Count; k++)
                        {
                            if ((inventory[k].itemType == ItemsMk2.ItemType.Misc) && (inventory[k].itemIsResource))
                            {
                                displayedSlotsToActual[displayedSlot] = k;
                                inventoryDisplayed[displayedSlot] = inventory[k];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[k];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showConsumables)
                    {
                        int displayedSlot = 0;
                        for (int k = 0; k < inventory.Count; k++)
                        {
                            if (inventory[k].itemType == ItemsMk2.ItemType.Consumable)
                            {
                                displayedSlotsToActual[displayedSlot] = k;
                                inventoryDisplayed[displayedSlot] = inventory[k];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[k];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showEquipment)
                    {
                        int displayedSlot = 0;
                        for (int k = 0; k < inventory.Count; k++)
                        {
                            if ((inventory[k].itemType == ItemsMk2.ItemType.Armor) || (inventory[k].itemType == ItemsMk2.ItemType.Weapon) || (inventory[k].itemType == ItemsMk2.ItemType.Jewelry))
                            {
                                displayedSlotsToActual[displayedSlot] = k;
                                inventoryDisplayed[displayedSlot] = inventory[k];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[k];
                                displayedSlot++;
                            }
                        }
                    }



                    GUILayout.BeginArea(new Rect(inventoryItemsXPos + (inventoryItemsBoxWidth * t * 1.05f), inventoryItemsYPos + (inventoryItemsBoxWidth * i * 1.05f), inventoryItemsBoxWidth * 1.05f, inventoryItemsBoxHeight * 1.05f));
                    if (GUILayout.Button(inventoryDisplayed[j].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)) && inventoryDisplayed[j].itemID != 0)
                    {
                        showTip = true;
                        tipIdItem = inventoryDisplayed[j].itemID;
                        tipIdSlot = displayedSlotsToActual[j];
                    }
                    //DISPLAY ITEMCOUNT BEGIN
                    GUILayout.BeginArea(new Rect(inventoryItemsBoxWidth - 25, inventoryItemsBoxWidth - 20, 40, 20));
                    //if stackable
                    if (inventoryDisplayed[j].itemMaxStack > 1)
                    {
                        GUILayout.Box("" + inventoryStacksDisplayed[j], GUILayout.Height(20), GUILayout.Width(25));
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

    void TradingWindow(int windowId)
    {
        GUILayout.BeginArea(new Rect(inventoryItemsXPos, 20, inventoryItemsBoxWidth * InventoryItemsBoxInRow * 1.25f, inventoryItemsBoxHeight));

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("All", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f)))
        {
            showAll = true;
            showEquipment = false;
            showConsumables = false;
            showResources = false;
            showMisc = false;
        }
        if (GUILayout.Button("Equpment", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f)))
        {
            showAll = false;
            showEquipment = true;
            showConsumables = false;
            showResources = false;
            showMisc = false;
        }
        if (GUILayout.Button("Consumables", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f)))
        {
            showAll = false;
            showEquipment = false;
            showConsumables = true;
            showResources = false;
            showMisc = false;
        }
        if (GUILayout.Button("Resources", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f)))
        {
            showAll = false;
            showEquipment = false;
            showConsumables = false;
            showResources = true;
            showMisc = false;
        }
        if (GUILayout.Button("Misc", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth * 1.1f)))
        {
            showAll = false;
            showEquipment = false;
            showConsumables = false;
            showResources = false;
            showMisc = true;
        }

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
        int j = 0;
        for (int i = 0; i < InventoryItemsRowCount; i++)
        {
            GUILayout.BeginHorizontal();
            for (int t = 0; t < InventoryItemsBoxInRow; t++)
            {
                if (j < inventory.Count)
                {

                    inventoryDisplayed[j] = emptySlots[1];
                    inventoryStacksDisplayed[j] = 1;
                    if (showAll)
                    {
                        int displayedSlot = 0;
                        for (int k = 0; k < inventory.Count; k++)
                        {
                            if (inventory[k].itemID != 0)
                            {
                                displayedSlotsToActual[displayedSlot] = k;
                                inventoryDisplayed[displayedSlot] = inventory[k];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[k];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showMisc)
                    {
                        int displayedSlot = 0;
                        for (int k = 0; k < inventory.Count; k++)
                        {
                            if (inventory[k].itemType == ItemsMk2.ItemType.Misc)
                            {
                                displayedSlotsToActual[displayedSlot] = k;
                                inventoryDisplayed[displayedSlot] = inventory[k];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[k];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showResources)
                    {
                        int displayedSlot = 0;
                        for (int k = 0; k < inventory.Count; k++)
                        {
                            if ((inventory[k].itemType == ItemsMk2.ItemType.Misc) && (inventory[k].itemIsResource))
                            {
                                displayedSlotsToActual[displayedSlot] = k;
                                inventoryDisplayed[displayedSlot] = inventory[k];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[k];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showConsumables)
                    {
                        int displayedSlot = 0;
                        for (int k = 0; k < inventory.Count; k++)
                        {
                            if (inventory[k].itemType == ItemsMk2.ItemType.Consumable)
                            {
                                displayedSlotsToActual[displayedSlot] = k;
                                inventoryDisplayed[displayedSlot] = inventory[k];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[k];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showEquipment)
                    {
                        int displayedSlot = 0;
                        for (int k = 0; k < inventory.Count; k++)
                        {
                            if ((inventory[k].itemType == ItemsMk2.ItemType.Armor) || (inventory[k].itemType == ItemsMk2.ItemType.Weapon) || (inventory[k].itemType == ItemsMk2.ItemType.Jewelry))
                            {
                                displayedSlotsToActual[displayedSlot] = k;
                                inventoryDisplayed[displayedSlot] = inventory[k];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[k];
                                displayedSlot++;
                            }
                        }
                    }



                    GUILayout.BeginArea(new Rect(inventoryItemsXPos + (inventoryItemsBoxWidth * t * 1.05f), inventoryItemsYPos + (inventoryItemsBoxWidth * i * 1.05f), inventoryItemsBoxWidth * 1.05f, inventoryItemsBoxHeight * 1.05f));
                    if (GUILayout.Button(inventoryDisplayed[j].itemIcon, GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)) && inventoryDisplayed[j].itemID != 0)
                    {
                        showTip = true;
                        tipIdItem = inventoryDisplayed[j].itemID;
                        tipIdSlot = displayedSlotsToActual[j];
                    }
                    //DISPLAY ITEMCOUNT BEGIN
                    GUILayout.BeginArea(new Rect(inventoryItemsBoxWidth - 25, inventoryItemsBoxWidth - 20, 40, 20));
                    //if stackable
                    if (inventoryDisplayed[j].itemMaxStack > 1)
                    {
                        GUILayout.Box("" + inventoryStacksDisplayed[j], GUILayout.Height(20), GUILayout.Width(25));
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
    }
}