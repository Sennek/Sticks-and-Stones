using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XmlLoader;

public class InventoryGUI : MonoBehaviour
{
    public SaveManager saveManager;
    Rect characterWindowRect;
    Rect tipRect;
    Rect destroyAmountRect;
    Rect showOverloadWarningRect;
    //TEST VARS BEGIN
    public Vector2 test1 = new Vector2(0, 0);
    public Vector2 test2 = new Vector2(0, 0);
    //TEST VARS END

    public bool characterWindowShow = false;
    public bool showTip = false;
    public bool showAmount = false;
    public bool weightOverload = false;
    public bool showTipEquipped = false;
    public bool showTradingWindow = false;
    public bool updateMerchantInventory = false;
    public bool showCharacterWindow = false;

    private bool sell = false;
    private bool buy = false;

    public bool merchantNotEnoughMoney;
    public bool meNotEnoughMoney;

    public bool showMenu = false;

    //inventory item slots variables
    public float inventoryItemsXPos;
    public float inventoryItemsYPos;
    private float inventoryItemsRowWidth;
    private float inventoryItemsRowHeight;
    private float inventoryItemsBoxHeight;
    private float inventoryItemsBoxWidth;

    public int playerGold = 300;
    public int merchantGold;
    public Blacksmith blacksmith;
    public Levelmanager levelmanager;

    public Vector2 scsize = new Vector2(Screen.width, Screen.height);

    private int itemCount = 1;
    private float itemCountFloat;
    public int tipIdItem;
    public int tipIdSlot;

    public int spawnId;
    public int spawnAmount;

    //Lists
    public List<ItemsMk2> inventory = new List<ItemsMk2>();
    public List<int> inventoryStacks = new List<int>();
    public List<int> inventoryStacksDisplayed = new List<int>();
    public List<ItemsMk2> equippedItems = new List<ItemsMk2>();
    public List<ItemsMk2> inventoryDisplayed = new List<ItemsMk2>();
    public List<int> displayedSlotsToActual = new List<int>();

    public List<ItemsMk2> tradeInventory = new List<ItemsMk2>();
    public List<int> tradeInventoryStacks = new List<int>();

    //CALLS TO OTHER SCRIPTS
    private ItemDatabaseMk2 database;
    private playerscript playerScript;

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

    public Vector2 pos = new Vector2(114, 708);

    private void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<playerscript>();
        database = GameObject.Find("ItemDB").GetComponent<ItemDatabaseMk2>();

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
        ModifyInventoryRows(4);

