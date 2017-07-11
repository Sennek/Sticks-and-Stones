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
    //inventory item slots variables
    public float inventoryItemsXPos;
    public float inventoryItemsYPos;
    float inventoryItemsRowWidth;
    float inventoryItemsRowHeight;
    private float inventoryItemsBoxHeight;
    public float inventoryItemsBoxWidth;
    public int InventoryItemsRowCount = 7;
    public int InventoryItemsBoxInRow = 5;

    private int itemCount = 1;
    private int tipId;


    public Vector2 testVector1 = new Vector2(100, 100);
    public Vector2 testVector2 = new Vector2(100, 100);
    public int spawnId;
    public int spawnAmount;


    public List<ItemsMk2> inventory = new List<ItemsMk2>();
    public List<ItemsMk2> emptySlots = new List<ItemsMk2>();
    public List<int> inventoryStacks = new List<int>();
    //CALLS TO OTHER SCRIPTS
    private ItemDatabaseMk2 database;
    private playerscript playerscript;

    //inventory item type tabs variables

    //equipped items variables
    public Vector2 horizontalTop = new Vector2(169, 32.5f);
    public Vector2 horizontalBottom = new Vector2(169, 581.1f);
    public Vector2 VerticalLeft = new Vector2(21.7f, 163.2f);
    public Vector2 VerticalRight = new Vector2(420.5f, 252.2f);
    //TESTERSTER

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
            emptySlots.Add(new ItemsMk2());
            inventoryStacks.Add(1);
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
                    ModifyPlayerWeight(database.items[id].itemWeight * (database.items[id].itemMaxStack - inventoryStacks[i]));
                    if (weightOverload)
                    { break; }
                    amount = amount - (database.items[id].itemMaxStack - inventoryStacks[i]);
                    inventoryStacks[i] = database.items[id].itemMaxStack;
                }

                if (amount < database.items[id].itemMaxStack)
                {
                    if ((database.items[id].itemMaxStack - inventoryStacks[i]) >= amount)
                    {
                        ModifyPlayerWeight(database.items[id].itemWeight * amount);
                        if (weightOverload)
                        { break; }
                        inventoryStacks[i] = inventoryStacks[i] + amount;
                        break;
                    }
                    if ((database.items[id].itemMaxStack - inventoryStacks[i]) < amount)
                    {
                        ModifyPlayerWeight(database.items[id].itemMaxStack - inventoryStacks[i]);
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
                                ModifyPlayerWeight(database.items[id].itemWeight * database.items[id].itemMaxStack);
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
                                ModifyPlayerWeight(database.items[id].itemWeight * amount);
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
                            ModifyPlayerWeight(database.items[id].itemWeight * amount);
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


    void ModifyPlayerWeight(float inputWeight)
    {
        if (inputWeight > (playerscript.maxWeight - playerscript.currentWeight))
        {
            weightOverload = true;
        }

        if (inputWeight <= (playerscript.maxWeight - playerscript.currentWeight))
        {
            playerscript.currentWeight = playerscript.currentWeight + inputWeight;
        }
    }
    void ShowOverloadWarning(int windowId)
    {
        GUILayout.BeginArea(new Rect(0, 0, 30, 30));
        if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(30)))
        {
            weightOverload = false;
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0, 0, 407, 407));
        GUILayout.BeginVertical();
        GUILayout.Box("Too heavy! Can't carry that much!", GUILayout.Width(400), GUILayout.Height(350));
        if (GUILayout.Button("OK", GUILayout.Width(400), GUILayout.Height(50)))
        {
            weightOverload = false;
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
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

        if (GUILayout.Button("+", GUILayout.Width(50), GUILayout.Height(50)) && itemCount < inventoryStacks[tipId])
        {
            itemCount++;
        }
        GUILayout.EndArea();
        GUILayout.EndHorizontal();


        GUILayout.BeginArea(new Rect(0, 350, 400, 50));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Destroy", GUILayout.Width(200), GUILayout.Height(50)))
        {
            if (itemCount == inventoryStacks[tipId]) // if input == full stack, destroy all
            {
                playerscript.currentWeight = playerscript.currentWeight - (inventory[tipId].itemWeight * itemCount);
                itemCount = 1;
                inventoryStacks[tipId] = 1;
                inventory[tipId] = emptySlots[tipId];
                showDestroyAmount = false;
            }
            else //if input != full stack, destroy input count
            {
                inventoryStacks[tipId] = inventoryStacks[tipId] - itemCount;
                showDestroyAmount = false;
                playerscript.currentWeight = playerscript.currentWeight - (inventory[tipId].itemWeight * itemCount);
                itemCount = 1;
            }
        }

        if (GUILayout.Button("Destroy All", GUILayout.Width(200), GUILayout.Height(50)))
        {
            playerscript.currentWeight = playerscript.currentWeight - (inventory[tipId].itemWeight * inventoryStacks[tipId]);
            itemCount = 1;
            inventoryStacks[tipId] = 1;
            inventory[tipId] = emptySlots[tipId];
            showDestroyAmount = false;
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    //ITEM TIP
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
        GUILayout.Box(inventory[tipId].itemIcon, GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.Box(inventory[tipId].itemName + '\n' + inventory[tipId].itemName + " stats go here", GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.EndHorizontal();
        GUILayout.Box(inventory[tipId].itemDesc, GUILayout.Width(407), GUILayout.Height(150));


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Destroy", GUILayout.Width(200), GUILayout.Height(50)))
        {
            //ask how much to destroy if stacked
            if (inventoryStacks[tipId] > 1)
            {
                showDestroyAmount = true;
                showTip = false;
            }
            //destroy immidiately if not stackable or stack == 1
            if (inventoryStacks[tipId] == 1)
            {
                showTip = false;
                inventoryStacks[tipId] = 0;
                inventory[tipId] = emptySlots[tipId];
            }
        }
        GUILayout.Button("Use", GUILayout.Width(200), GUILayout.Height(50));
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
            AddItem(spawnId, spawnAmount);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

        //horizontal top
        GUILayout.BeginArea(new Rect(horizontalTop, new Vector2(inventoryItemsBoxWidth * 2.25f, inventoryItemsBoxHeight)));
        GUILayout.BeginHorizontal();
        GUILayout.Button("Head", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth));
        GUILayout.Button("Amulet", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        //horizontal bottom
        GUILayout.BeginArea(new Rect(horizontalBottom, new Vector2(inventoryItemsBoxWidth * 2.25f, inventoryItemsBoxHeight)));
        GUILayout.BeginHorizontal();
        GUILayout.Button("Legs", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth));
        GUILayout.Button("Feet", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        //vertical left
        GUILayout.BeginArea(new Rect(VerticalLeft, new Vector2(inventoryItemsBoxWidth, inventoryItemsBoxHeight * 4.25f)));
        GUILayout.BeginVertical();
        GUILayout.Button("Torso", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth));
        GUILayout.Button("Hands", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth));
        GUILayout.Button("Ring1", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth));
        GUILayout.Button("Ring2", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth));
        GUILayout.EndVertical();
        GUILayout.EndArea();

        //vertical right
        GUILayout.BeginArea(new Rect(VerticalRight, new Vector2(inventoryItemsBoxWidth, inventoryItemsBoxHeight * 2.25f)));
        GUILayout.BeginVertical();
        GUILayout.Button("L.Hand", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth));
        GUILayout.Button("R.Hand", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth));
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
                        tipId = j;
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