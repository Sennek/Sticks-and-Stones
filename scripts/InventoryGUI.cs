using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGUI : MonoBehaviour {
    Rect characterWindowRect;
    public bool characterWindowShow = true;
    //inventory item slots variables



    public float inventoryItemsXPos;
    public float inventoryItemsYPos;
    float inventoryItemsRowWidth;
    float inventoryItemsRowHeight;
    private float inventoryItemsBoxHeight;
    public float inventoryItemsBoxWidth;
    public int InventoryItemsRowCount = 7;
    public int InventoryItemsBoxInRow = 5;

    public float screenHeight;
    public float screenWidth;
    //inventory item type tabs variables

    //equipped items variables
    public Vector2 horizontalTop = new Vector2(169, 32.5f);
    public Vector2 horizontalBottom = new Vector2(169, 581.1f);
    public Vector2 VerticalLeft = new Vector2(21.7f, 163.2f);
    public Vector2 VerticalRight = new Vector2(420.5f, 252.2f);
    //TESTERSTER
    public Texture2D test;

    //progress bars variables
    public Texture2D encFull;
    public Texture2D encEmpty;
    public float maxWeight = 120;
    public float currentWeight = 50;

    public Vector2 pos = new Vector2(114, 708);

    private Dictionary<int, string> inventoryNameDictionary;



    private void Start()
    {

       
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        //inventory item slots variables
        characterWindowRect = new Rect(0, 0, Screen.width, Screen.height);
        inventoryItemsXPos = Screen.width * 0.5f;
        inventoryItemsBoxWidth = Screen.width * 0.0857f;
        inventoryItemsYPos = inventoryItemsBoxWidth + inventoryItemsBoxWidth / 3;
        inventoryItemsBoxHeight = inventoryItemsBoxWidth;

        //inventory item type tabs variables
    }


    private void OnGUI()
    {
        if (characterWindowShow)
        {
            characterWindowRect = GUI.Window(0, characterWindowRect, characterWindowMethod, "Character screen");
        }
    }

    void characterWindowMethod(int windowId)
    {
        // character equipped slots
        inventoryNameDictionary = new Dictionary<int, string>()
    {
        {0, string.Empty},
        {1, string.Empty},
        {2, string.Empty},
        {3, string.Empty}
    };

        inventoryNameDictionary[0] = ItemClass.rockItem.name;

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
        GUILayout.Button(inventoryNameDictionary[0], GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth));
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
        GUILayout.BeginArea(new Rect(inventoryItemsXPos, inventoryItemsYPos, inventoryItemsBoxWidth * InventoryItemsBoxInRow * 1.25f, inventoryItemsBoxHeight * InventoryItemsRowCount * 1.25f));

        for (int i = 0; i < InventoryItemsRowCount; i++)
        {
            GUILayout.BeginHorizontal();
            for (int t = 0; t < InventoryItemsBoxInRow; t++)
            {
                if (GUILayout.Button("Empty slot", GUILayout.Height(inventoryItemsBoxHeight), GUILayout.Width(inventoryItemsBoxWidth)))
                {

                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();

        //encumbrance bar
        GUI.BeginGroup(new Rect(Screen.width * 0.6f, Screen.height - 45, 350, 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), encEmpty);

        GUI.BeginGroup(new Rect(0, 0, 350 * (currentWeight / maxWeight), 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), encFull);
        GUI.EndGroup();

        GUI.EndGroup();

        //bar area

        GUILayout.BeginArea(new Rect(pos, new Vector2(360, 100)));

        //exp bar

        GUI.BeginGroup(new Rect(0, 0, 350, 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), encEmpty);

        GUI.BeginGroup(new Rect(0, 0, 350 * (currentWeight / maxWeight), 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), encFull);
        GUI.EndGroup();

        GUI.EndGroup();

        //hp bar

        GUI.BeginGroup(new Rect(0, 35, 350, 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), encEmpty);

        GUI.BeginGroup(new Rect(0, 0, 350 * (currentWeight / maxWeight), 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), encFull);
        GUI.EndGroup();

        GUI.EndGroup();

        //mp/sp bar

        GUI.BeginGroup(new Rect(0, 70, 350, 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), encEmpty);

        GUI.BeginGroup(new Rect(0, 0, 350 * (currentWeight / maxWeight), 25));
        GUI.DrawTexture(new Rect(0, 0, 350, 25), encFull);
        GUI.EndGroup();

        GUI.EndGroup();

        GUILayout.EndArea();

    }

}