        for (int i = 0; i < 10; i++)
        { equippedItems.Add(database.items[0]); }
    }

    //modify inventory rows
    public void ModifyInventoryRows(int rowAmount)
    {
        if (rowAmount > 0)
        {
            for (int i = 0; i < rowAmount * 5; i++)
            {
                inventory.Add(database.items[0]);
                inventoryStacks.Add(1);
                inventoryDisplayed.Add(database.items[0]);
                inventoryStacksDisplayed.Add(1);
                displayedSlotsToActual.Add(1);
            }
        }

        if (rowAmount < 0)
        {
            for (int i = 0; i < rowAmount * (-5); i++)
            {
                inventory.RemoveAt(inventory.Count - 1);
                inventoryStacks.RemoveAt(inventoryStacks.Count - 1);
                inventoryDisplayed.RemoveAt(inventoryDisplayed.Count - 1);
                inventoryStacksDisplayed.RemoveAt(inventoryStacksDisplayed.Count - 1);
                displayedSlotsToActual.RemoveAt(displayedSlotsToActual.Count - 1);
            }
        }

    }
    public void ModifyTradeInventoryRows(int rowAmount)
    {
        if (rowAmount > 0)
        {
            for (int i = 0; i < rowAmount * 5; i++)
            {
                tradeInventory.Add(database.items[0]);
                tradeInventoryStacks.Add(1);
            }
        }

        if (rowAmount < 0)
        {
            for (int i = 0; i < rowAmount * (-5); i++)
            {
                tradeInventory.RemoveAt(tradeInventory.Count - 1);
                tradeInventoryStacks.RemoveAt(tradeInventoryStacks.Count - 1);
            }
        }

    }



    //ADD ITEM METHOD
    public void AddItem(int id, int amount)
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
                    playerScript.currentWeight += database.items[id].itemWeight * (database.items[id].itemMaxStack - inventoryStacks[i]);
                    if (weightOverload)
                    { break; }
                    amount = amount - (database.items[id].itemMaxStack - inventoryStacks[i]);
                    inventoryStacks[i] = database.items[id].itemMaxStack;
                }

                if (amount < database.items[id].itemMaxStack)
                {
                    if ((database.items[id].itemMaxStack - inventoryStacks[i]) >= amount)
                    {
                        playerScript.currentWeight +=database.items[id].itemWeight * amount;
                        if (weightOverload)
                        { break; }
                        inventoryStacks[i] = inventoryStacks[i] + amount;
                        break;
                    }
                    if ((database.items[id].itemMaxStack - inventoryStacks[i]) < amount)
                    {
                        playerScript.currentWeight += database.items[id].itemMaxStack - inventoryStacks[i];
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
                                playerScript.currentWeight = playerScript.currentWeight + (database.items[id].itemWeight * database.items[id].itemMaxStack);
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
                                playerScript.currentWeight = playerScript.currentWeight + (database.items[id].itemWeight * amount);
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
                        if (inventory[r].itemID == 0)
                        {
                            playerScript.currentWeight = playerScript.currentWeight + (database.items[id].itemWeight * amount);
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
            playerScript.currentWeight = playerScript.currentWeight - inventory[slotId].itemWeight;
            inventory[slotId] = database.items[0];
            inventoryStacks[slotId] = 1;
        }
        else
        {
            playerScript.currentWeight = playerScript.currentWeight - inventory[slotId].itemWeight * amount;
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
                        inventory[j] = database.items[0];
                        inventoryStacks[j] = 1;
                        break;
                    }
                }
            }
        }
    }

    void ShowWarning(int windowId)
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
        if (merchantNotEnoughMoney)
        {
            GUILayout.Box("The merchant is out of money", GUILayout.Width(400), GUILayout.Height(350));
            if (GUILayout.Button("OK", GUILayout.Width(400), GUILayout.Height(50)))
            {
                merchantNotEnoughMoney = false;
            }
        }
        if (meNotEnoughMoney)
        {
            GUILayout.Box("I don't have the money to buy that!", GUILayout.Width(400), GUILayout.Height(350));
            if (GUILayout.Button("OK", GUILayout.Width(400), GUILayout.Height(50)))
            {
                meNotEnoughMoney = false;
            }
        }
        if (weightOverload)
        {
            GUILayout.Box("Too heavy! Can't carry that much!", GUILayout.Width(400), GUILayout.Height(350));
            if (GUILayout.Button("OK", GUILayout.Width(400), GUILayout.Height(50)))
            {
                weightOverload = false;
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void EquipItem(int itemId)
    {
        //adding stats
        playerScript.strength += database.items[itemId].itemAddStr;
        playerScript.agility += database.items[itemId].itemAddAgi;
        playerScript.intellect += database.items[itemId].itemAddInt;
        playerScript.vitality += database.items[itemId].itemAddVit;
        playerScript.charisma += database.items[itemId].itemAddCha;
        playerScript.minDef += database.items[itemId].itemMinDef;
        playerScript.maxDef += database.items[itemId].itemMaxDef;
        playerScript.minAtk += database.items[itemId].itemMinAttack;
        playerScript.maxAtk += database.items[itemId].itemMaxAttack;
        playerScript.speed -= database.items[itemId].itemHandlingSpeed;
        playerScript.crit += database.items[itemId].itemCritChance;

        if (database.items[itemId].itemIsHead)
        {
            if (equippedItems[0].itemID == 0)
            {
                equippedItems[0] = database.items[itemId];
                inventory[tipIdSlot] = database.items[0];
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
                inventory[tipIdSlot] = database.items[0];
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
                inventory[tipIdSlot] = database.items[0];
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
                inventory[tipIdSlot] = database.items[0];
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
                inventory[tipIdSlot] = database.items[0];
                showTip = false;
            }
            else if (equippedItems[7].itemID == 0)
            {
                equippedItems[7] = database.items[itemId];
                inventory[tipIdSlot] = database.items[0];
                showTip = false;
            }

        }
        if (database.items[itemId].itemIsMainHand)
        {
            if (equippedItems[8].itemID == 0)
            {
                equippedItems[8] = database.items[itemId];
                inventory[tipIdSlot] = database.items[0];
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
                inventory[tipIdSlot] = database.items[0];
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
                inventory[tipIdSlot] = database.items[0];
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
                inventory[tipIdSlot] = database.items[0];
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
        //removing stats
        playerScript.strength -= database.items[tipIdItem].itemAddStr;
        playerScript.agility -= database.items[tipIdItem].itemAddAgi;
        playerScript.intellect -= database.items[tipIdItem].itemAddInt;
        playerScript.vitality -= database.items[tipIdItem].itemAddVit;
        playerScript.charisma -= database.items[tipIdItem].itemAddCha;
        playerScript.minDef -= database.items[tipIdItem].itemMinDef;
        playerScript.maxDef -= database.items[tipIdItem].itemMaxDef;
        playerScript.minAtk -= database.items[tipIdItem].itemMinAttack;
        playerScript.maxAtk -= database.items[tipIdItem].itemMaxAttack;
        playerScript.speed += database.items[tipIdItem].itemHandlingSpeed;
        playerScript.crit -= database.items[tipIdItem].itemCritChance;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemID == 0)
            {
                equippedItems[tipIdSlot] = database.items[0];
                inventory[i] = database.items[tipIdItem];
                break;
            }
        }

        showTipEquipped = false;
    }

    private void OnGUI()
    {
        //menu popdown
        GUILayout.BeginArea(new Rect(Screen.width - 120, 0, 120, 500));
        GUILayout.BeginVertical();
        if (GUILayout.Button("MENU", GUILayout.Width(100), GUILayout.Height(100)))
        {
            showMenu = !showMenu;
        }
        if (showMenu)
        {
            if (GUILayout.Button("Inventory", GUILayout.Width(100), GUILayout.Height(100)))
            { characterWindowShow = true; }
            if (GUILayout.Button("Character", GUILayout.Width(100), GUILayout.Height(100)))
            { showCharacterWindow = true; }
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

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
        if (GUILayout.Button("Add Exp", GUILayout.Width(90), GUILayout.Height(25)))
        {
            playerScript.AddExp(spawnAmount);
        }
        if (GUILayout.Button("Save", GUILayout.Width(90), GUILayout.Height(25)))
        {
            SaveManager.SaveData(1);
        }
        if (GUILayout.Button("Load", GUILayout.Width(90), GUILayout.Height(25)))
        {
            SaveManager.LoadData(1);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
        //SPAWN ITEMS FOR TESTING END

        GUILayout.BeginArea(new Rect(0, 0, 500, 50));
        GUILayout.BeginHorizontal();
        GUILayout.Box(Resources.Load<Texture2D>("gp"), GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.Box("" + playerGold, GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.Box("mgp " + merchantGold, GUILayout.Width(50), GUILayout.Height(50));
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

        if (showAmount)
        {
            destroyAmountRect = GUI.Window(4, destroyAmountRect, ShowAmount, "");
        }

        if (weightOverload || meNotEnoughMoney || merchantNotEnoughMoney)
        {
            showOverloadWarningRect = GUI.Window(5, showOverloadWarningRect, ShowWarning, "");
        }

        if (showCharacterWindow)
        {
            GUI.Window(6, new Rect(0, 0, Screen.width, Screen.height), CharacterWindow, "Character window");
        }


    }
    void UseItem(int id)
    {
        bool itemIsUsed = false;
        //restore health
        if ((playerScript.currentHealth < playerScript.maxHealth) && database.items[id].itemRestoreHealth != 0)
        {
            itemIsUsed = true;
            if (database.items[id].itemRestoreHealth >= (playerScript.maxHealth - playerScript.currentHealth))
            {
                playerScript.currentHealth = playerScript.maxHealth;
            }
            else
            { playerScript.currentHealth += database.items[id].itemRestoreHealth; }
        }
        //restore mana
        if ((playerScript.currentMana < playerScript.maxMana) && (database.items[id].itemRestoreMana != 0) && playerScript.isCaster)
        {
            itemIsUsed = true;
            if (database.items[id].itemRestoreMana >= (playerScript.maxMana - playerScript.currentMana))
            {
                playerScript.currentMana = playerScript.maxMana;
            }
            else
            { playerScript.currentMana += database.items[id].itemRestoreMana; }
        }
        //restore stamina
        if ((playerScript.currentStamina < playerScript.maxStamina) && (database.items[id].itemRestoreStamina != 0) && !playerScript.isCaster)
        {
            itemIsUsed = true;
            if (database.items[id].itemRestoreMana >= (playerScript.maxStamina - playerScript.currentStamina))
            {
                playerScript.currentStamina = playerScript.maxStamina;
            }
            else
            { playerScript.currentStamina += database.items[id].itemRestoreMana; }
        }
        if (itemIsUsed)
        {
            RemoveItem(tipIdSlot, 1);
            itemIsUsed = false;
        }

        if (inventory[tipIdSlot].itemName == null)
        { showTip = false; }
    }

    //AMOUNT
    void ShowAmount(int windowId)
    {
        GUILayout.BeginArea(new Rect(0, 0, 30, 30));
        if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(30)))
        {
            showTip = true;
            showAmount = false;
            sell = false;
            buy = false;
        }
        GUILayout.EndArea();

        GUILayout.BeginVertical();

        GUILayout.BeginArea(new Rect(0, 0, 400, 350));
        GUILayout.BeginHorizontal();
        if (buy)
        {
            if (GUILayout.Button("-", GUILayout.Width(50), GUILayout.Height(50)) && itemCount > 0)
            {
                itemCount--;
            }

            GUILayout.Box("" + itemCount, GUILayout.Width(50), GUILayout.Height(50));

            if (GUILayout.Button("+", GUILayout.Width(50), GUILayout.Height(50)) && itemCount < tradeInventoryStacks[tipIdSlot])
            {
                itemCount++;
            }
        }
        else
        {
            if (GUILayout.Button("-", GUILayout.Width(50), GUILayout.Height(50)) && itemCount > 0)
            {
                itemCount--;
            }

            GUILayout.Box("" + itemCount, GUILayout.Width(50), GUILayout.Height(50));

            if (GUILayout.Button("+", GUILayout.Width(50), GUILayout.Height(50)) && itemCount < tradeInventoryStacks[tipIdSlot])
            {
                itemCount++;
            }
        }
        GUILayout.EndArea();
        GUILayout.EndHorizontal();


        GUILayout.BeginArea(new Rect(0, 350, 400, 50));
        GUILayout.BeginHorizontal();

        if (sell)
        {
            if (GUILayout.Button("Sell", GUILayout.Width(200), GUILayout.Height(50)))
            {
                Sell(itemCount);
                showAmount = false;
                sell = false;
            }

            if (GUILayout.Button("Sell All", GUILayout.Width(200), GUILayout.Height(50)))
            {
                Sell(inventoryStacks[tipIdSlot]);
                showAmount = false;
                sell = false;
            }
        }
        if (buy)
        {
            if (GUILayout.Button("Buy", GUILayout.Width(200), GUILayout.Height(50)))
            {
                Buy(itemCount);
                showAmount = false;
                buy = false;
            }

            if (GUILayout.Button("Buy All", GUILayout.Width(200), GUILayout.Height(50)))
            {
                Buy(tradeInventoryStacks[tipIdSlot]);
                showAmount = false;
                buy = false;
            }
        }
        else
        {
            if (GUILayout.Button("Destroy", GUILayout.Width(200), GUILayout.Height(50)))
            {
                RemoveItem(tipIdSlot, itemCount);
                itemCount = 1;
                showAmount = false;
            }

            if (GUILayout.Button("Destroy All", GUILayout.Width(200), GUILayout.Height(50)))
            {
                RemoveItem(tipIdSlot, inventoryStacks[tipIdSlot]);
                itemCount = 1;
                showAmount = false;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void Sell(int amount)
    {
        if ((inventory[tipIdSlot].itemCost * amount) > merchantGold)
        {
            merchantNotEnoughMoney = true;
        }
        else
        {
            for (int i = 0; i < tradeInventory.Count; i++)
            {
                //look for underfilled stacks and add to if found
                if ((tradeInventory[i].itemID == inventory[tipIdSlot].itemID) && (tradeInventory[i].itemMaxStack - tradeInventoryStacks[i] >= amount))
                {
                    tradeInventoryStacks[i] = tradeInventoryStacks[i] + amount;
                    break;
                }

                //if no underfilled stacks to add to
                if (i == tradeInventory.Count - 1)
                {
                    for (int k = 0; k < tradeInventory.Count; k++)
                    {
                        if (tradeInventory[k].itemID == 0)
                        {
                            tradeInventory[k] = inventory[tipIdSlot];
                            tradeInventoryStacks[k] = amount;
                            break;
                        }
                    }
                }
            }
            playerGold = playerGold + (inventory[tipIdSlot].itemCost * amount);
            merchantGold = merchantGold - (inventory[tipIdSlot].itemCost * amount);
            RemoveItem(tipIdSlot, amount);
        }
        updateMerchantInventory = true;
    }

    void Buy(int amount)
    {
        if ((tradeInventory[tipIdSlot].itemCost * amount) > playerGold)
        {
            meNotEnoughMoney = true;
        }
        else
        {
            AddItem(tipIdItem, amount);
            if (!weightOverload)
            {
                playerGold -= tradeInventory[tipIdSlot].itemCost * amount;
                merchantGold += tradeInventory[tipIdSlot].itemCost * amount;
                //if buy fullstack
                if (amount == tradeInventoryStacks[tipIdSlot])
                {
                    tradeInventory[tipIdSlot] = database.items[0];
                    tradeInventoryStacks[tipIdSlot] = 1;
                }
                //if buy less than a stack
                else
                {
                    tradeInventoryStacks[tipIdSlot] = tradeInventoryStacks[tipIdSlot] - amount;
                }
            }
        }
        updateMerchantInventory = true;
    }
    //ITEM TIP INVENTORY
    void ShowTip(int windowId)
    {
        GUILayout.BeginArea(new Rect(0, 0, 30, 30));
        if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(30)))
        {
            showTip = false;
            sell = false;
            buy = false;
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0, 0, 407, 407));
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Box(database.items[tipIdItem].itemIcon, GUILayout.Width(200), GUILayout.Height(200));
        if (database.items[tipIdItem].itemType == ItemsMk2.ItemType.Consumable)
        { }
        GUILayout.Box(database.items[tipIdItem].itemName + '\n' + database.items[tipIdItem].itemName + " stats go here" + '\n' + "can sell for " + database.items[tipIdItem].itemCost + " gp", GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.EndHorizontal();
        GUILayout.Box(database.items[tipIdItem].itemDesc, GUILayout.Width(407), GUILayout.Height(150));


        if (sell)
        {
            if (GUILayout.Button("Sell", GUILayout.Width(400), GUILayout.Height(50)))
            {
                //ask how much to sell if stacked
                if (inventoryStacks[tipIdSlot] > 1)
                {
                    showAmount = true;
                }
                //sell immidiately if not stackable or stack == 1
                if (inventoryStacks[tipIdSlot] == 1)
                {
                    Sell(1);
                    sell = false;
                }
                showTip = false;
            }
        }
        if (buy)
        {
            if (GUILayout.Button("Buy", GUILayout.Width(400), GUILayout.Height(50)))
            {
                //ask how much to buyl if stacked
                if (tradeInventoryStacks[tipIdSlot] > 1)
                {
                    showAmount = true;
                }
                //buy immidiately if not stackable or stack == 1
                if (tradeInventoryStacks[tipIdSlot] == 1)
                {
                    Buy(1);
                    buy = false;
                }
                showTip = false;
            }
        }
        else
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Destroy", GUILayout.Width(200), GUILayout.Height(50)))
            {
                //ask how much to destroy if stacked
                if (inventoryStacks[tipIdSlot] > 1)
                {
                    showAmount = true;
                }
                //destroy immidiately if not stackable or stack == 1
                if (inventoryStacks[tipIdSlot] == 1)
                {
                    RemoveItem(tipIdSlot, 1);
                }
                showTip = false;
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
        }
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
            playerScript.currentWeight = playerScript.currentWeight - database.items[tipIdItem].itemWeight;
            equippedItems[tipIdSlot] = database.items[0];
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
        GUILayout.BeginArea(new Rect(0, 0, 30, 30));
        if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(30)))
        {
            characterWindowShow = false;
        }
        GUILayout.EndArea();
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
        GUILayout.BeginArea(new Rect(Screen.width * 0.5f, 20, Screen.width * 0.5f, inventoryItemsBoxWidth));
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
        GUILayout.BeginArea(new Rect(Screen.width * 0.5f, inventoryItemsBoxWidth + 30, Screen.width * 0.5f, Screen.height));
        int k = 0;
        for (int i = 0; i < inventory.Count / 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (k < inventory.Count)
                {
                    inventoryDisplayed[k] = database.items[0];
                    inventoryStacksDisplayed[k] = 1;

                    if (showAll)
                    {
                        int displayedSlot = 0;
                        for (int t = 0; t < inventory.Count; t++)
                        {
                            displayedSlotsToActual[displayedSlot] = t;
                            inventoryDisplayed[displayedSlot] = inventory[t];
                            inventoryStacksDisplayed[displayedSlot] = inventoryStacks[t];
                            displayedSlot++;
                        }
                    }
                    if (showMisc)
                    {
                        int displayedSlot = 0;
                        for (int t = 0; t < inventory.Count; t++)
                        {
                            if (inventory[t].itemType == ItemsMk2.ItemType.Misc)
                            {
                                displayedSlotsToActual[displayedSlot] = t;
                                inventoryDisplayed[displayedSlot] = inventory[t];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[t];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showResources)
                    {
                        int displayedSlot = 0;
                        for (int t = 0; t < inventory.Count; t++)
                        {
                            if ((inventory[t].itemType == ItemsMk2.ItemType.Misc) && (inventory[t].itemIsResource))
                            {
                                displayedSlotsToActual[displayedSlot] = t;
                                inventoryDisplayed[displayedSlot] = inventory[t];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[t];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showConsumables)
                    {
                        int displayedSlot = 0;
                        for (int t = 0; t < inventory.Count; t++)
                        {
                            if (inventory[t].itemType == ItemsMk2.ItemType.Consumable)
                            {
                                displayedSlotsToActual[displayedSlot] = t;
                                inventoryDisplayed[displayedSlot] = inventory[t];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[t];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showEquipment)
                    {
                        int displayedSlot = 0;
                        for (int t = 0; t < inventory.Count; t++)
                        {
                            if ((inventory[t].itemType == ItemsMk2.ItemType.Armor) || (inventory[t].itemType == ItemsMk2.ItemType.Weapon) || (inventory[t].itemType == ItemsMk2.ItemType.Jewelry))
                            {
                                displayedSlotsToActual[displayedSlot] = t;
                                inventoryDisplayed[displayedSlot] = inventory[t];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[t];
                                displayedSlot++;
                            }
                        }
                    }

                    //display item icon
                    GUILayout.BeginArea(new Rect(inventoryItemsBoxWidth * j * 1.05f, inventoryItemsBoxWidth * i * 1.05f, inventoryItemsBoxWidth, inventoryItemsBoxWidth));
                    if (GUILayout.Button(inventoryDisplayed[k].itemIcon, GUILayout.Width(inventoryItemsBoxWidth), GUILayout.Height(inventoryItemsBoxWidth)))
                    {
                        if (inventoryDisplayed[k].itemID != 0)
                        {
                            showTip = true;
                            tipIdItem = inventoryDisplayed[k].itemID;
                            tipIdSlot = displayedSlotsToActual[k];
                        }
                    }
                    //display item stack
                    GUILayout.BeginArea(new Rect(inventoryItemsBoxWidth - 25, inventoryItemsBoxWidth - 20, 20, 20));

                    if (inventoryDisplayed[k].itemMaxStack > 1)
                    {
                        GUILayout.Box("" + inventoryStacksDisplayed[k], GUILayout.Height(20), GUILayout.Width(25));
                        GUILayout.Space(inventoryItemsBoxWidth - 20);
                    }
                    GUILayout.EndArea();
                    //display item stack end
                    GUILayout.EndArea();
                    k++;
                }
            }
        }
        GUILayout.EndArea();
        //encumbrance bar

        GUI.contentColor = Color.black;


        GUILayout.BeginArea(new Rect((Screen.width * 0.6f) - 95, Screen.height - 45, 400, 25));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Encumbrance");
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect((Screen.width * 0.6f) + 360, Screen.height - 45, 400, 25));
        GUILayout.BeginHorizontal();
        GUILayout.Label(playerScript.currentWeight + "/" + playerScript.maxWeight);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0.6f, Screen.height - 45, 400, 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), Resources.Load<Texture2D>("encempty"));
        GUI.DrawTexture(new Rect(0, 0, 350 * (playerScript.currentWeight / playerScript.maxWeight), 25), Resources.Load<Texture2D>("encfull"));
        GUILayout.EndArea();
        //bar area
        //labels placeholder
        GUILayout.BeginArea(new Rect(pos - new Vector2(80, 0), new Vector2(450, 100)));
        GUILayout.BeginVertical();
        GUILayout.Label("Experience");
        GUILayout.Space(12);
        GUILayout.Label("Health");
        GUILayout.Space(12);
        if (playerScript.isCaster)
        { GUILayout.Label("Mana"); }
        else
        { GUILayout.Label("Stamina"); }
        GUILayout.EndVertical();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(pos + new Vector2(360, 0), new Vector2(450, 100)));
        GUILayout.BeginVertical();
        GUILayout.Label(playerScript.currentExp + "/" + playerScript.maxExp);
        GUILayout.Space(12);
        GUILayout.Label(playerScript.currentHealth + "/" + playerScript.maxHealth);
        GUILayout.Space(12);
        if (playerScript.isCaster)
        { GUILayout.Label(playerScript.currentMana + "/" + playerScript.maxMana); }
        else
        { GUILayout.Label(playerScript.currentStamina + "/" + playerScript.maxStamina); }
        GUILayout.EndVertical();
        GUILayout.EndArea();
        //labels placeholder end

        GUILayout.BeginArea(new Rect(pos, new Vector2(450, 100)));
        //exp bar
        GUI.DrawTexture(new Rect(0, 0, 350, 25), Resources.Load<Texture2D>("expempty"));
        GUI.DrawTexture(new Rect(0, 0, 350 * ((float)playerScript.currentExp / (float)playerScript.maxExp), 25), Resources.Load<Texture2D>("expfull"));
        //hp bar
        GUI.DrawTexture(new Rect(0, 35, 350, 25), Resources.Load<Texture2D>("hpempty"));
        GUI.DrawTexture(new Rect(0, 35, 350 * ((float)playerScript.currentHealth / (float)playerScript.maxHealth), 25), Resources.Load<Texture2D>("hpfull"));
        //mp bar
        if (playerScript.isCaster)
        {
            GUI.DrawTexture(new Rect(0, 70, 350, 25), Resources.Load<Texture2D>("mpempty"));
            GUI.DrawTexture(new Rect(0, 70, 350 * ((float)playerScript.currentMana / (float)playerScript.maxMana), 25), Resources.Load<Texture2D>("mpfull"));
        }
        //sp bar
        else
        {
            GUI.DrawTexture(new Rect(0, 70, 350, 25), Resources.Load<Texture2D>("staempty"));
            GUI.DrawTexture(new Rect(0, 70, 350 * ((float)playerScript.currentStamina / (float)playerScript.maxStamina), 25), Resources.Load<Texture2D>("stafull"));
        }
        GUILayout.EndArea();

    }

    void TradingWindow(int windowId)
    {
        GUILayout.BeginArea(new Rect(0, 0, 30, 30));
        if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(30)))
        {
            showTradingWindow = false;
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0.5f, 20, Screen.width * 0.5f, inventoryItemsBoxWidth));
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

        //trade inventory
        GUILayout.BeginArea(new Rect(Screen.width * 0.005f, inventoryItemsBoxWidth + 30, Screen.width * 0.5f, Screen.height));
        int q = 0;
        for (int i = 0; i < tradeInventory.Count / 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (q < tradeInventory.Count)
                {
                    //display item icon
                    GUILayout.BeginArea(new Rect(inventoryItemsBoxWidth * j * 1.05f, inventoryItemsBoxWidth * i * 1.05f, inventoryItemsBoxWidth, inventoryItemsBoxWidth));
                    if (GUILayout.Button(tradeInventory[q].itemIcon, GUILayout.Width(inventoryItemsBoxWidth), GUILayout.Height(inventoryItemsBoxWidth)))
                    {
                        if (tradeInventory[q].itemID != 0)
                        {
                            sell = false;
                            buy = true;
                            showTip = true;
                            tipIdItem = tradeInventory[q].itemID;
                            tipIdSlot = q;
                        }
                    }
                    //display item stack
                    GUILayout.BeginArea(new Rect(inventoryItemsBoxWidth - 25, inventoryItemsBoxWidth - 20, 20, 20));
                    if (tradeInventory[q].itemMaxStack > 1)
                    {
                        GUILayout.Box("" + tradeInventoryStacks[q], GUILayout.Height(20), GUILayout.Width(25));
                        GUILayout.Space(inventoryItemsBoxWidth - 20);
                    }
                    GUILayout.EndArea();
                    //display item stack end
                    GUILayout.EndArea();
                    q++;
                }
            }
        }
        GUILayout.EndArea();
        //char inventory
        GUILayout.BeginArea(new Rect(Screen.width * 0.5f, inventoryItemsBoxWidth + 30, Screen.width * 0.5f, Screen.height));
        int k = 0;
        for (int i = 0; i < inventory.Count / 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (k < inventory.Count)
                {
                    inventoryDisplayed[k] = database.items[0];
                    inventoryStacksDisplayed[k] = 1;

                    if (showAll)
                    {
                        int displayedSlot = 0;
                        for (int t = 0; t < inventory.Count; t++)
                        {
                            displayedSlotsToActual[displayedSlot] = t;
                            inventoryDisplayed[displayedSlot] = inventory[t];
                            inventoryStacksDisplayed[displayedSlot] = inventoryStacks[t];
                            displayedSlot++;
                        }
                    }
                    if (showMisc)
                    {
                        int displayedSlot = 0;
                        for (int t = 0; t < inventory.Count; t++)
                        {
                            if (inventory[t].itemType == ItemsMk2.ItemType.Misc)
                            {
                                displayedSlotsToActual[displayedSlot] = t;
                                inventoryDisplayed[displayedSlot] = inventory[t];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[t];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showResources)
                    {
                        int displayedSlot = 0;
                        for (int t = 0; t < inventory.Count; t++)
                        {
                            if ((inventory[t].itemType == ItemsMk2.ItemType.Misc) && (inventory[t].itemIsResource))
                            {
                                displayedSlotsToActual[displayedSlot] = t;
                                inventoryDisplayed[displayedSlot] = inventory[t];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[t];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showConsumables)
                    {
                        int displayedSlot = 0;
                        for (int t = 0; t < inventory.Count; t++)
                        {
                            if (inventory[t].itemType == ItemsMk2.ItemType.Consumable)
                            {
                                displayedSlotsToActual[displayedSlot] = t;
                                inventoryDisplayed[displayedSlot] = inventory[t];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[t];
                                displayedSlot++;
                            }
                        }
                    }
                    if (showEquipment)
                    {
                        int displayedSlot = 0;
                        for (int t = 0; t < inventory.Count; t++)
                        {
                            if ((inventory[t].itemType == ItemsMk2.ItemType.Armor) || (inventory[t].itemType == ItemsMk2.ItemType.Weapon) || (inventory[t].itemType == ItemsMk2.ItemType.Jewelry))
                            {
                                displayedSlotsToActual[displayedSlot] = t;
                                inventoryDisplayed[displayedSlot] = inventory[t];
                                inventoryStacksDisplayed[displayedSlot] = inventoryStacks[t];
                                displayedSlot++;
                            }
                        }
                    }

                    //display item icon
                    GUILayout.BeginArea(new Rect(inventoryItemsBoxWidth * j * 1.05f, inventoryItemsBoxWidth * i * 1.05f, inventoryItemsBoxWidth, inventoryItemsBoxWidth));
                    if (GUILayout.Button(inventoryDisplayed[k].itemIcon, GUILayout.Width(inventoryItemsBoxWidth), GUILayout.Height(inventoryItemsBoxWidth)))
                    {
                        if (inventoryDisplayed[k].itemID != 0)
                        {
                            buy = false;
                            sell = true;
                            showTip = true;
                            tipIdItem = inventoryDisplayed[k].itemID;
                            tipIdSlot = displayedSlotsToActual[k];
                        }
                    }
                    //display item stack
                    GUILayout.BeginArea(new Rect(inventoryItemsBoxWidth - 25, inventoryItemsBoxWidth - 20, 20, 20));

                    if (inventoryDisplayed[k].itemMaxStack > 1)
                    {
                        GUILayout.Box("" + inventoryStacksDisplayed[k], GUILayout.Height(20), GUILayout.Width(25));
                        GUILayout.Space(inventoryItemsBoxWidth - 20);
                    }
                    GUILayout.EndArea();
                    //display item stack end
                    GUILayout.EndArea();
                    k++;
                }
            }
        }
        GUILayout.EndArea();

        GUI.contentColor = Color.black;


        GUILayout.BeginArea(new Rect((Screen.width * 0.6f) - 95, Screen.height - 45, 400, 25));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Encumbrance");
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect((Screen.width * 0.6f) + 360, Screen.height - 45, 400, 25));
        GUILayout.BeginHorizontal();
        GUILayout.Label(playerScript.currentWeight + "/" + playerScript.maxWeight);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0.6f, Screen.height - 45, 400, 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), Resources.Load<Texture2D>("encempty"));
        GUI.DrawTexture(new Rect(0, 0, 350 * (playerScript.currentWeight / playerScript.maxWeight), 25), Resources.Load<Texture2D>("encfull"));
        GUILayout.EndArea();
    }

    void CharacterWindow(int windowId)
    {
        GUILayout.BeginArea(new Rect(0, 0, 30, 30));
        if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(30)))
        {
            showCharacterWindow = false;
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0.47f, Screen.height * 0.17f, 500, 150));
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Label("Max health = " + playerScript.maxHealth);
        if (playerScript.isCaster)
        { GUILayout.Label("Max mana = " + playerScript.maxMana); }
        else
        { GUILayout.Label("Max stamina = " + playerScript.maxStamina); }
        GUILayout.Label("Max weight = " + playerScript.maxWeight);
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Attack = " + playerScript.minAtk + "-" + playerScript.maxAtk);
        GUILayout.Label("Defence = " + playerScript.minDef + "-" + playerScript.maxDef);
        GUILayout.Label("Speed = " + playerScript.speed);
        GUILayout.Label("Crit chance = " + playerScript.crit * 100 + "%");
        GUILayout.Label("Dodge chance = " + playerScript.crit * 100 + "%");
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0.47f, Screen.height * 0.4f, 70, 500));
        GUILayout.BeginVertical();
        for (int i = 0; i < 4; i++)
        {
            GUILayout.Button("Class " + (i + 1), GUILayout.Width(70), GUILayout.Height(70));
            GUILayout.Space(40);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0.6f, Screen.height * 0.35f, 300, 20));
        GUILayout.Box(playerScript.playerClass + " class skilltree:");
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0.62f, Screen.height * 0.13f, 700, 70));
        GUILayout.Box("Available points = " + playerScript.statPoints, GUILayout.Width(150), GUILayout.Height(25));
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0.45f, Screen.height * 0.05f, 700, 70));
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        if (GUILayout.Button("STR", GUILayout.Width(50), GUILayout.Height(30)))
        {
            if (playerScript.statPoints > 0)
            {
                playerScript.statPoints--;
                playerScript.strength++;
            }
        }
        GUILayout.Box("" + playerScript.strength, GUILayout.Width(50), GUILayout.Height(20));
        GUILayout.EndVertical();
        GUILayout.Space(-100);
        GUILayout.BeginVertical();
        if (GUILayout.Button("VIT", GUILayout.Width(50), GUILayout.Height(30)))
        {
            if (playerScript.statPoints > 0)
            {
                playerScript.statPoints--;
                playerScript.vitality++;
            }
        }
        GUILayout.Box("" + playerScript.vitality, GUILayout.Width(50), GUILayout.Height(20));
        GUILayout.EndVertical();
        GUILayout.Space(-100);
        GUILayout.BeginVertical();
        if (GUILayout.Button("AGI", GUILayout.Width(50), GUILayout.Height(30)))
        {
            if (playerScript.statPoints > 0)
            {
                playerScript.statPoints--;
                playerScript.agility++;
            }
        }
        GUILayout.Box("" + playerScript.agility, GUILayout.Width(50), GUILayout.Height(20));
        GUILayout.EndVertical();
        GUILayout.Space(-100);
        GUILayout.BeginVertical();
        if (GUILayout.Button("INT", GUILayout.Width(50), GUILayout.Height(30)))
        {
            if (playerScript.statPoints > 0)
            {
                playerScript.statPoints--;
                playerScript.intellect++;
            }
        }
        GUILayout.Box("" + playerScript.intellect, GUILayout.Width(50), GUILayout.Height(20));
        GUILayout.EndVertical();
        GUILayout.Space(-100);
        GUILayout.BeginVertical();
        if (GUILayout.Button("CHA", GUILayout.Width(50), GUILayout.Height(30)))
        {
            if (playerScript.statPoints > 0)
            {
                playerScript.statPoints--;
                playerScript.charisma++;
            }
        }
        GUILayout.Box("" + playerScript.charisma, GUILayout.Width(50), GUILayout.Height(20));
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0.15f, Screen.height * 0.1f, 200, 50));
        GUILayout.Box("Character level " + playerScript.level);
        GUILayout.EndArea();

        //bar area
        GUI.contentColor = Color.black;
        //labels placeholder
        GUILayout.BeginArea(new Rect(pos - new Vector2(80, 0), new Vector2(450, 100)));
        GUILayout.BeginVertical();
        GUILayout.Label("Experience");
        GUILayout.Space(12);
        GUILayout.Label("Health");
        GUILayout.Space(12);
        if (playerScript.isCaster)
        { GUILayout.Label("Mana"); }
        else
        { GUILayout.Label("Stamina"); }
        GUILayout.EndVertical();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(pos + new Vector2(360, 0), new Vector2(450, 100)));
        GUILayout.BeginVertical();
        GUILayout.Label(playerScript.currentExp + "/" + playerScript.maxExp);
        GUILayout.Space(12);
        GUILayout.Label(playerScript.currentHealth + "/" + playerScript.maxHealth);
        GUILayout.Space(12);
        if (playerScript.isCaster)
        { GUILayout.Label(playerScript.currentMana + "/" + playerScript.maxMana); }
        else
        { GUILayout.Label(playerScript.currentStamina + "/" + playerScript.maxStamina); }
        GUILayout.EndVertical();
        GUILayout.EndArea();
        //labels placeholder end

        GUILayout.BeginArea(new Rect(pos, new Vector2(450, 100)));
        //exp bar
        GUI.DrawTexture(new Rect(0, 0, 350, 25), Resources.Load<Texture2D>("expempty"));
        GUI.DrawTexture(new Rect(0, 0, 350 * ((float)playerScript.currentExp / (float)playerScript.maxExp), 25), Resources.Load<Texture2D>("expfull"));
        //hp bar
        GUI.DrawTexture(new Rect(0, 35, 350, 25), Resources.Load<Texture2D>("hpempty"));
        GUI.DrawTexture(new Rect(0, 35, 350 * ((float)playerScript.currentHealth / (float)playerScript.maxHealth), 25), Resources.Load<Texture2D>("hpfull"));
        //mp bar
        if (playerScript.isCaster)
        {
            GUI.DrawTexture(new Rect(0, 70, 350, 25), Resources.Load<Texture2D>("mpempty"));
            GUI.DrawTexture(new Rect(0, 70, 350 * ((float)playerScript.currentMana / (float)playerScript.maxMana), 25), Resources.Load<Texture2D>("mpfull"));
        }
        //sp bar
        else
        {
            GUI.DrawTexture(new Rect(0, 70, 350, 25), Resources.Load<Texture2D>("staempty"));
            GUI.DrawTexture(new Rect(0, 70, 350 * ((float)playerScript.currentStamina / (float)playerScript.maxStamina), 25), Resources.Load<Texture2D>("stafull"));
        }
        GUILayout.EndArea();

    }

    private void Update()
    {
        if (displayedSlotsToActual.Count != inventory.Count)
        {
            if (displayedSlotsToActual.Count < inventory.Count)
            {
                for (int i = 0; i < (inventory.Count - displayedSlotsToActual.Count); i++)
                { displayedSlotsToActual.Add(1); }
            }
            if (displayedSlotsToActual.Count > inventory.Count)
            {
                for (int i = 0; i < (displayedSlotsToActual.Count - inventory.Count); i++)
                { displayedSlotsToActual.Remove(1); }
            }
        }

        //adjust inventory slots amount
        if (inventory[inventory.Count - 1].itemID != 0)
        { ModifyInventoryRows(1); }
        if ((inventory.Count > 20) && inventory[inventory.Count - 6].itemID == 0)
        { ModifyInventoryRows(-1); }
        //adjust trade inventory slots amount
        if (tradeInventory.Count > 0)
        {
            if (tradeInventory[tradeInventory.Count - 1].itemID != 0)
            { ModifyTradeInventoryRows(1); }
            if ((tradeInventory.Count > 20) && tradeInventory[tradeInventory.Count - 6].itemID == 0)
            { ModifyTradeInventoryRows(-1); }
        }

        test2.x = test1.x / Screen.width;
        test2.y = test1.y / Screen.height;
    }
}