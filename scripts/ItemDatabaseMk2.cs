using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabaseMk2 : MonoBehaviour
{
    public List<ItemsMk2> items = new List<ItemsMk2>();

    void Start()
    {
        items.Add(new ItemsMk2(0, ItemsMk2.ItemType.Weapon, "Iron Sword", "A simple iron sword", 2.5f, 25, 1, 5, 12, 0.05f, 3, true, false, true, false));
        items.Add(new ItemsMk2(1, ItemsMk2.ItemType.Weapon, "Quick Dagger", "A fast dagger", 1, 20, 1, 2, 6, 0.15f, 1, false, true, true, false));
        items.Add(new ItemsMk2(2, ItemsMk2.ItemType.Armor, "Helmet", "Muum, look, i am a knight now! ", 4, 55, 1, 2, 4, 2, true, false, false, false, false));
        items.Add(new ItemsMk2(3, ItemsMk2.ItemType.Armor, "Jacket", "I will need your bike too", 5, 105, 1, 5, 8, 1, false, true, false, false, false));
        items.Add(new ItemsMk2(4, ItemsMk2.ItemType.Armor, "Leather Gloves", "Tough leather gloves", 3, 60, 1, 1, 2, 1, false, false, true, false, false));
        items.Add(new ItemsMk2(5, ItemsMk2.ItemType.Armor, "Leather Pants", "Tough leather pants", 2, 80, 1, 3, 5, 2, false, false, false, true, false));
        items.Add(new ItemsMk2(6, ItemsMk2.ItemType.Armor, "Cool boots", "You cant just buy these everywhere", 2, 50, 1, 2, 3, 2, false, false, false, false, true));
        items.Add(new ItemsMk2(7, ItemsMk2.ItemType.Jewelry, "Necklace of Mana", "Feel the mana flowing(through your anus)", 0.5f, 160, 1, 50, 0, 0, true, false));
        items.Add(new ItemsMk2(8, ItemsMk2.ItemType.Jewelry, "Ring of Health", "More Health for your money", 0.3f, 120, 1, 0, 50, 0, false, true));
        items.Add(new ItemsMk2(9, ItemsMk2.ItemType.Jewelry, "Ring of Stamina", "No need to jog anymore", 0.3f, 120, 1, 0, 0, 50, false, true));
        items.Add(new ItemsMk2(10, ItemsMk2.ItemType.Misc, "A rock", "A simple rock", 2, 1, 99, false, false, false));
        items.Add(new ItemsMk2(11, ItemsMk2.ItemType.Misc, "Holy Chalice", "A sacred relic you bought for 10 gp from that fat merchant  ", 1.5f, 10, 1, false, false, false));
        items.Add(new ItemsMk2(12, ItemsMk2.ItemType.Misc, "Iron ore", "Chunks of flesh ripped out from Mother Earth", 0.2f, 5, 99, false, true, false));
        items.Add(new ItemsMk2(13, ItemsMk2.ItemType.Misc, "Sanya", "- When does the gayclub open ?" + '\n' + "Sanya(c)", 3, 0, 1, false, false, true));
        items.Add(new ItemsMk2(14, ItemsMk2.ItemType.Misc, "Gem", "Thank you for the donation", 0.1f, 0, 99, true, false, false));
        items.Add(new ItemsMk2(15, ItemsMk2.ItemType.Consumable, "Health Potion", "Restore Health", 0.5f, 20, 10, 20, 0, 0));
        items.Add(new ItemsMk2(16, ItemsMk2.ItemType.Consumable, "Mana Potion", "Restore Mana", 0.5f, 20, 10, 0, 20, 0));
        items.Add(new ItemsMk2(17, ItemsMk2.ItemType.Consumable, "Stamina Potion", "Restore Stamina", 0.5f, 20, 10, 0, 0, 20));

    }
}
