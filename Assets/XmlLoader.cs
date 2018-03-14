using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Xml.Serialization;

namespace XmlLoader
{
    
    [Serializable, XmlRoot("Dialogue")]
    public class Dialogue
    {
        [XmlElement("Node")]
        public List<Node> nodes = new List<Node>();
    }

    [Serializable]
    public class Node
    {
        [XmlAttribute("OpenNextId")]
        public int toId;
        [XmlAttribute("Id")]
        public int id;
        [XmlElement("Text")]
        public string text;
        [XmlElement("Option")]
        public List<Option> options = new List<Option>();
    }
    
    [Serializable]
    public class Option
    {
        [XmlAttribute("ToNodeId")]
        public int id;
        [XmlText]
        public string text;
    }

    public class SaveManager
    {

        public string savePath;
        public PlayerData playerData = new PlayerData();
        public static void SaveData(int slot)
        {
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + slot + ".xml", FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
            StreamWriter sw = new StreamWriter(stream);
            PlayerData playerData = new PlayerData();
            playerscript playerScript = GameObject.Find("Player").GetComponent<playerscript>();
            InventoryGUI inventoryScript = GameObject.Find("ItemDB").GetComponent<InventoryGUI>();
            ItemDatabaseMk2 database = GameObject.Find("ItemDB").GetComponent<ItemDatabaseMk2>();
            for (int i = 0; i < inventoryScript.inventory.Count; i++)
            { playerData.inventoryId.Add(inventoryScript.inventory[i].itemID); }
            for (int i = 0; i < inventoryScript.equippedItems.Count; i++)
            { playerData.inventoryEquippedId.Add(inventoryScript.equippedItems[i].itemID); }
            playerData.inventoryStacks = inventoryScript.inventoryStacks;
            playerData.maxWeight = playerScript.maxWeight;
            playerData.currentWeight = playerScript.currentWeight;
            playerData.maxHealth = playerScript.maxHealth;
            playerData.currentHealth = playerScript.currentHealth;
            playerData.maxMana = playerScript.maxMana;
            playerData.currentMana = playerScript.currentMana;
            playerData.maxStamina = playerScript.maxStamina;
            playerData.currentStamina = playerScript.currentStamina;
            playerData.maxExp = playerScript.maxExp;
            playerData.currentExp = playerScript.currentExp;
            playerData.isCaster = playerScript.isCaster;
            playerData.level = playerScript.level;
            playerData.statPoints = playerScript.statPoints;
            playerData.playerClass = playerScript.playerClass;
            playerData.speed = playerScript.speed;
            playerData.dodge = playerScript.dodge;
            playerData.crit = playerScript.crit;
            playerData.minDef = playerScript.minDef;
            playerData.maxDef = playerScript.maxDef;
            playerData.minAtk = playerScript.minAtk;
            playerData.maxAtk = playerScript.maxAtk;
            playerData.strength = playerScript.strength;
            playerData.agility = playerScript.agility;
            playerData.intellect = playerScript.intellect;
            playerData.charisma = playerScript.charisma;
            playerData.vitality = playerScript.vitality;
            serializer.Serialize(sw, playerData);
            sw.Close();
            stream.Close();
        }

        public static void LoadData(int slot)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
            using (Stream stream = new FileStream(Application.persistentDataPath + "/" + slot + ".xml", FileMode.Open))
            {
                PlayerData playerData = (PlayerData)serializer.Deserialize(stream);
                playerscript playerScript = GameObject.Find("Player").GetComponent<playerscript>();
                InventoryGUI inventoryScript = GameObject.Find("ItemDB").GetComponent<InventoryGUI>();
                ItemDatabaseMk2 database = GameObject.Find("ItemDB").GetComponent<ItemDatabaseMk2>();
                for (int i = 0; i < playerData.inventoryId.Count; i++)
                {
                    inventoryScript.inventory[i] = database.items[playerData.inventoryId[i]];

                }
                for (int i = 0; i < playerData.inventoryEquippedId.Count; i++)
                {
                    inventoryScript.equippedItems[i] = database.items[playerData.inventoryEquippedId[i]];
                }
                inventoryScript.inventoryStacks = playerData.inventoryStacks;
                playerScript.maxWeight = playerData.maxWeight;
                playerScript.currentWeight = playerData.currentWeight;
                playerScript.maxHealth = playerData.maxHealth;
                playerScript.currentHealth = playerData.currentHealth;
                playerScript.maxMana = playerData.maxMana;
                playerScript.currentMana = playerData.currentMana;
                playerScript.maxStamina = playerData.maxStamina;
                playerScript.currentStamina = playerData.currentStamina;
                playerScript.maxExp = playerData.maxExp;
                playerScript.currentExp = playerData.currentExp;
                playerScript.isCaster = playerData.isCaster;
                playerScript.level = playerData.level;
                playerScript.statPoints = playerData.statPoints;
                playerScript.playerClass = playerData.playerClass;
                playerScript.speed = playerData.speed;
                playerScript.dodge = playerData.dodge;
                playerScript.crit = playerData.crit;
                playerScript.minDef = playerData.minDef;
                playerScript.maxDef = playerData.maxDef;
                playerScript.minAtk = playerData.minAtk;
                playerScript.maxAtk = playerData.maxAtk;
                playerScript.strength = playerData.strength;
                playerScript.agility = playerData.agility;
                playerScript.intellect = playerData.intellect;
                playerScript.charisma = playerData.charisma;
                playerScript.vitality = playerData.vitality;
            }
        }
    }
    [Serializable]
    public class AllData
    {
        public PlayerData playerData;
    }
    [Serializable]
    public class PlayerData
    {
        public List<int> inventoryId = new List<int>();
        public List<int> inventoryStacks = new List<int>();
        public List<int> inventoryEquippedId = new List<int>();
        public float maxWeight;
        public float currentWeight;
        public int maxHealth;
        public int currentHealth;
        public int maxMana;
        public int currentMana;
        public int maxStamina;
        public int currentStamina;
        public int maxExp;
        public int currentExp;
        public bool isCaster;
        public int level;
        public int statPoints;
        public string playerClass;
        public int speed;
        public float dodge;
        public float crit;
        public int minDef;
        public int maxDef;
        public int minAtk;
        public int maxAtk;
        public int strength;
        public int agility;
        public int intellect;
        public int charisma;
        public int vitality;
    }

    [Serializable, XmlRoot("EnemyUnits")]
    public class EnemyUnits
    {
        [XmlElement("Unit")]
        public List<EnemyUnit> units = new List<EnemyUnit>();
    }

    [Serializable]
    public class EnemyUnit
    {
        [XmlAttribute("Id")]
        public int id;
        [XmlElement("Name")]
        public string name;
        [XmlElement("Slot")]
        public int slot;
        [XmlElement("Atkrange")]
        public int range;
        [XmlElement("Hp")]
        public int hp;
        [XmlElement("Minatk")]
        public int minatk;
        [XmlElement("Maxatk")]
        public int maxatk;
        [XmlElement("Def")]
        public int def;
        [XmlElement("Spd")]
        public int spd;
    }
}
