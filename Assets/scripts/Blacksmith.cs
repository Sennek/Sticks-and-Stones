using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XmlLoader;
using System.IO;
using System.Xml.Serialization;

public class Blacksmith : MonoBehaviour
{

    private int smithyLevel = 1;
    public int hasGold = 500;
    GameObject player;
    bool showInteractionWindow = false;
    private InventoryGUI inventoryGui;
    private ItemDatabaseMk2 database;
    private Levelmanager levelmanager;
    private playerscript playerscript;
    public Dialogue dialogue;
    public bool showFirstNode = true;
    public int nodeToDisplay;
    public int nodeToDisplayFirst;
    public bool showDialogueWindow;
    Rect dialogueWindowRect = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 500);

    public List<ItemsMk2> inventory = new List<ItemsMk2>();
    public List<int> inventoryStacks = new List<int>();

    private void Start()
    {
        playerscript = GameObject.Find("Player").GetComponent<playerscript>();
        database = GameObject.Find("ItemDB").GetComponent<ItemDatabaseMk2>();
        player = GameObject.Find("Player");
        inventoryGui = GameObject.Find("ItemDB").GetComponent<InventoryGUI>();

        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue));
        TextAsset xml = (TextAsset)Resources.Load("blacksmith", typeof(TextAsset));
        using (StringReader stream = new StringReader(xml.text))
        { dialogue = (Dialogue)serializer.Deserialize(stream); }
        for (int i = 0; i < 20; i++)
        {
            inventory.Add(database.items[0]);
            inventoryStacks.Add(1);
        }
        for (int i = 0; i < Random.Range(10, 15); i++)
        {
            int k = Random.Range(1, 17);
            inventory[i] = database.items[k];
            inventoryStacks[i] = Random.Range(1, database.items[k].itemMaxStack);
        }

    }
    void OnMouseDown()
    {
        inventoryGui.merchantGold = hasGold;
        inventoryGui.tradeInventory = inventory;
        inventoryGui.tradeInventoryStacks = inventoryStacks;
        nodeToDisplay = nodeToDisplayFirst;
        showDialogueWindow = true;
    }


    private void OnGUI()
    {
        if (showDialogueWindow)
        {
            dialogueWindowRect = GUI.Window(1, dialogueWindowRect, DialogueWindowMethod, "");
        }
    }

    void DialogueWindowMethod(int windowId)
    {
        GUILayout.BeginArea(new Rect(0, 0, 500, 500));
        GUILayout.BeginVertical();

        if (nodeToDisplay >= 0)
        {
            if (dialogue.nodes[nodeToDisplay].toId != 0)
            { nodeToDisplayFirst = dialogue.nodes[nodeToDisplay].toId; }

            GUILayout.Box(dialogue.nodes[nodeToDisplay].text, GUILayout.Width(500), GUILayout.Height(300));
            for (int i = 0; i < dialogue.nodes[nodeToDisplay].options.Count; i++)
            {
                if (GUILayout.Button(dialogue.nodes[nodeToDisplay].options[i].text, GUILayout.Width(500), GUILayout.Height(25)))
                {

                    nodeToDisplay = dialogue.nodes[nodeToDisplay].options[i].id;

                    if (nodeToDisplay < 0)
                    {
                        showDialogueWindow = false;
                        break;
                    }
                    if (nodeToDisplay == 12)
                    {
                        inventoryGui.showTradingWindow = true;
                        showDialogueWindow = false;
                        break;
                    }
                    if (nodeToDisplay == 14)
                    {
                        if (inventoryGui.playerGold >= 100)
                        {
                            inventoryGui.playerGold -= 100;
                            playerscript.AddExp(50);
                        }
                        else
                        {
                            nodeToDisplay = 15;
                        }
                    }
                    if (nodeToDisplay == 10)
                    {
                        inventoryGui.playerGold += 50;
                    }

                }

            }
        }


        GUILayout.EndVertical();

        GUILayout.EndArea();
    }

    private void Update()
    {
        if (inventoryGui.updateMerchantInventory)
        {
            inventory = inventoryGui.tradeInventory;
            inventoryStacks = inventoryGui.tradeInventoryStacks;
            hasGold = inventoryGui.merchantGold;
            inventoryGui.updateMerchantInventory = false;
        }
    }

}
