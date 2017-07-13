using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smithy : MonoBehaviour {

    public int smithyLevel = 1;
    GameObject player;
    bool playerWithinRange = false;
    bool showInteractionWindow = false;
    private InventoryGUI inventory;

    private void Start()
    {
        player = GameObject.Find("Player");
        inventory = GameObject.Find("ItemDB").GetComponent<InventoryGUI>();
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        playerWithinRange = true;
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        playerWithinRange = false;
        showInteractionWindow = false;
    }

    private void OnGUI()
    {
        if (showInteractionWindow)
        {
            GUI.Window(0, new Rect(100, 100, 300, 250), ShowInteraction, "Smithy");
        }   
    }

    void ShowInteraction(int windowID)
    {
        GUI.Label(new Rect(20, 30, 300, 100), "Smithy level is " + smithyLevel+". Do you want to upgrade?");

        if (GUI.Button(new Rect(50, 150, 75, 30), "Yes"))
        {
            upgrade();
        }

        if (GUI.Button(new Rect(130, 150, 75, 30), "No, i want to trade"))
        {
            showInteractionWindow = false;
            inventory.showTradingWindow = true;
        }
    }

    void upgrade()
    {
        smithyLevel++;
    }

    private void Update()
    {
        if((Input.GetButtonDown("Interact")) && playerWithinRange && !showInteractionWindow)
        {
            showInteractionWindow = true;
        }
    }

}
